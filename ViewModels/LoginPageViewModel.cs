﻿using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Models;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
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
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ShowForgotPassword { get; set; }
        public DelegateCommand ShowSignUpCommand { get; set; }
        public DtoUsuario User { get; set; } = new DtoUsuario();
        private bool rememberMe;
        public bool RememberMe
        {
            get { return rememberMe; }
            set { SetProperty(ref rememberMe, value); }
        }
        #endregion
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Title = "Login";
            LoginCommand = new DelegateCommand(Login);
            ShowForgotPassword = new DelegateCommand(async () => await NavigationService.NavigateAsync("ForgotPasswordPage"));
            ShowSignUpCommand = new DelegateCommand(ShowSignUp);
        }
        private async void ShowSignUp()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    await NavigationService.NavigateAsync("SignUpPage");
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

        private async void Login()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    Errors = Utilities.ValidateModel(User);
                    if (Errors.ContainsKey("Nickname"))
                        Errors.Remove("Nickname");
                    if (Errors.Any())
                    {
                        RaisePropertyChanged(nameof(Errors));
                    }
                    else
                    {

                        Client = new ApiClient(await SecureStorage.GetAsync("UserToken") ?? "");
                        DtoUsuario currentUser = await Client.PostAsync<DtoUsuario, DtoUsuario>($"Autorizacion", User);
                        if (!string.IsNullOrWhiteSpace(currentUser.CurrentToken))
                        {
                            CurrentUser = currentUser;
                            await SecureStorage.SetAsync("CurrentUser", JsonSerializer.Serialize(currentUser));
                            Preferences.Default.Set("RememberMeEnabled", RememberMe);

                            INavigationResult result = await NavigationService.NavigateAsync("/NavigationPage/HomePage");
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Errors["LoginValidation"] = AppResource.LblLoginMessageValidation;
                RaisePropertyChanged(nameof(Errors));
            }
            catch (Exception e)
            {
                await PageDialogService.DisplayAlertAsync(AppResource.LblDialogTitle, AppResource.LblBadRequestServer, AppResource.BtnClose);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
