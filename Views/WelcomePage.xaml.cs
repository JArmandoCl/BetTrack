namespace BetTrack.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        BtnGoToLogin.IsVisible = e.CurrentPosition == 4;
        SkipWelcome.IsVisible = e.CurrentPosition < 4;
    }
}