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
    public class UserCasinosPageViewModel : ViewModelBase
    {
        #region Object declarations   
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand GoToImportCasinoCommand { get; set; }
        public DelegateCommand PerformActionToCasinoCommand => new DelegateCommand(PerformActionToCasino);
        #region Search
        public DelegateCommand<string> PerformSearchCommand { get; set; }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); PerformSearch(searchText); }
        }
        #endregion
        private IList<object> selectedCasinos;
        public IList<object> SelectedCasinos
        {
            get { return selectedCasinos; }
            set { SetProperty(ref selectedCasinos, value); }
        }
        private ObservableCollection<DtoUsuarioCasino> userCasinos = new ObservableCollection<DtoUsuarioCasino>();
        public ObservableCollection<DtoUsuarioCasino> UserCasinos
        {
            get { return userCasinos; }
            set { SetProperty(ref userCasinos, value); }
        }
        ObservableCollection<DtoUsuarioCasino> BackupUserCasinos;
        private bool canDelete = false;
        public bool CanDelete
        {
            get { return canDelete; }
            set { SetProperty(ref canDelete, value); }
        }
        #endregion
        public UserCasinosPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            GoToImportCasinoCommand = new DelegateCommand(ImportCasino);
            PerformSearchCommand = new DelegateCommand<string>(PerformSearch);
            DeleteCommand = new DelegateCommand(Delete).ObservesCanExecute(() => CanDelete);
        }

        private async void Delete()
        {
            bool confirmResult = await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblConfirmDeleteCasinos, AppResource.BtnContinue, AppResource.BtnCancel);
            if (confirmResult)
            {
                try
                {
                    if (!IsBusy)
                    {
                        IsBusy = true;
                        Client = new Api.ApiClient(CurrentUser.CurrentToken);
                        foreach (DtoUsuarioCasino usuarioCasinoActual in SelectedCasinos.Select(x => (DtoUsuarioCasino)x))
                        {
                            await Client.DeleteAsync($@"UsuarioCasino\{usuarioCasinoActual.UsuarioCasinoId}");
                        }
                        SelectedCasinos.Clear();
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
                    await LoadUserCasinos();
                }
            }
        }

        private void PerformActionToCasino()
        {
            if (SelectedCasinos != null)
                CanDelete = SelectedCasinos.Count > 0;
        }

        private async void ImportCasino()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    await NavigationService.NavigateAsync("BetTrackCasinosPage");
                }
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
            if (string.IsNullOrWhiteSpace(query))
            {
                UserCasinos = BackupUserCasinos;
            }
            else
            {
                UserCasinos = new ObservableCollection<DtoUsuarioCasino>(BackupUserCasinos.Where(c => c.Nombre.Contains(query)));
            }
        }
        public async Task LoadUserCasinos()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    UserCasinos = await Client.GetAsync<ObservableCollection<DtoUsuarioCasino>>(@$"UsuarioCasino\ObtenerUsuarioCasinos\{CurrentUser.UsuarioId}");
                    BackupUserCasinos = UserCasinos;
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
            base.OnNavigatedTo(parameters);
            await LoadUserCasinos();
        }
    }
}
