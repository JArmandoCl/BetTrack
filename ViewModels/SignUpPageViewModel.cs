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
    public class SignUpPageViewModel : ViewModelBase
    {
        #region Object declarations
        public DelegateCommand SignUpCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DtoUsuario Usuario { get; set; } = new DtoUsuario();
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
                    Client = new ApiClient();
                    Usuario = await Client.PostAsync<DtoUsuario, DtoUsuario>($"Usuario", Usuario);
                    await PageDialogService.DisplayAlertAsync(Resources.Languages.AppResource.LblSignUpSuccessful, AppResource.LblSignUpSuccessfulMessage, AppResource.BtnClose);
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
