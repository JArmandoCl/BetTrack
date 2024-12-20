using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand SignUpCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        #endregion
        public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            SignUpCommand = new DelegateCommand(SignUp);
            CancelCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());
        }

        private void SignUp()
        {
            IsBusy = true;
        }
    }
}
