using Controller;
using RaceSim;

public class Program {
    
    static void Main(string[] args) {
        
        //Initialise
        Data.Initialize();
        Data.NextRace();
        Visualisation.Initialize();
        Data.currentRace.DriversChanged += Visualisation.DriversChanged;
        Data.currentRace.RaceEnded += Visualisation.RaceEnded;
        
        while (Data.currentRace.Participants.Count > 0) { for (;;) { } }
    }

}
