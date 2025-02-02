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
        FrameNextPage.IsVisible = !BtnGoToLogin.IsVisible;
        LabelNextPage.IsVisible = !BtnGoToLogin.IsVisible;
        FramePreviousPage.IsVisible = e.CurrentPosition > 0;
        LabelPreviousPage.IsVisible = e.CurrentPosition > 0;
        SkipWelcome.IsVisible = e.CurrentPosition < 4;
    }

    private void PreviousPage_Tapped(object sender, TappedEventArgs e)
    {
        if (Welcome.Position > 0)
            Welcome.Position -= 1;
    }

    private void NextPage_Tapped(object sender, TappedEventArgs e)
    {
        Welcome.Position += 1;
    }
}