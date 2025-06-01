using BetTrack.ViewModels;

namespace BetTrack.Views;

public partial class BankrollDashboardPage : ContentPage
{
    BankrollDashboardPageViewModel binding;
    public BankrollDashboardPage()
    {
        InitializeComponent();
    }

    private void tabView_SelectionChanging(object sender, Syncfusion.Maui.Toolkit.TabView.SelectionChangingEventArgs e)
    {
        if (binding == null)
        {
            binding = BindingContext as BankrollDashboardPageViewModel;
        }
        binding.TabSelectedCommand.Execute(e.Index);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}