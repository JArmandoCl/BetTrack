using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class BetTrackCasinosPageViewModel : ViewModelBase
    {
        #region Object declarations   
        #region Search
        public DelegateCommand<string> PerformSearchCommand { get; set; }
        public DelegateCommand GoToImportCasinosCommand { get; set; }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value);
                if (string.IsNullOrEmpty(searchText))
                {
                    PerformSearch(searchText);
                }
            }
        }
        #endregion
        private ObservableCollection<DtoCasino> casinos;
        public ObservableCollection<DtoCasino> Casinos
        {
            get { return casinos; }
            set { SetProperty(ref casinos, value); }
        }
        public ObservableCollection<DtoCasino> BackupCasinos { get; set; }
        private IList<object> selectedCasinos;
        public IList<object> SelectedCasinos
        {
            get { return selectedCasinos; }
            set { SetProperty(ref selectedCasinos, value); }
        }
        #endregion
        public BetTrackCasinosPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            PerformSearchCommand = new DelegateCommand<string>(PerformSearch);
            GoToImportCasinosCommand = new DelegateCommand(GoToImportCasinos);
        }

        private async void GoToImportCasinos()
        {
            try
            {
                if (!IsBusy)
                {
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    List<DtoUsuarioCasino> importedCasinos = new List<DtoUsuarioCasino>();
                    foreach (DtoCasino usuarioCasinoActual in SelectedCasinos.Select(x => (DtoCasino)x))
                    {
                        DtoUsuarioCasino newUsuarioCasino = new DtoUsuarioCasino
                        {
                            EstatusUsuarioCasinoId = 1,
                            Icono = usuarioCasinoActual.Icono,
                            Nombre = usuarioCasinoActual.Nombre,
                            UsuarioId = CurrentUser.UsuarioId,
                            FechaRegistro = ApiClient.GetCurrentDateTime(),
                            CasinoId = usuarioCasinoActual.CasinoId
                        };
                        newUsuarioCasino = await Client.PostAsync<DtoUsuarioCasino, DtoUsuarioCasino>($"UsuarioCasino", newUsuarioCasino);
                        importedCasinos.Add(newUsuarioCasino);
                    }
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

        public void PerformSearch(string query)
        {
            try
            {
                IsBusy = true;
                if (string.IsNullOrWhiteSpace(query))
                {
                    Casinos = BackupCasinos;
                }
                else
                {
                    Casinos = new ObservableCollection<DtoCasino>(BackupCasinos.Where(c => c.Nombre.ToLower().Contains(query.ToLower())));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task LoadCasinos()
        {
            Casinos = await Client.GetAsync<ObservableCollection<DtoCasino>>(@$"UsuarioCasino\ObtenerCasinosBetTrack");
            BackupCasinos = Casinos;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    await LoadCasinos();
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
