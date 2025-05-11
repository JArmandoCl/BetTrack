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
    public class HomePageViewModel : ViewModelBase
    {
        #region Object declarations   
        private ObservableCollection<DtoUsuarioBankroll> bankrollsUser;
        public ObservableCollection<DtoUsuarioBankroll> BankrollsUser
        {
            get { return bankrollsUser; }
            set { SetProperty(ref bankrollsUser, value); }
        }
        public DelegateCommand<DtoUsuarioBankroll> SelectedCasinoChangedCommand { get { return new DelegateCommand<DtoUsuarioBankroll>(x => SelectedCasinoChanged(x)); } }
        public DelegateCommand GoToTipstersList { get { return new DelegateCommand(async () => { await NavigationService.NavigateAsync("TipstersListPage"); }); } }
        public DelegateCommand GoToUserProfile { get { return new DelegateCommand(async () => { await NavigationService.NavigateAsync("ProfilePage"); }); } }
        public DelegateCommand GoToUserCasinosList { get { return new DelegateCommand(async () => { INavigationResult result = await NavigationService.NavigateAsync("UserCasinosPage"); Debug.WriteLine(result); }); } }
        public DelegateCommand GoToNewEditBankroll { get { return new DelegateCommand(async () => { INavigationResult result = await NavigationService.NavigateAsync("NewEditBankrollPage"); Debug.WriteLine(result); }); } }
        public DelegateCommand GoToUserCategories { get { return new DelegateCommand(async () => { INavigationResult result = await NavigationService.NavigateAsync("CategoriesPage"); Debug.WriteLine(result); }); } }
        #endregion
        public HomePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        public async void SelectedCasinoChanged(DtoUsuarioBankroll selectedCasino, bool isEditing = false)
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (isEditing)
                        await NavigationService.NavigateAsync("NewEditBankrollPage", new NavigationParameters { { "SelectedBankroll", selectedCasino.UsuarioBankrollId } });
                    else
                        await NavigationService.NavigateAsync("BankrollDashboardPage", new NavigationParameters { { "SelectedBankroll", selectedCasino.UsuarioBankrollId } });
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
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    await LoadBasicInfo();
                    if (!string.IsNullOrWhiteSpace(CurrentUser?.FotoUsuario))
                    {
                        ProfilePhoto = CurrentUser.FotoUsuario;
                    }
                    //Load user bankrolls
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    BankrollsUser = await Client.GetAsync<ObservableCollection<DtoUsuarioBankroll>>($"UsuarioBankroll/ObtenerBankrollsUsuario/{CurrentUser.UsuarioId}");
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
