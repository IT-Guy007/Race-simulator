namespace MAUI;

public partial class CompetitionPage : ContentPage
{
    private RacePage _racePage;
    private Window _window;
    
    public CompetitionPage() {
        InitializeComponent();
        _racePage = new RacePage();
        _window = new Window{Page = _racePage};
    }

    private void RaceStart(object sender, EventArgs e) {
        Application.Current.OpenWindow(_window);
    }
}