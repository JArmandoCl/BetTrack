using BetTrack.ViewModels;
using BetTrack.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Prism;

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
                        if (VersionTracking.Default.IsFirstLaunchEver)
                        {
                            await navigationService.NavigateAsync("NavigationPage/BienvenidaPage");
                        }
                        else
                        {
                            await navigationService.NavigateAsync("LoginPage");
                        }
                    });
                })  
                .ConfigureEssentials(essentials =>
                {
                    essentials.UseVersionTracking();
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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
            containerRegistry.RegisterForNavigation<BienvenidaPage, BienvenidaPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegistroPage, RegistroPageViewModel>();
        }
    }
}
