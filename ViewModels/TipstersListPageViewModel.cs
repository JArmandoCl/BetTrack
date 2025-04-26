using Android.App.AppSearch;
using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BetTrack.ViewModels
{
    public class TipstersListPageViewModel : ViewModelBase
    {
        #region Object declarations 
        private DtoUsuarioTipster selectedTipster;
        public DtoUsuarioTipster SelectedTipster
        {
            get { return selectedTipster; }
            set { SetProperty(ref selectedTipster, value); }
        }
        private ObservableCollection<DtoUsuarioTipster> tipsters = new ObservableCollection<DtoUsuarioTipster>();
        public ObservableCollection<DtoUsuarioTipster> Tipsters
        {
            get { return tipsters; }
            set { SetProperty(ref tipsters, value); }
        }
        ObservableCollection<DtoUsuarioTipster> BackupTipsters;
        public DelegateCommand GoToNewTipsterCommand { get; set; }
        public DelegateCommand PerformActionToTipsterCommand => new DelegateCommand(PerformActionToTipster);
        #region Search
        public DelegateCommand<string> PerformSearchCommand { get; set; }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); PerformSearch(searchText); }
        }
        #endregion
        #endregion
        public TipstersListPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            GoToNewTipsterCommand = new DelegateCommand(NewTipster);
            PerformSearchCommand = new DelegateCommand<string>(PerformSearch);
        }

        private async Task DeleteTipster()
        {
            try
            {
                if (!IsBusy)
                {
                    if (SelectedTipster != null)
                    {
                        string deleteLabel = AppResource.LblConfirmTipsterDelete;
                        deleteLabel = deleteLabel.Trim().Replace("$tipster$", SelectedTipster.NombreTipster);
                        bool result = await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, deleteLabel, AppResource.BtnContinue, AppResource.BtnCancel);
                        if (result)
                        {
                            IsBusy = true;
                            Client = new ApiClient(CurrentUser.CurrentToken);
                            bool updated = await Client.DeleteAsync($"UsuarioTipster/{SelectedTipster.UsuarioTipsterId}");
                            if (updated)
                                await LoadTipsters();
                            else
                                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
                        }
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
        private async Task EditTipster()
        {
            if (SelectedTipster != null)
            {
                Application.Current.UserAppTheme = AppTheme.Dark;//Light mode doesn´t show entry bottom line

                string newTipsterLabel = AppResource.LblNewTipster;
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (currentCulture == "es")
                {
                    newTipsterLabel = newTipsterLabel.Replace("$action$", "Editar");
                }
                else if (currentCulture == "en")
                {
                    newTipsterLabel = newTipsterLabel.Replace("$action$", "Edit");
                }
                string newTipster = await PageDialogService.DisplayPromptAsync(newTipsterLabel, AppResource.LblTipsterName, AppResource.LblSave, AppResource.BtnCancel, initialValue: SelectedTipster.NombreTipster.Trim(), keyboardType: KeyboardType.Text);
                if (newTipster != null && newTipster.Trim().Equals(""))
                {
                    await ShowToast(AppResource.LblTipsterNameRequired, ToastDuration.Short);
                }
                else if (!string.IsNullOrWhiteSpace(newTipster))
                {
                    IsBusy = true;
                    SelectedTipster.NombreTipster = newTipster.Trim();
                    Client = new ApiClient(CurrentUser.CurrentToken);
                    await Client.PutAsync($"UsuarioTipster/{SelectedTipster.UsuarioTipsterId}", SelectedTipster);
                    await LoadTipsters();
                    await ShowToast(AppResource.LblSuccess, ToastDuration.Short);
                }
            }
        }

        public void PerformSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Tipsters = BackupTipsters;
            }
            else
            {
                Tipsters = new ObservableCollection<DtoUsuarioTipster>(BackupTipsters.Where(c => c.NombreTipster.Contains(query)));
            }
        }

        public async void PerformActionToTipster()
        {
            try
            {
                if (!IsBusy)
                {
                    if (SelectedTipster != null)
                    {
                        Application.Current.UserAppTheme = AppTheme.Dark;
                        string result = await PageDialogService.DisplayActionSheetAsync(AppResource.LblChooseAnAction, AppResource.BtnCancel, null, $"{AppResource.LblEdit} tipster", $"{AppResource.LblDelete} tipster");
                        if (result.Equals($"{AppResource.LblEdit} tipster"))
                        {
                            await EditTipster();
                        }
                        else if (result.Equals($"{AppResource.LblDelete} tipster"))
                        {
                            await DeleteTipster();
                        }
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
                Application.Current.UserAppTheme = AppTheme.Light;
                SelectedTipster = null;
                IsBusy = false;
            }
        }

        private async void NewTipster()
        {
            try
            {
                if (!IsBusy)
                {
                    Application.Current.UserAppTheme = AppTheme.Dark;//Light mode doesn´t show entry bottom line

                    var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    string newTipsterLabel = AppResource.LblNewTipster;
                    if (currentCulture == "es")
                    {
                        newTipsterLabel = newTipsterLabel.Replace("$action$", "Nuevo");
                    }
                    else if (currentCulture == "en")
                    {
                        newTipsterLabel = newTipsterLabel.Replace("$action$", "New");
                    }
                    string newTipster = await PageDialogService.DisplayPromptAsync(newTipsterLabel, AppResource.LblTipsterName, AppResource.LblSave, AppResource.BtnCancel, keyboardType: KeyboardType.Text);
                    if (newTipster != null && newTipster.Trim().Equals(""))
                    {
                        await ShowToast(AppResource.LblTipsterNameRequired, ToastDuration.Short);
                    }
                    else if (!string.IsNullOrWhiteSpace(newTipster))
                    {
                        IsBusy = true;
                        DtoUsuarioTipster tipster = new DtoUsuarioTipster
                        {
                            NombreTipster = newTipster.Trim(),
                            UsuarioId = CurrentUser.UsuarioId,
                            FechaRegistro = ApiClient.GetCurrentDateTime()
                        };
                        Client = new ApiClient(CurrentUser.CurrentToken);
                        tipster = await Client.PostAsync<DtoUsuarioTipster, DtoUsuarioTipster>("UsuarioTipster", tipster);
                        if (tipster.UsuarioTipsterId > 0)
                        {
                            Tipsters.Add(tipster);
                            await ShowToast(AppResource.LblSuccess, ToastDuration.Short);
                        }
                        else
                        {
                            await ShowToast(AppResource.LblFailed, ToastDuration.Short);
                        }
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
                Application.Current.UserAppTheme = AppTheme.Light;
                IsBusy = false;
            }
        }

        public async Task LoadTipsters()
        {
            Tipsters = await Client.GetAsync<ObservableCollection<DtoUsuarioTipster>>(@$"UsuarioTipster\ObtenerUsuarioTipsters\{CurrentUser.UsuarioId}");
            BackupTipsters = Tipsters;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Client = new ApiClient(CurrentUser.CurrentToken);
                    await LoadTipsters();
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
