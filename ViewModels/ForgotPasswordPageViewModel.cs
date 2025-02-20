using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private string email = "";
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        #endregion
        public ForgotPasswordPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);
            CancelCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());
        }

        private async void ForgotPassword()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Client = new ApiClient(await SecureStorage.GetAsync("UserToken")??"");
                    string result = await Client.PostAsync<DtoReestablecerContrasenia, string>($"Usuario/solicitar-reestablecimiento", new DtoReestablecerContrasenia
                    {
                        Email = Email
                    });
                    if (result.StartsWith("200-"))
                    {
                        await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblSendForgotPasswordInstructions, AppResource.BtnClose);
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
                    }
                    await NavigationService.GoBackAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
