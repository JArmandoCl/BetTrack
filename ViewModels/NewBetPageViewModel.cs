using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public DtoDeporte DeporteSeleccionado { get; set; }
        public DtoUsuarioTipster TipsterSeleccionado { get; set; }
        public DtoCategoriaUsuario CategoriaSeleccionada { get; set; }
        public DtoEstatusApuesta EstatusApuestaSeleccionado { get; set; }
        #endregion
        #endregion
        public NewBetPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            SaveBetCommand = new DelegateCommand(SaveBet);
        }
        public void SaveBet()
        {
            
        }
        private async Task LoadCombos()
        {
            Apuesta.Tipsters = await Client.GetAsync<List<DtoUsuarioTipster>>(@$"UsuarioTipster\ObtenerUsuarioTipsters\{CurrentUser.UsuarioId}");
            Apuesta.Categorias = await Client.GetAsync<List<DtoCategoriaUsuario>>(@$"CategoriaUsuario\ObtenerCategoriasUsuarios\{CurrentUser.UsuarioId}");
            Apuesta.TiposApuesta = await Client.GetAsync<List<DtoTipoApuesta>>(@$"Catalogo\TiposApuesta");
            Apuesta.DetalleApuesta.EstatusApuesta = await Client.GetAsync<List<DtoEstatusApuesta>>(@$"Catalogo\EstatusApuesta");
            Apuesta.DetalleApuesta.Deportes = await Client.GetAsync<List<DtoDeporte>>(@$"Catalogo\Deportes\{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}");
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            BetType = parameters["BetType"] as string;
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
