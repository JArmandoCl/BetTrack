using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Models;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        #region Object declarations

        #region Errors handle
        private Dictionary<string, string> _errors = new();
        public Dictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
        #endregion
        public DelegateCommand SignUpCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DtoUsuario User { get; set; } = new DtoUsuario();
        #endregion
        public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            SignUpCommand = new DelegateCommand(SignUp);
            CancelCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());
        }

        private async void SignUp()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Errors = Utilities.ValidateModel(User);
                    if (Errors.Any())
                    {
                        RaisePropertyChanged(nameof(Errors));
                    }
                    else
                    {
                        Client = new ApiClient(await SecureStorage.GetAsync("UserToken") ?? "");
                        User = await Client.PostAsync<DtoUsuario, DtoUsuario>($"Usuario", User);
                        await PageDialogService.DisplayAlertAsync(Resources.Languages.AppResource.LblSignUpSuccessful, AppResource.LblSignUpSuccessfulMessage, AppResource.BtnClose);
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
