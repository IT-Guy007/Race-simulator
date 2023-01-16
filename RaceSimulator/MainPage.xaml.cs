using Controller;
using Model;

namespace MAUI;

public partial class MainPage : ContentPage {
    
    public MainPage() {
        InitializeComponent();
    }

    private void Clicked(object sender, EventArgs e) {
        Console.WriteLine("Starting new competition");
        //Initialise the data
        Data.Initialize();
        Navigation.PushAsync(new CompetitionPage());
    }
}