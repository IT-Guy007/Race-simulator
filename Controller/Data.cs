using System.Drawing;
using Model;

namespace Controller;

public static class Data {

    public static Competition currentCompetition { get; set; }
    public static Race currentRace { get; set; }
    private static Direction direction;

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

        currentCompetition.Tracks.Enqueue(new Track("Zandvoort", createSections(zandvoortSections),Direction.North,3, Spectre.Console.Color.Orange3, Microsoft.Maui.Graphics.Color.FromArgb("#FFA500")));
        currentCompetition.Tracks.Enqueue(new Track("Spa", createSections(spaSections), Direction.South, 2, Spectre.Console.Color.DarkGreen, Microsoft.Maui.Graphics.Color.FromArgb("#023020")));
        currentCompetition.Tracks.Enqueue(new Track("Square", createSections(squareSections),Direction.East,1, Spectre.Console.Color.DarkSlateGray1, Microsoft.Maui.Graphics.Color.FromArgb("#2F4F4F")));
    }

    private static LinkedList<Section> createSections(SectionTypes[] trackSections) {
        LinkedList<Section> sectionList = new LinkedList<Section>();

        for (int i = 0; i != trackSections.Length; i++)
        {
            sectionList.AddLast(new Section(trackSections[i]));
        }

        return sectionList;

    }

    public static int[] calculateCanvas() {
        //Start position
        int x = 5;
        int y = 5;
        
        //Canvas
        int xSize = 0;
        int ySize = 0;
        int xMax = x;
        int yMax = y;
        int xMin = x;
        int yMin = y;
        //Calculate canvas size
        foreach (var varSection in currentRace.Track.Sections) {
            SetDirection(varSection);

            switch (direction) {
                case Direction.North:
                    y--;
                    break;
                case Direction.East:
                    x++;
                    break;
                case Direction.South:
                    y++;
                    break;
                case Direction.West:
                    x--;
                    break;
            }

            if (x > xMax) {
                xMax = x;
            }

            if (y > yMax) {
                yMax = y;
            }

            if (x < xMin) {
                xMin = x;
            }

            if (y < yMin) {
                yMin = y;
            }
        }
        
        xSize = xMax - xMin + 1;
        ySize = yMax - yMin + 1;
        int[] result = {xSize,ySize,xMin,yMin,x,y};
        return result;
    }
    
    //Set new direction for next section
    public static Direction SetDirection(Section section) {
        switch (section.sectionType) {

            case SectionTypes.LeftCorner:
                switch(direction) {
                    case Direction.North:
                        return Direction.West;
                    case Direction.East:
                        return Direction.North;
                    case Direction.South:
                        return Direction.East;
                    case Direction.West:
                        return Direction.South;
                }

                break;
            case SectionTypes.RightCorner:
                switch (direction) {
                    case Direction.North:
                        return Direction.East;
                    case Direction.East:
                        return Direction.South;
                    case Direction.South:
                        return Direction.West;
                    case Direction.West:
                        return Direction.North;
                }
                break;
        }

        return direction;
    }
    
}

