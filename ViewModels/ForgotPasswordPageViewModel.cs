using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Models;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelBase
    {

        #region Errors handle
        private Dictionary<string, string> _errors = new();
        public Dictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        #endregion
        #region Object declarations
        public DelegateCommand ForgotPasswordCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DtoReestablecerContrasenia RecoverPassword { get; set; } = new();
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
                    Errors = Utilities.ValidateModel(RecoverPassword);
                    if (Errors.Any())
                    {
                        RaisePropertyChanged(nameof(Errors));
                    }
                    else
                    {
                        Client = new ApiClient(await SecureStorage.GetAsync("UserToken") ?? "");
                        string result = await Client.PostAsync<DtoReestablecerContrasenia, string>($"Usuario/solicitar-reestablecimiento", RecoverPassword);
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
