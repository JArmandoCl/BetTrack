using BetTrack.Dtos;

namespace BetTrack.Views;

public partial class NewBetPage : ContentPage
{
    public NewBetPage()
    {
        InitializeComponent();
        BetDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
    }

    private void BetAmount_Focused(object sender, FocusEventArgs e)
    {
        if (BetAmount.Value == 0)
        {
            BetAmount.Value = 0;
        }
    }

    private void Odds_Focused(object sender, FocusEventArgs e)
    {
        if (Odds.Value == 0)
        {
            Odds.Value = 0;
        }
    }

    private void DateTimePicker_OkButtonClicked(object sender, EventArgs e)
    {
        Dispatcher.Dispatch(() => DateTimePicker.IsOpen = false);
    }

    private void OpenBetDatePicker_Clicked(object sender, EventArgs e)
    {
        Dispatcher.Dispatch(() => DateTimePicker.IsOpen = true);
    }

    private void DateTimePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.DateTimePickerSelectionChangedEventArgs e)
    {
        BetDate.Text = e.NewValue?.ToString("dd-MM-yyyy hh:mm tt");
    }

    private void EsApuestaGratuita_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        if (EsApuestaPagada.IsChecked == true && e.IsChecked == true)
        {
            EsApuestaPagada.IsChecked = false;
        }
    }

    private void EsApuestaPagada_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        if (EsApuestaGratuita.IsChecked == true && e.IsChecked == true)
        {
            EsApuestaGratuita.IsChecked = false;
        }
    }

    private void EstatusApuesta_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        DtoEstatusApuesta estatusApuesta = (DtoEstatusApuesta)e.AddedItems.FirstOrDefault();
        if (estatusApuesta != null && estatusApuesta.EstatusApuestaId == 4)
        {
            InputCashout.IsVisible = true;
        }
        else
        {
            InputCashout.IsVisible = false;
            Cashout.Value = 0;
        }
    }

    private void Cashout_Focused(object sender, FocusEventArgs e)
    {
        if (Cashout.Value == 0)
        {
            Cashout.Value = 0;
        }
    }
}