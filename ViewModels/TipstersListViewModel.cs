using BetTrack.Api;
using BetTrack.Dtos;
using BetTrack.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.ViewModels
{
    public class TipstersListViewModel : ViewModelBase
    {
        #region Object declarations   
        public DelegateCommand GoToNewTipster { get; set; }
        #endregion
        public TipstersListViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            GoToNewTipster = new DelegateCommand(NewTipster);
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
                    DtoUsuarioTipster tipster = new DtoUsuarioTipster
                    {
                        NombreTipster = newTipsterLabel,
                        UsuarioId = CurrentUser.UsuarioId,
                        FechaRegistro = ApiClient.ObtenerFechaActual()
                    };
                    Client = new ApiClient(CurrentUser.CurrentToken);
                    tipster = await Client.PostAsync<DtoUsuarioTipster, DtoUsuarioTipster>("UsuarioTipster", tipster);
                    if (tipster.UsuarioTipsterId > 0)
                    {

                    }
                    IsBusy = true;
                }
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
    }
}
