using BetTrack.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        #region Object declarations   
        public DelegateCommand GoToTipstersList { get; set; }
        #endregion
        public HomePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            GoToTipstersList = new DelegateCommand(async () => { await navigationService.NavigateAsync("TipstersList"); });
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
