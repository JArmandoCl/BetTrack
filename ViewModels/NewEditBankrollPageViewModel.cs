using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Models;
using BetTrack.Resources.Languages;
using Prism.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class NewEditBankrollPageViewModel : ViewModelBase
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
        public DelegateCommand DeleteBankrollCommand { get { return new DelegateCommand(()=>UpdateBankroll("Delete")); } }
        public DelegateCommand ArchiveBankrollCommand { get { return new DelegateCommand(()=>UpdateBankroll("Archive")); } }

        public DelegateCommand CreateEditBankrollCommand { get { return new DelegateCommand(CreateEditBankroll); } }
        private DtoUsuarioBankroll bankroll = new DtoUsuarioBankroll();
        private DtoMoneda selectedCurrency = new();
        private DtoFormatoCuota selectedOdd = new();
        private DtoTipoBankroll selectedBankrollType = new();

        public DtoUsuarioBankroll Bankroll
        {
            get { return bankroll; }
            set { SetProperty(ref bankroll, value); }
        }
        public DtoMoneda SelectedCurrency { get => selectedCurrency; set => SetProperty(ref selectedCurrency, value); }
        public DtoFormatoCuota SelectedOdd { get => selectedOdd; set => SetProperty(ref selectedOdd, value); }
        public DtoTipoBankroll SelectedBankrollType { get => selectedBankrollType; set => SetProperty(ref selectedBankrollType, value); }
        #endregion
        public NewEditBankrollPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }
        private async void UpdateBankroll(string action)
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    switch (action)
                    {
                        case "Delete":
                            if (Bankroll.UsuarioBankrollId > 0)
                            {
                                bool confirm = await PageDialogService.DisplayAlertAsync(AppResource.TitleDeleteBankroll, AppResource.LblConfirmDeleteBankroll, AppResource.BtnYes, AppResource.BtnCancel);
                                if (confirm)
                                {
                                    Bankroll.EstatusBankrollId = 3;
                                }
                            }
                            break;
                        case "Archive":
                            if (Bankroll.UsuarioBankrollId > 0)
                            {
                                bool confirm = await PageDialogService.DisplayAlertAsync(AppResource.TitleArchiveBankroll, AppResource.LblConfirmArchiveBankroll, AppResource.BtnYes, AppResource.BtnCancel);
                                if (confirm)
                                {
                                    Bankroll.EstatusBankrollId = 2;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    await Client.PutAsync($@"UsuarioBankroll/{Bankroll.UsuarioBankrollId}", Bankroll);
                    await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSuccess, AppResource.BtnClose);
                    await NavigationService.GoBackAsync();
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
        private async void CreateEditBankroll()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Bankroll.MonedaId = SelectedCurrency?.MonedaId ?? 0;
                    Bankroll.TipoBankrollId = SelectedBankrollType?.TipoBankrollId ?? 0;
                    Bankroll.FormatoCuotaId = SelectedOdd?.FormatoCuotaId ?? 0;
                    Errors = Utilities.ValidateModel(Bankroll);
                    if (Errors.Any())
                    {
                        RaisePropertyChanged(nameof(Errors));
                    }
                    else
                    {
                        if (Bankroll.UsuarioBankrollId > 0)
                        {
                            //Edit
                            Bankroll.FechaModificacion = ApiClient.GetCurrentDateTime();
                            await Client.PutAsync($@"UsuarioBankroll/{Bankroll.UsuarioBankrollId}", Bankroll);
                        }
                        else
                        {
                            //New
                            Bankroll.FechaRegistro = ApiClient.GetCurrentDateTime();
                            Bankroll.FechaModificacion = ApiClient.GetCurrentDateTime();
                            Bankroll.UsuarioId = CurrentUser.UsuarioId;
                            Bankroll = await Client.PostAsync<DtoUsuarioBankroll, DtoUsuarioBankroll>(@"UsuarioBankroll", Bankroll);
                        }
                        await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSuccess, AppResource.BtnClose);
                        await NavigationService.GoBackAsync();
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
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (!IsBusy)
                {
                    base.OnNavigatedTo(parameters);
                    IsBusy = true;
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    if (parameters.ContainsKey("SelectedBankroll"))
                    {
                        long bankrollId = parameters.GetValue<long>("SelectedBankroll");
                        Bankroll = await Client.GetAsync<DtoUsuarioBankroll>($"UsuarioBankroll/{bankrollId}");

                        Bankroll.Monedas = await Client.GetAsync<List<DtoMoneda>>(@"Catalogo/Monedas");
                        SelectedCurrency = Bankroll.Monedas.FirstOrDefault(x => x.MonedaId == (Bankroll.UsuarioBankrollId == 0 ? 16 : Bankroll.MonedaId));
                        Bankroll.FormatoCuotas = await Client.GetAsync<List<DtoFormatoCuota>>(@"Catalogo/FormatoCuota");
                        SelectedOdd = Bankroll.FormatoCuotas.FirstOrDefault(x => x.FormatoCuotaId == (Bankroll.FormatoCuotaId == 0 ? 1 : Bankroll.FormatoCuotaId));
                        Bankroll.TiposBankroll = await Client.GetAsync<List<DtoTipoBankroll>>(@"Catalogo/TipoBankroll");
                        SelectedBankrollType = Bankroll.TiposBankroll.FirstOrDefault(x => x.TipoBankrollId == (Bankroll.TipoBankrollId == 0 ? 1 : Bankroll.TipoBankrollId));
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
    }
}
