using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public DelegateCommand ShowDashboardCommand { get; set; }
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Login";
            ShowDashboardCommand = new DelegateCommand(ShowDashboard);
        }

        private async void ShowDashboard()
        {
            await NavigationService.NavigateAsync("//NavigationPage/DashboardPage");
        }
    }
}
