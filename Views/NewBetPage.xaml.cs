namespace BetTrack.Views;

public partial class NewBetPage : ContentPage
{
	public NewBetPage()
	{
		InitializeComponent();
	}

    private void BetAmount_Focused(object sender, FocusEventArgs e)
    {
		BetAmount.Text = "";
    }
}