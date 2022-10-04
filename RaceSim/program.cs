using Controller;

public class Program {
    static void Main(string[] args) {
        Data.Initialize();
        Console.WriteLine("Current track is: " + Data.currentRace.Track.name);

        for (; ; ) {
            Thread.Sleep(100);
        }
    }
}
