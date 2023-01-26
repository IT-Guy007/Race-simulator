using System.Drawing;
using Model;

namespace Controller;

public static class Data {

    public static Competition CurrentCompetition { get; set; }
    public static Race CurrentRace { get; set; }
    private static Direction direction;

    public static void Initialize() {
        CurrentCompetition = new Competition();
        AddDrivers();
        AddTracks();
    }

    private static void AddDrivers() {
        CurrentCompetition.Participants = new List<IParticipant>();
        AddDriver("Max Verstappen", TeamColors.Yellow, 01, new Car(80, 80, 80, false));
        AddDriver("Sergio Perez", TeamColors.Yellow, 33, new Car(80, 80, 80, false));
        AddDriver("Charles Leclerc", TeamColors.Red, 16, new Car(80, 80, 80, false));
        AddDriver("Carlos Sainz", TeamColors.Red, 55, new Car(80, 80, 80, false));
        AddDriver("Lewis Hamilton", TeamColors.Grey, 44, new Car(80, 80, 80, false));
        AddDriver("George Russell", TeamColors.Grey, 63, new Car(80, 80, 80, false));
        AddDriver("Lando Norris", TeamColors.Orange, 04, new Car(80, 80, 80, false));
        AddDriver("Daniel Riccardo", TeamColors.Orange, 03, new Car(80, 80, 80, false));
    }

    public static void NextRace() {
        if (CurrentRace == null) {
            if (CurrentCompetition.Tracks.Count == 0) {
                Console.WriteLine("No more races");
            } else {
                CurrentRace?.Stop();
                CurrentRace = new Race(CurrentCompetition.Tracks.Dequeue(), CurrentCompetition.Participants);
                CurrentRace.PlaceDriversOnTrack();
                CurrentRace.RandomEquipment();
                Console.WriteLine("The next race is on " + CurrentRace.Track.name);
                CurrentRace.Start();
            }
        }
    }

    private static void AddDriver(string name, TeamColors color, int driverNumber, IEquipment equipment) {
        CurrentCompetition.Participants.Add(new Driver(name, color, driverNumber, equipment));
    }

    private static void AddTracks() {
        SectionTypes[] zandvoortSections = {
            SectionTypes.Finish,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.LeftCorner,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.LeftCorner,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.StartGrid
        };
        SectionTypes[] squareSections = {
            SectionTypes.Finish,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.StartGrid,
        };
        SectionTypes[] spaSections = {
            SectionTypes.Finish,
            SectionTypes.RightCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.LeftCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.LeftCorner,
            SectionTypes.StartGrid
            
        };
        CurrentCompetition.Tracks.Enqueue(new Track("Square", CreateSections(squareSections),Direction.East,1, Spectre.Console.Color.DarkSlateGray1, Microsoft.Maui.Graphics.Color.FromArgb("#2F4F4F"),6,0));
        CurrentCompetition.Tracks.Enqueue(new Track("Zandvoort", CreateSections(zandvoortSections),Direction.North,1, Spectre.Console.Color.Orange3, Microsoft.Maui.Graphics.Color.FromArgb("#FFA500"),0,1));
        CurrentCompetition.Tracks.Enqueue(new Track("Spa", CreateSections(spaSections), Direction.South, 1, Spectre.Console.Color.DarkGreen, Microsoft.Maui.Graphics.Color.FromArgb("#023020"),0,0));

    }

    private static LinkedList<Section> CreateSections(SectionTypes[] trackSections) {
        LinkedList<Section> sectionList = new LinkedList<Section>();

        for (int i = 0; i != trackSections.Length; i++)
        {
            sectionList.AddLast(new Section(trackSections[i]));
        }

        return sectionList;

    }

}

