using BetTrack.ViewModels;
using BetTrack.Views;

namespace BetTrack
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXxdcHRVRWdcUEN1WEpWYUA=");
            Application.Current.UserAppTheme = AppTheme.Dark;//Light mode doesn´t show entry bottom line
            InitializeComponent();
        }

    }
}
