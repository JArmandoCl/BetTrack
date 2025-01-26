using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ShowForgotPassword { get; set; }
        public DelegateCommand ShowSignUpCommand { get; set; }
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
            await NavigationService.NavigateAsync("SignUpPage");
        }

        private async void Login()
        {
            INavigationResult result = await NavigationService.NavigateAsync("//NavigationPage/HomePage");
        }
    }
}
