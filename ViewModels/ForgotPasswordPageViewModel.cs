using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand ForgotPasswordCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        #endregion
        public ForgotPasswordPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);
            CancelCommand = new DelegateCommand(async ()=>await NavigationService.GoBackAsync());
        }

        private void ForgotPassword()
        {
            
        }
    }
}
