using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class BankrollDashboardPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand NewBetCommand { get => new DelegateCommand(async () => await NavigationService.NavigateAsync("NewBetPage")); }
        #endregion
        public BankrollDashboardPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Mi Dashboard";            
        }

        
    }
}
