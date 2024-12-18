using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        public DelegateCommand ShowRegistroCommand { get; set; }
        public DashboardPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Mi Dashboard";
            ShowRegistroCommand = new DelegateCommand(ShowRegistro);
        }

        private async void ShowRegistro()
        {
            await NavigationService.NavigateAsync("RegistroPage");
        }
    }
}
