namespace MAUI;

public partial class MainPage : ContentPage {

    public MainPage()
    {
        InitializeComponent();
    }

    private void RaceStart(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RacePage());
    }
}