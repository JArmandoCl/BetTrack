using BetTrack.Dtos;
using BetTrack.ViewModels;
using System.Diagnostics;

namespace BetTrack.Views;

public partial class HomePage : ContentPage
{
    HomePageViewModel homePageViewModel;
    public HomePage()
    {
        InitializeComponent();
    }

    private void Casinos_SwipeEnded(object sender, Syncfusion.Maui.ListView.SwipeEndedEventArgs e)
    {
        if (e.Offset >= 100)
        {
            homePageViewModel = (HomePageViewModel)BindingContext;
            homePageViewModel.SelectedCasinoChanged((DtoUsuarioBankroll)e.DataItem, true);
            Casinos.ResetSwipeItem();
        }
    }
}