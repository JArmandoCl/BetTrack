using BetTrack.Api;
using BetTrack.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible, IPageLifecycleAware
    {
        #region Object declarations
        public ApiClient Client { get; set; }
        protected readonly INavigationService NavigationService;
        protected readonly IPageDialogService PageDialogService;
        private string title = "";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        private DtoUsuario currentUser;
        public DtoUsuario CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }
        #endregion
        protected ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            LoadBasicInfo();
        }

        private async void LoadBasicInfo()
        {
            string currentUserSaved = await SecureStorage.GetAsync("CurrentUser") ?? JsonSerializer.Serialize(new DtoUsuario());
            if (!string.IsNullOrWhiteSpace(currentUserSaved))
            {
                CurrentUser = JsonSerializer.Deserialize<DtoUsuario>(currentUserSaved) ?? new DtoUsuario();
            }
        }

        public virtual void Destroy()
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }
}
