using BetTrack.ViewModels;
using BetTrack.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Prism;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

namespace BetTrack
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UsePrism(prism =>
                {
                    prism.RegisterTypes(RegisterTypes)
                    .CreateWindow(async navigationService =>
                    {

                        INavigationResult result = await navigationService.NavigateAsync("//WelcomePage");
                        //if (VersionTracking.Default.IsFirstLaunchEver)
                        //{
                        //    await navigationService.NavigateAsync("NavigationPage/WelcomePage");
                        //}
                        //else
                        //{
                        //    await navigationService.NavigateAsync("LoginPage");
                        //}
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

            return builder.Build();
        }


        private static void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Registrar páginas y ViewModels para la navegación
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<DashboardPage, DashboardPageViewModel>();
            containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<ForgotPasswordPage, ForgotPasswordPageViewModel>();
        }
    }
}
