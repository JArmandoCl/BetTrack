using BetTrack.Dtos;
using BetTrack.Models;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
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
        private DtoUsuario user;
        public DtoUsuario User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }
        public DelegateCommand GoToChangePictureCommand { get { return new DelegateCommand(GoToChangePicture); } }
        public DelegateCommand GoToSaveCommand { get { return new DelegateCommand(GoToSave); } }

        #endregion
        public ProfilePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }
        private async void GoToSave()
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

                        await SaveProfile(); 
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
        private async void GoToChangePicture()
        {
            try
            {
                if (!IsBusy)
                {
                    Utilities utilities = new Utilities();
                    string localPath = "";
                    string result = await PageDialogService.DisplayActionSheetAsync(AppResource.LblChooseAnOption, AppResource.BtnCancel, null, AppResource.LblCamera, AppResource.LblGallery);
                    IsBusy = true;
                    if (result.Equals(AppResource.LblCamera))
                    {
                        localPath = await utilities.TakePhoto();
                    }
                    else if (result.Equals(AppResource.LblGallery))
                    {
                        localPath = await utilities.TakePhotoFromGallery();
                    }
                    else
                    {
                        return;
                    }

                    string newProfilePhoto = await utilities.UploadProfilePhoto(localPath);
                    if (!string.IsNullOrEmpty(newProfilePhoto))
                    {
                        User.FotoUsuario = newProfilePhoto;
                        await SaveProfile();
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblFailed, AppResource.BtnClose);
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
                User.FotoUsuario = "default_user_profile.png";
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task SaveProfile()
        {
            Client = new Api.ApiClient(CurrentUser.CurrentToken);
            await Client.PutAsync(@$"Usuario\{User.UsuarioId}", User);
            await SecureStorage.SetAsync("CurrentUser", JsonSerializer.Serialize(User));
            LoadBasicInfo();
            await PageDialogService.DisplayAlertAsync(Resources.Languages.AppResource.LblDialogTitle, AppResource.LblSuccess, AppResource.BtnClose);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            User = (DtoUsuario)CurrentUser.Clone();
        }
    }
}
