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
        #region Object declarations
        
        #endregion
        public DashboardPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Mi Dashboard";
            
        }

        
    }
}
