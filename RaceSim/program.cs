using Controller;
using Model;
using RaceSim;

public class Program {
    static void Main(string[] args) {
        
        //Initialise
        Data.Initialize();
        Data.NextRace();
        Visualisation.Initialize();
        Data.currentRace.DriversChanged += Visualisation.DriversChanged;
        Data.currentRace.RaceEnded += RaceEnded;
            
        
        // Loop
        while (Data.currentRace.Participants.Count > 0) { for (;;) { } }
         
    }
    
    //Race ended, start new race if exists
    private static void RaceEnded(object sender, EventArgs eventArgs) {
        Data.NextRace();
        Data.currentRace.DriversChanged += Visualisation.DriversChanged;
        Data.currentRace.RaceEnded += RaceEnded;
        Visualisation.Initialize();
    }
}
