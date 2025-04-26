using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using Prism.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class NewEditBankrollPageViewModel : ViewModelBase
    {
        #region Object declarations   
        public DelegateCommand CancelCommand { get { return new DelegateCommand(async () => { await NavigationService.GoBackAsync(); }); } }
        public DelegateCommand CreateEditBankrollCommand { get { return new DelegateCommand(CreateEditBankroll); } }
        private DtoUsuarioBankroll bankroll = new DtoUsuarioBankroll();
        public DtoUsuarioBankroll Bankroll
        {
            get { return bankroll; }
            set { SetProperty(ref bankroll, value); }
        }
        #endregion
        public NewEditBankrollPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }
        private async void CreateEditBankroll()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    if (Bankroll.UsuarioBankrollId > 0)
                    {
                        //Edit
                        Bankroll.FechaModificacion = ApiClient.GetCurrentDateTime();
                        await Client.PutAsync($@"UsuarioBankroll/{Bankroll.UsuarioBankrollId}",Bankroll);
                    }
                    else
                    {
                        //New
                        Bankroll.FechaRegistro = ApiClient.GetCurrentDateTime();
                        Bankroll.FechaModificacion = ApiClient.GetCurrentDateTime();
                        Bankroll.UsuarioId = CurrentUser.UsuarioId;
                        Bankroll = await Client.PostAsync<DtoUsuarioBankroll, DtoUsuarioBankroll>(@"UsuarioBankroll", Bankroll);
                    }
                    await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSuccess, AppResource.BtnClose);
                    await NavigationService.GoBackAsync();
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
            try
            {
                if (!IsBusy)
                {
                    base.OnNavigatedTo(parameters);
                    IsBusy = true;
                    Client = new Api.ApiClient(CurrentUser.CurrentToken);
                    if (parameters.ContainsKey("SelectedBankroll"))
                    {
                        long bankrollId = parameters.GetValue<long>("SelectedBankroll");
                        Bankroll = await Client.GetAsync<DtoUsuarioBankroll>($"UsuarioBankroll/{bankrollId}");
                    }
                    else
                    {
                        Bankroll.Monedas = await Client.GetAsync<List<DtoMoneda>>(@"UsuarioBankroll/Monedas");
                        Bankroll.FormatoCuotas = await Client.GetAsync<List<DtoFormatoCuota>>(@"UsuarioBankroll/FormatoCuota");
                        Bankroll.TiposBankroll = await Client.GetAsync<List<DtoTipoBankroll>>(@"UsuarioBankroll/TipoBankroll");
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
    }
}
