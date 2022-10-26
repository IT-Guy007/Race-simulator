using System.Drawing;
using Model;

namespace Controller;

public static class Data
{

    public static Competition currentCompetition { get; set; }
    public static Race currentRace { get; set; }

    public static void Initialize() {
        currentCompetition = new Competition();
        addDrivers();
        addTracks();
    }

    public static void addDrivers() {
        currentCompetition.Participants = new List<IParticipant>();
        addDriver("Max Verstappen", TeamColors.Yellow, 01, new Car(80, 80, 80, false));
        addDriver("Sergio Perez", TeamColors.Yellow, 33, new Car(80, 80, 80, false));
        addDriver("Charles Lecrerc", TeamColors.Red, 16, new Car(80, 80, 80, false));
        addDriver("Carlos Sainz", TeamColors.Red, 55, new Car(80, 80, 80, false));
        addDriver("Lewis Hamilton", TeamColors.Grey, 44, new Car(80, 80, 80, false));
        addDriver("George Russell", TeamColors.Grey, 63, new Car(80, 80, 80, false));
        addDriver("Lando Norris", TeamColors.Orange, 04, new Car(80, 80, 80, false));
        addDriver("Daniel Ricciardo", TeamColors.Orange, 03, new Car(80, 80, 80, false));
    }

    public static void NextRace()
    {
        if (currentRace == null)
        {
            if (currentCompetition.Tracks.Count == 0)
            {
                Console.WriteLine("No more races");
            }
            else
            {
                currentRace?.Stop();
                currentRace = new Race(currentCompetition.Tracks.Dequeue(), currentCompetition.Participants);
                currentRace.PlaceDriversInQue();
                currentRace.RandomEquipment();
                currentRace.Start();
            }
        }
    }

    public static void addDriver(string name, TeamColors color, int driverNumber, IEquipment equipment)
    {
        currentCompetition.Participants.Add(new Driver(name, color, driverNumber, equipment));
    }

    public static void addTracks()
    {
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
            SectionTypes.Straight,
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

        currentCompetition.Tracks.Enqueue(new Track("Zandvoort", createSections(zandvoortSections),Direction.North,3, Spectre.Console.Color.Orange3));
        currentCompetition.Tracks.Enqueue(new Track("Spa", createSections(spaSections), Direction.South, 2, Spectre.Console.Color.DarkGreen));
        currentCompetition.Tracks.Enqueue(new Track("Square", createSections(squareSections),Direction.East,1, Spectre.Console.Color.DarkSlateGray1));
    }

    public static LinkedList<Section> createSections(SectionTypes[] trackSections) {
        LinkedList<Section> sectionList = new LinkedList<Section>();

        for (int i = 0; i != trackSections.Length; i++)
        {
            sectionList.AddLast(new Section(trackSections[i]));
        }

        return sectionList;

    }
}

