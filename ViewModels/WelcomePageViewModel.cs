using BetTrack.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class WelcomePageViewModel : ViewModelBase
    {
        #region Object declarations       
        public DelegateCommand ShowLoginCommand { get; set; }
        private ObservableCollection<DtoImage> welcomeImages = new ObservableCollection<DtoImage>();
        public ObservableCollection<DtoImage> WelcomeImages
        {
            get { return welcomeImages; }
            set { SetProperty(ref welcomeImages, value); }
        }
        #endregion
        public WelcomePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Welcome";
            WelcomeImages.Add(new DtoImage { Image = "i1" });
            WelcomeImages.Add(new DtoImage { Image = "i2" });
            WelcomeImages.Add(new DtoImage { Image = "i3" });
            WelcomeImages.Add(new DtoImage { Image = "i4" });
            WelcomeImages.Add(new DtoImage { Image = "" });
            ShowLoginCommand = new DelegateCommand(ShowLogin);
        }

        private async void ShowLogin()
        {
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }

}
