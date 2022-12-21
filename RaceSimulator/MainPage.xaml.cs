namespace MAUI;

public partial class MainPage : ContentPage {

    StatisticsPage _statisticsPage;
    private RacePage _racePage;
    public MainPage()
    {
        InitializeComponent();
    }

    private void Clicked(object sender, EventArgs e) {
        Navigation.PushAsync(new CompetitionPage());
    }
}