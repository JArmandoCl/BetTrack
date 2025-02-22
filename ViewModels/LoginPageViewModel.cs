using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ShowForgotPassword { get; set; }
        public DelegateCommand ShowSignUpCommand { get; set; }
        public DtoUsuario Usuario { get; set; } = new DtoUsuario();
        private bool rememberMe;
        public bool RememberMe
        {
            get { return rememberMe; }
            set { SetProperty(ref rememberMe, value); }
        }
        #endregion
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Login";
            LoginCommand = new DelegateCommand(Login);
            ShowForgotPassword = new DelegateCommand(async () => await NavigationService.NavigateAsync("ForgotPasswordPage"));
            ShowSignUpCommand = new DelegateCommand(ShowSignUp);
        }
        private async void ShowSignUp()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    await NavigationService.NavigateAsync("SignUpPage");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void Login()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Client = new ApiClient(await SecureStorage.GetAsync("UserToken") ?? "");
                    DtoUsuario currentUser = await Client.PostAsync<DtoUsuario, DtoUsuario>($"Autorizacion", Usuario);
                    if (!string.IsNullOrWhiteSpace(currentUser.CurrentToken))
                    {
                        CurrentUser = currentUser;
                        await SecureStorage.SetAsync("CurrentUser", JsonSerializer.Serialize(currentUser));
                        Preferences.Default.Set("RememberMeEnabled", RememberMe);

                        INavigationResult result = await NavigationService.NavigateAsync("//NavigationPage/HomePage");
                    }
                }
            }
            catch (Exception e)
            {
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
