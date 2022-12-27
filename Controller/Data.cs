using System.Drawing;
using Model;

namespace Controller;

public static class Data {

    public static Competition currentCompetition { get; set; }
    public static Race currentRace { get; set; }
    private static Direction direction;

    public static void Initialize() {
        currentCompetition = new Competition();
        AddDrivers();
        AddTracks();
    }

    private static void AddDrivers() {
        currentCompetition.Participants = new List<IParticipant>();
        AddDriver("Max Verstappen", TeamColors.Yellow, 01, new Car(80, 80, 80, false));
        AddDriver("Sergio Perez", TeamColors.Yellow, 33, new Car(80, 80, 80, false));
        AddDriver("Charles Lecrerc", TeamColors.Red, 16, new Car(80, 80, 80, false));
        AddDriver("Carlos Sainz", TeamColors.Red, 55, new Car(80, 80, 80, false));
        AddDriver("Lewis Hamilton", TeamColors.Grey, 44, new Car(80, 80, 80, false));
        AddDriver("George Russell", TeamColors.Grey, 63, new Car(80, 80, 80, false));
        AddDriver("Lando Norris", TeamColors.Orange, 04, new Car(80, 80, 80, false));
        AddDriver("Daniel Ricciardo", TeamColors.Orange, 03, new Car(80, 80, 80, false));
    }

    public static void NextRace() {
        if (currentRace == null) {
            if (currentCompetition.Tracks.Count == 0) {
                Console.WriteLine("No more races");
            } else {
                currentRace?.Stop();
                currentRace = new Race(currentCompetition.Tracks.Dequeue(), currentCompetition.Participants);
                currentRace.PlaceDriversInQue();
                currentRace.RandomEquipment();
                currentRace.Start();
            }
        }
    }

    private static void AddDriver(string name, TeamColors color, int driverNumber, IEquipment equipment) {
        currentCompetition.Participants.Add(new Driver(name, color, driverNumber, equipment));
    }

    private static void AddTracks() {
        SectionTypes[] zandvoortSections = {
            SectionTypes.StartGrid,
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
            SectionTypes.Straight
        };
        SectionTypes[] squareSections = {
            SectionTypes.StartGrid,
            SectionTypes.Finish,
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
            SectionTypes.RightCorner,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight,
            SectionTypes.Straight
        };
        SectionTypes[] spaSections = {
            SectionTypes.StartGrid,
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
            SectionTypes.LeftCorner
            
        };

        currentCompetition.Tracks.Enqueue(new Track("Zandvoort", CreateSections(zandvoortSections),Direction.North,3, Spectre.Console.Color.Orange3, Microsoft.Maui.Graphics.Color.FromArgb("#FFA500"),0,2));
        currentCompetition.Tracks.Enqueue(new Track("Spa", CreateSections(spaSections), Direction.South, 2, Spectre.Console.Color.DarkGreen, Microsoft.Maui.Graphics.Color.FromArgb("#023020"),0,0));
        currentCompetition.Tracks.Enqueue(new Track("Square", CreateSections(squareSections),Direction.East,1, Spectre.Console.Color.DarkSlateGray1, Microsoft.Maui.Graphics.Color.FromArgb("#2F4F4F"),0,0));
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

