using BetTrack.Resources.Languages;
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
        public DelegateCommand NewBetCommand { get => new DelegateCommand(NewBet); }

        #endregion
        public BankrollDashboardPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {        
        }

        
        private async void NewBet()
        {
            try
            {
                Application.Current.UserAppTheme = AppTheme.Dark;//Light mode doesn´t show entry bottom line
                if (!IsBusy)
                {
                    IsBusy = true;
                    string option = await PageDialogService.DisplayActionSheetAsync(AppResource.LblBetType,AppResource.BtnCancel,null,AppResource.LblSingleBet,AppResource.LblParleyBet);

                    await NavigationService.NavigateAsync("NewBetPage", new NavigationParameters { { "BetType", option } });
                }
            }
            catch (UnauthorizedAccessException e)
            {
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSessionExpired, AppResource.BtnClose);
                SecureStorage.RemoveAll();
                Preferences.Default.Clear();
                INavigationResult result = await NavigationService.NavigateAsync("//LoginPage");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
