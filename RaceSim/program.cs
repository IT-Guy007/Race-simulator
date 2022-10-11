using Controller;
using RaceSim;

public class Program {
    static void Main(string[] args) {
        
        //Initialise
        Data.Initialize();
        Visualisation.Initialize();
        
        //Run
        Visualisation.DrawTrack(Data.currentRace.Track);
        
        //Keeping it running
        for (; ; ) {
            Thread.Sleep(100);
        }
    }
}
