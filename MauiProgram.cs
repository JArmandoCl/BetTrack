using BetTrack.ViewModels;
using BetTrack.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Prism;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
using System.Diagnostics;

namespace BetTrack
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            INavigationResult navigationResult = null;
            var builder = MauiApp.CreateBuilder();
            try
            {
                builder
                       .UseMauiApp<App>()
                       .UseMauiCommunityToolkit()
                       .UsePrism(prism =>
                       {
                           prism.RegisterTypes(RegisterTypes)
                           .CreateWindow(async navigationService =>
                           {
                               bool rememberMeEnabled = false;
                               rememberMeEnabled = Preferences.Default.Get("RememberMeEnabled", false);

                               if (VersionTracking.Default.IsFirstLaunchEver)
                               {
                                   navigationResult = await navigationService.NavigateAsync("NavigationPage/WelcomePage");
                               }
                               else if (!rememberMeEnabled)
                               {
                                   navigationResult = await navigationService.NavigateAsync("LoginPage");
                               }
                               else
                               {
                                   navigationResult = await navigationService.NavigateAsync("//NavigationPage/HomePage");
                               }
                               if (navigationResult?.Success == false)
                               {
                                   Debug.WriteLine(navigationResult.Exception?.GetRootException());
                               }
                           });
                       })
                       .ConfigureSyncfusionCore()
                       .ConfigureSyncfusionToolkit()
                       .ConfigureEssentials(essentials =>
                       {
                           essentials.UseVersionTracking();
                       })
                       .ConfigureFonts(fonts =>
                       {
                           fonts.AddFont("Kabisat Demo-ItalicTall.ttf", "Logo");
                           fonts.AddFont("Lato-Bold.ttf", "Bold");
                           fonts.AddFont("Lato-Italic.ttf", "Italic");
                           fonts.AddFont("Lato-Regular.ttf", "Lato");
                           fonts.AddFont("fontawesome.ttf", "fa");
                       });

#if DEBUG
                builder.Logging.AddDebug();
#endif
            }
            catch (Exception e)
            {

                throw;
            }

            return builder.Build();
        }


        private static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Registrar páginas y ViewModels para la navegación
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BankrollDashboardPage, BankrollDashboardPageViewModel>();
            containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<ForgotPasswordPage, ForgotPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<TipstersListPage, TipstersListPageViewModel>();
            containerRegistry.RegisterForNavigation<UserCasinosPage, UserCasinosPageViewModel>();
            containerRegistry.RegisterForNavigation<BetTrackCasinosPage, BetTrackCasinosPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<NewEditBankrollPage, NewEditBankrollPageViewModel>();
            containerRegistry.RegisterForNavigation<NewBetPage, NewBetPageViewModel>();
        }
    }
}
