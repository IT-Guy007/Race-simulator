using Controller;
using MAUI.ViewModel;

namespace MAUI;

public partial class RaceStatisticsPage {
    
    private readonly Window RaceSimulatorWindow;

    public RaceStatisticsPage() {
        
        //Initialise XAML
        InitializeComponent();
        
        RaceStatisticsViewModel RaceStatisticsViewModel = new RaceStatisticsViewModel();
        
        //Set binding
        BindingContext = RaceStatisticsViewModel;

        //Add handler
        Data.CurrentRace.RaceEnded += RaceEnded;
        
        //Create new window
        RaceSimulatorWindow = new Window(new RaceSimulator());
        Application.Current?.OpenWindow(RaceSimulatorWindow);

    }
    
    private void RaceEnded(object sender, EventArgs eventArgs) {
        
        //End race
        Console.WriteLine("End of the race");
        
        // Close the active window
        Application.Current.CloseWindow(RaceSimulatorWindow);
        
        Navigation.PushAsync(new CompetitionPage());

    }
}
