using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class BankrollDashboardPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand<DtoApuesta> SelectedBetChangedCommand { get; set; }
        public DelegateCommand NewBetCommand { get => new DelegateCommand(NewBet); }
        public DelegateCommand<int?> TabSelectedCommand { get => new DelegateCommand<int?>(TabSelected); }
        private ObservableCollection<DtoApuesta> bets = new();
        public ObservableCollection<DtoApuesta> Bets
        {
            get { return bets; }
            set { SetProperty(ref bets, value); }
        }

        public long SelectedBankroll { get; set; }
        private DtoUsuarioBankroll bankroll;
        public DtoUsuarioBankroll Bankroll
        {
            get { return bankroll; }
            set { SetProperty(ref bankroll, value); }
        }
        #endregion
        public BankrollDashboardPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }
        private async void TabSelected(int? tabSelected)
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    switch (tabSelected)
                    {
                        case 0:
                            Client = new Api.ApiClient(CurrentUser.CurrentToken);
                            Bets = await Client.GetAsync<ObservableCollection<DtoApuesta>>($"Apuesta/ObtenerApuestas/{SelectedBankroll}");
                            break;

                        case 1:
                            break;

                        case 2:
                            break;
                        default:
                            break;
                    }
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

        private async void NewBet()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    string option = await PageDialogService.DisplayActionSheetAsync(AppResource.LblBetType, AppResource.BtnCancel, null, AppResource.LblSingleBet, AppResource.LblParleyBet);
                    if (option != null && option != AppResource.BtnCancel)
                    {
                        INavigationResult navigationResult = await NavigationService.NavigateAsync("NewBetPage", new NavigationParameters { { "BetType", option }, { "SelectedBankroll", SelectedBankroll } });
                    }
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

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("SelectedBankroll"))
            {
                SelectedBankroll = (long)parameters["SelectedBankroll"];
                try
                {
                    if (!IsBusy)
                    {
                        IsBusy = true;
                        Client = new Api.ApiClient(CurrentUser.CurrentToken);
                        Bankroll = await Client.GetAsync<DtoUsuarioBankroll>($"UsuarioBankroll/{SelectedBankroll}");
                        Bets = await Client.GetAsync<ObservableCollection<DtoApuesta>>($"Apuesta/ObtenerApuestas/{SelectedBankroll}");
                        Title = Bankroll.NombreBankroll;
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
            else if (parameters.ContainsKey("NewBet"))
            {
                var newBet = parameters["NewBet"] as DtoApuesta;
                if (newBet != null)
                {
                    Bets.Add(newBet);
                }
            }
        }
    }
}
