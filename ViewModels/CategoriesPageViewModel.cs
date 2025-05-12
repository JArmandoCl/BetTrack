using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class CategoriesPageViewModel : ViewModelBase
    {
        #region Object declarations
        private DtoCategoriaUsuario selectedCategory;
        public DtoCategoriaUsuario SelectedCategory
        {
            get { return selectedCategory; }
            set { SetProperty(ref selectedCategory, value); }
        }
        private ObservableCollection<DtoCategoriaUsuario> categories = new ObservableCollection<DtoCategoriaUsuario>();
        public ObservableCollection<DtoCategoriaUsuario> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }
        ObservableCollection<DtoCategoriaUsuario> BackupCategories;
        public DelegateCommand GoToNewCategoryCommand { get; set; }
        public DelegateCommand PerformActionToCategoryCommand => new DelegateCommand(PerformActionToCategory);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);

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
        public CategoriesPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            GoToNewCategoryCommand = new DelegateCommand(NewCategory);
            PerformSearchCommand = new DelegateCommand<string>(PerformSearch);
        }
        private async void Refresh()
        {
            try
            {
                Client = new ApiClient(CurrentUser.CurrentToken);
                await LoadCategories();
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

        private async Task DeleteCategory()
        {
            try
            {
                if (!IsBusy)
                {
                    if (SelectedCategory != null)
                    {
                        string deleteLabel = AppResource.LblDeleteCategory;
                        deleteLabel = deleteLabel.Trim().Replace("$category$", SelectedCategory.Nombre);
                        bool result = await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, deleteLabel, AppResource.BtnContinue, AppResource.BtnCancel);
                        if (result)
                        {
                            IsBusy = true;
                            Client = new ApiClient(CurrentUser.CurrentToken);
                            bool updated = await Client.DeleteAsync($"CategoriaUsuario/{SelectedCategory.CategoriaUsuarioId}");
                            if (updated)
                                await LoadCategories();
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
        private async Task EditCategory()
        {
            if (SelectedCategory != null)
            {

                string newCategoryLabel = AppResource.LblNewEditCategory;
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (currentCulture == "es")
                {
                    newCategoryLabel = newCategoryLabel.Replace("$action$", "Editar");
                }
                else if (currentCulture == "en")
                {
                    newCategoryLabel = newCategoryLabel.Replace("$action$", "Edit");
                }
                string newCategory = await PageDialogService.DisplayPromptAsync(newCategoryLabel, AppResource.LblCategoryName, AppResource.LblSave, AppResource.BtnCancel, initialValue: SelectedCategory.Nombre.Trim(), keyboardType: KeyboardType.Text);
                if (newCategory != null && newCategory.Trim().Equals(""))
                {
                    await ShowToast(AppResource.LblCategoryNameRequired, ToastDuration.Short);
                }
                else if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    IsBusy = true;
                    SelectedCategory.Nombre = newCategory.Trim();
                    Client = new ApiClient(CurrentUser.CurrentToken);
                    await Client.PutAsync($"CategoriaUsuario/{SelectedCategory.CategoriaUsuarioId}", SelectedCategory);
                    await LoadCategories();
                    await ShowToast(AppResource.LblSuccess, ToastDuration.Short);
                }
            }
        }

        public void PerformSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Categories = BackupCategories;
            }
            else
            {
                Categories = new ObservableCollection<DtoCategoriaUsuario>(BackupCategories.Where(c => c.Nombre.Contains(query)));
            }
        }

        public async void PerformActionToCategory()
        {
            try
            {
                if (!IsBusy)
                {
                    if (SelectedCategory != null)
                    {
                        string result = await PageDialogService.DisplayActionSheetAsync(AppResource.LblChooseAnAction, AppResource.BtnCancel, null, $"{AppResource.LblEdit}", $"{AppResource.LblDelete}");
                        if (result.Equals($"{AppResource.LblEdit}"))
                        {
                            await EditCategory();
                        }
                        else if (result.Equals($"{AppResource.LblDelete}"))
                        {
                            await DeleteCategory();
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
                SelectedCategory = null;
                IsBusy = false;
            }
        }

        private async void NewCategory()
        {
            try
            {
                if (!IsBusy)
                {

                    var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    string newCategoryLabel = AppResource.LblNewEditCategory;
                    if (currentCulture == "es")
                    {
                        newCategoryLabel = newCategoryLabel.Replace("$action$", "Nueva");
                    }
                    else if (currentCulture == "en")
                    {
                        newCategoryLabel = newCategoryLabel.Replace("$action$", "New");
                    }
                    string newCategory = await PageDialogService.DisplayPromptAsync(newCategoryLabel, AppResource.LblCategoryName, AppResource.LblSave, AppResource.BtnCancel, keyboardType: KeyboardType.Text);
                    if (newCategory != null && newCategory.Trim().Equals(""))
                    {
                        await ShowToast(AppResource.LblCategoryNameRequired, ToastDuration.Short);
                    }
                    else if (!string.IsNullOrWhiteSpace(newCategory))
                    {
                        IsBusy = true;
                        DtoCategoriaUsuario category = new DtoCategoriaUsuario
                        {
                            Nombre = newCategory.Trim(),
                            UsuarioId = CurrentUser.UsuarioId,
                            FechaRegistro = ApiClient.GetCurrentDateTime(),
                            FechaModificacion = ApiClient.GetCurrentDateTime()
                        };
                        Client = new ApiClient(CurrentUser.CurrentToken);
                        category = await Client.PostAsync<DtoCategoriaUsuario, DtoCategoriaUsuario>("CategoriaUsuario", category);
                        if (category.CategoriaUsuarioId > 0)
                        {
                            Categories.Add(category);
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
                IsBusy = false;
            }
        }

        public async Task LoadCategories()
        {
            Categories = await Client.GetAsync<ObservableCollection<DtoCategoriaUsuario>>(@$"CategoriaUsuario\ObtenerCategoriasUsuarios\{CurrentUser.UsuarioId}");
            BackupCategories = Categories;
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
                    await LoadCategories();
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
