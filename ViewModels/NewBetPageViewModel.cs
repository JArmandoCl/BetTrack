using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Models;
using BetTrack.Resources.Languages;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class NewBetPageViewModel : ViewModelBase
    {
        #region Object declarations   
        #region Errors handle
        private Dictionary<string, string> _errors = new();
        public Dictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        #endregion
        public DelegateCommand SaveBetCommand { get; set; }
        public string BetType { get; set; }
        public bool IsParley { get; set; }
        private DtoApuesta apuesta = new DtoApuesta();

        public DtoApuesta Apuesta
        {
            get { return apuesta; }
            set { SetProperty(ref apuesta, value); }
        }
        #region Objetos seleccionados
        private DtoDeporte deporteSeleccionado = new DtoDeporte();
        public DtoDeporte SelectedSport { get => deporteSeleccionado; set => SetProperty(ref deporteSeleccionado, value); }
        private DtoUsuarioTipster tipsterSeleccionado = new DtoUsuarioTipster();
        public DtoUsuarioTipster SelectedTipster { get => tipsterSeleccionado; set => SetProperty(ref tipsterSeleccionado, value); }
        private DtoUsuarioCasino selectedCasino = new DtoUsuarioCasino();
        public DtoUsuarioCasino SelectedCasino
        {
            get { return selectedCasino; }
            set { SetProperty(ref selectedCasino, value); }
        }
        private DtoCategoriaUsuario categoriaSeleccionada = new DtoCategoriaUsuario();
        public DtoCategoriaUsuario SelectedCategory { get => categoriaSeleccionada; set => SetProperty(ref categoriaSeleccionada, value); }
        private DtoEstatusApuesta estatusApuestaSeleccionado = new DtoEstatusApuesta();
        public DtoEstatusApuesta SelectedStatus { get => estatusApuestaSeleccionado; set => SetProperty(ref estatusApuestaSeleccionado, value); }
        #endregion
        #endregion
        public NewBetPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            SaveBetCommand = new DelegateCommand(SaveBet);
        }
        public async void SaveBet()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    //Main
                    Apuesta.CategoriaUsuarioId = SelectedCategory?.CategoriaUsuarioId ?? 0;
                    Apuesta.UsuarioTipsterId = SelectedTipster?.UsuarioTipsterId ?? 0;
                    Apuesta.UsuarioCasinoId = SelectedCasino?.UsuarioCasinoId ?? 0;
                    //Detail
                    Apuesta.DetalleApuesta.DeporteId = SelectedSport?.DeporteId ?? 0;
                    Apuesta.DetalleApuesta.EstatusApuestaId = SelectedStatus?.EstatusApuestaId ?? 0;
                    Apuesta.DetalleApuesta.Nombre = Apuesta.Nombre;

                    Errors = Utilities.ValidateModel(Apuesta);

                    if (Errors.Any())
                    {
                        RaisePropertyChanged(nameof(Errors));
                    }
                    else
                    {
                        Client = new ApiClient(CurrentUser.CurrentToken);
                        if (Apuesta.DetalleApuesta.EstatusApuestaId != 5)//Pendiente-Pending
                        {
                            Apuesta.MontoCobrado = Apuesta.DetalleApuesta.EstatusApuestaId == 4 ? Apuesta.Cashout : Utilities.CalculateTotalPayout(Apuesta.Importe, Apuesta.DetalleApuesta.Cuota, Apuesta.DetalleApuesta.EstatusApuestaId);
                            Apuesta.Ganancia = Apuesta.DetalleApuesta.EstatusApuestaId == 4 ? Apuesta.Cashout - Apuesta.Importe : Utilities.CalculateNetProfit(Apuesta.Importe, Apuesta.DetalleApuesta.Cuota, Apuesta.DetalleApuesta.EstatusApuestaId);
                        }
                        Apuesta = await Client.PostAsync<DtoApuesta, DtoApuesta>($"Apuesta", Apuesta);
                        if (Apuesta.ApuestaId > 0)
                        {
                            await ShowToast(AppResource.LblSuccess, ToastDuration.Short);
                            await NavigationService.GoBackAsync();
                        }
                        else
                        {
                            await ShowToast(AppResource.LblFailed, ToastDuration.Short);
                        }
                    }

                }
            }
            catch (UnauthorizedAccessException e)
            {
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSessionExpired, AppResource.BtnClose);
                SecureStorage.RemoveAll();
                Preferences.Default.Clear();
                INavigationResult result = await NavigationService.NavigateAsync("//LoginPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadCombos()
        {
            Apuesta.Tipsters = await Client.GetAsync<List<DtoUsuarioTipster>>(@$"UsuarioTipster\ObtenerUsuarioTipsters\{CurrentUser.UsuarioId}");
            SelectedTipster = Apuesta.Tipsters.FirstOrDefault();
            Apuesta.Categorias = await Client.GetAsync<List<DtoCategoriaUsuario>>(@$"CategoriaUsuario\ObtenerCategoriasUsuarios\{CurrentUser.UsuarioId}");
            SelectedCategory = Apuesta.Categorias.FirstOrDefault();
            Apuesta.DetalleApuesta.EstatusApuesta = await Client.GetAsync<List<DtoEstatusApuesta>>(@$"Catalogo\EstatusApuesta");
            SelectedStatus = Apuesta.DetalleApuesta.EstatusApuesta.FirstOrDefault(x => x.EstatusApuestaId == 5);
            Apuesta.DetalleApuesta.Deportes = await Client.GetAsync<List<DtoDeporte>>(@$"Catalogo\Deportes\{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}");
            SelectedSport = Apuesta.DetalleApuesta.Deportes.FirstOrDefault(x => x.DeporteId == 1);
            Apuesta.TipoApuestaId = BetType.ToLower().Equals("derecha") || BetType.ToLower().Equals("derecha") ? 1 : 2;
            Apuesta.UserCasinos = await Client.GetAsync<List<DtoUsuarioCasino>>(@$"UsuarioCasino\ObtenerUsuarioCasinos\{CurrentUser.UsuarioId}");
            SelectedCasino = Apuesta.UserCasinos.FirstOrDefault();
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            BetType = parameters["BetType"] as string;
            Apuesta.UsuarioBankrollId = (long)parameters["SelectedBankroll"];
            Title = $"{AppResource.TxtBetBetTitle}-{BetType}";
            IsParley = BetType == AppResource.LblParleyBet;
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    await LoadCombos();
                }
            }
            catch (UnauthorizedAccessException e)
            {
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSessionExpired, AppResource.BtnClose);
                SecureStorage.RemoveAll();
                Preferences.Default.Clear();
                INavigationResult result = await NavigationService.NavigateAsync("//LoginPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
