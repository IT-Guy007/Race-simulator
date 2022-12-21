using Controller;
using Model;

namespace MAUI;

public partial class MainPage : ContentPage {
    
    public MainPage() {
        InitializeComponent();
    }

    private void Clicked(object sender, EventArgs e) {
        //Initialise the data
        if (Data.currentCompetition == null) {
            Data.Initialize();
        }

        Navigation.PushAsync(new CompetitionPage());
    }
}