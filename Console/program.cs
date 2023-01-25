using Controller;
using RaceSim;

public class Program {
    
    static void Main(string[] args) {
        
        //Initialise
        Data.Initialize();
        Data.NextRace();
        Visualisation.Initialize();
        Data.CurrentRace.DriversChanged += Visualisation.DriversChanged;
        Data.CurrentRace.RaceEnded += Visualisation.RaceEnded;
        
        while (Data.CurrentRace.Participants.Count > 0) { for (;;) { } }
    }

}
