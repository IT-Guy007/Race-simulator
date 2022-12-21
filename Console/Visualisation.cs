using Model;
using Controller;
using Spectre.Console;

namespace RaceSim;

public static class Visualisation
{

    private static int tileSize;
    private static Table table;
    private static Canvas canvas;
    private static Direction direction;

    public static void Initialize()
    {
        direction = Data.currentRace.Track.startDirection;
        tileSize = Straight.Length; //Which is always 7
        table = new Table();
    }

    //Events
    public static void DriversChanged(object sender, DriversChanged e)
    {
        DrawTrack();
    }

    public static void DrawTrack()
    {
        //Start position
        int x = 5;
        int y = 5;

        //Canvas
        int xSize;
        int ySize;
        int xMax = x;
        int yMax = y;
        int xMin = x;
        int yMin = y;

        //Calculate canvas size
        foreach (var varSection in Data.currentRace.Track.Sections)
        {
            setDirection(varSection);

            switch (direction)
            {
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

            if (x > xMax)
            {
                xMax = x;
            }

            if (y > yMax)
            {
                yMax = y;
            }

            if (x < xMin)
            {
                xMin = x;
            }

            if (y < yMin)
            {
                yMin = y;
            }
        }

        xSize = xMax - xMin + 1;
        ySize = yMax - yMin + 1;

        //Create canvas
        canvas = new Canvas(xSize * tileSize, ySize * tileSize);
        canvas.Scale = false;
        direction = Data.currentRace.Track.startDirection;
        x = 5 - xMin;
        y = 5 - yMin;

        //Background
        for (int i = 0; i < xSize * tileSize; i++)
        {
            for (int j = 0; j < ySize * tileSize; j++)
            {
                canvas.SetPixel(i, j, Data.currentRace.Track.backgroundColor);
            }
        }

        //Draw tiles
        foreach (Section section in Data.currentRace.Track.Sections)
        {
            DrawSection(section, x, y);

            switch (direction)
            {
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
        }
        /*
        //Write screen
        //TABLES DON'T WORK, MAYBLE LATER
        AnsiConsole.Clear();
        table.RoundedBorder();
        table.Alignment(Justify.Left);
        
        //Create row data
        string positions = "1\n2\n3\n4\n5\n6\n7\n8";
        string participants = "";
        foreach (var driver in Data.currentRace.Participants) {
           participants = participants + (driver.DriverNumber + " " + driver.Name + "\n");
        }
        
        //Create column
        table.Title("Track: " + Data.currentRace.Track.name);
        table.AddColumn("Laps: " + Data.currentRace.Track.laps).Centered();
        table.AddColumn(new TableColumn("Position").Centered());
        table.AddColumn(new TableColumn("Racers").Alignment(Justify.Left));

        // Create row
        table.AddRow(canvas);
        table.AddRow(canvas,positions, participants);

        //Print to console
        AnsiConsole.Write(canvas);
        */
    }

    public static void DrawSection(Section section, int x, int y)
    {

        string[] tile = new string[tileSize];

        //Get graphic for section
        switch (section.sectionType)
        {
            case SectionTypes.Straight:
                if (direction == Direction.North)
                {
                    tile = StraightUp;
                }
                else if (direction == Direction.West)
                {
                    tile = Straight;
                }
                else if (direction == Direction.South)
                {
                    tile = StraightUp;
                }
                else if (direction == Direction.East)
                {
                    tile = Straight;
                }

                break;
            case SectionTypes.LeftCorner:
                if (direction == Direction.North)
                {
                    tile = CornerWestToSouth;
                    direction = Direction.West;
                }
                else if (direction == Direction.West)
                {
                    tile = CornerEastToSouth;
                    direction = Direction.South;
                }
                else if (direction == Direction.South)
                {
                    tile = CornerEastToNorth;
                    direction = Direction.East;
                }
                else if (direction == Direction.East)
                {
                    tile = CornerWestToNorth;
                    direction = Direction.North;
                }

                break;
            case SectionTypes.RightCorner:
                if (direction == Direction.North)
                {
                    tile = CornerEastToSouth;
                    direction = Direction.East;
                }
                else if (direction == Direction.West)
                {
                    tile = CornerEastToNorth;
                    direction = Direction.North;
                }
                else if (direction == Direction.South)
                {
                    tile = CornerWestToNorth;
                    direction = Direction.West;
                }
                else if (direction == Direction.East)
                {
                    tile = CornerWestToSouth;
                    direction = Direction.South;
                }

                break;
            case SectionTypes.StartGrid:
                if (direction == Direction.North)
                {
                    tile = StartUp;
                }
                else if (direction == Direction.West)
                {
                    tile = Start;
                }
                else if (direction == Direction.South)
                {
                    tile = StartUp;
                }
                else if (direction == Direction.East)
                {
                    tile = Start;
                }

                break;
            case SectionTypes.Finish:
                if (direction == Direction.North)
                {
                    tile = FinishUp;
                }
                else if (direction == Direction.West)
                {
                    tile = Finish;
                }
                else if (direction == Direction.South)
                {
                    tile = FinishUp;
                }
                else if (direction == Direction.East)
                {
                    tile = Finish;
                }

                break;
        }

        //Add drivers to it
        tile = addDrivers(tile, Data.currentRace.Positions[section].Left,
            Data.currentRace.Positions[section].Right);

        //Draw the tile
        for (int i = 0; i < tile.Length; i++)
        {
            for (int j = 0; j < tile[i].Length; j++)
            {
                int piX = x * tileSize + j;
                int pixY = y * tileSize + i;

                char singleTile = tile[i][j];
                Color color = Color.Black;

                //Set color for tile
                switch (singleTile)
                {
                    //Track items

                    //Side of the track
                    case '║':
                        color = Color.Red;
                        break;
                    //Side of the track
                    case '═':
                        color = Color.Orange1;
                        break;

                    //Track
                    case ' ':
                        color = Color.White;
                        break;

                    //Positions
                    case '1':
                        color = Color.White;
                        break;
                    case '2':
                        color = Color.White;
                        break;
                    case 'A':
                        color = Color.White;
                        break;
                    case 'B':
                        color = Color.White;
                        break;
                    case 'C':
                        color = Color.White;
                        break;
                    case 'D':
                        color = Color.White;
                        break;
                    case 'E':
                        color = Color.White;
                        break;
                    case 'F':
                        color = Color.White;
                        break;
                    case 'G':
                        color = Color.White;
                        break;
                    case 'H':
                        color = Color.White;
                        break;

                    //Driver
                    case 'r':
                        color = Color.Red;
                        break;
                    case 'b':
                        color = Color.Blue;
                        break;
                    case 'g':
                        color = Color.Green;
                        break;
                    case 'y':
                        color = Color.Yellow;
                        break;
                    case 'o':
                        color = Color.Orange1;
                        break;
                    case 'h':
                        color = Color.Grey;
                        break;
                    //Finish
                    case 'f':
                        color = Color.Red;
                        break;
                    //Finish
                    case '-':
                        color = Color.Wheat1;
                        break;

                    //Broken car
                    case '#':
                        color = Color.SandyBrown;
                        break;

                }

                //Set pixel on position
                canvas.SetPixel(piX, pixY, color);
            }
        }
    }

    //Set new direction for next section
    private static void setDirection(Section section)
    {
        switch (section.sectionType)
        {

            case SectionTypes.LeftCorner:
                if (direction == Direction.North)
                {
                    direction = Direction.West;
                }
                else if (direction == Direction.East)
                {
                    direction = Direction.North;
                }
                else if (direction == Direction.South)
                {
                    direction = Direction.East;
                }
                else if (direction == Direction.West)
                {
                    direction = Direction.South;
                }

                break;
            case SectionTypes.RightCorner:
                if (direction == Direction.North)
                {
                    direction = Direction.East;
                }
                else if (direction == Direction.East)
                {
                    direction = Direction.South;
                }
                else if (direction == Direction.South)
                {
                    direction = Direction.West;
                }
                else if (direction == Direction.West)
                {
                    direction = Direction.North;
                }

                break;
        }
    }

    //Add drivers to section
    private static string[] addDrivers(string[] givenSize, IParticipant driverLeft, IParticipant driverRight)
    {

        string[] tile = new string[givenSize.Length];

        givenSize.CopyTo(tile, 0);

        for (int i = 0; i < tile.Length; i++)
        {
            string line = tile[i];

            if (driverLeft != null)
            {
                if (!driverLeft.Equipment.IsBroken)
                {
                    line = line.Replace('1', TeamColorToChar(driverLeft.TeamColor));
                    line = line.Replace('3', TeamColorToChar(driverLeft.TeamColor));
                    line = line.Replace('5', TeamColorToChar(driverLeft.TeamColor));
                    line = line.Replace('7', TeamColorToChar(driverLeft.TeamColor));
                }
                else
                {
                    line = line.Replace('1', '#');
                }
            }

            if (driverRight != null)
            {
                if (!driverRight.Equipment.IsBroken)
                {
                    line = line.Replace('2', TeamColorToChar(driverRight.TeamColor));
                    line = line.Replace('4', TeamColorToChar(driverRight.TeamColor));
                    line = line.Replace('6', TeamColorToChar(driverRight.TeamColor));
                    line = line.Replace('8', TeamColorToChar(driverRight.TeamColor));
                }
                else
                {
                    line = line.Replace('2', '#');
                }
            }

            tile[i] = line;
        }

        return tile;
    }

    #region graphics

    // Create the trackTypes

    private static string[] Straight =
    {
        "║║║║║║║",
        "       ",
        "   2   ",
        "       ",
        "   1   ",
        "       ",
        "║║║║║║║",

    };

    private static string[] StraightUp =
    {
        "║     ║",
        "║     ║",
        "║     ║",
        "║ 1 2 ║",
        "║     ║",
        "║     ║",
        "║     ║",

    };

    private static string[] Finish =
    {
        "║║║║║║║",
        "     f-",
        "   1 -f",
        "     f-",
        "   2 -f",
        "     f-",
        "║║║║║║║",

    };

    private static string[] FinishUp =
    {
        "║     ║",
        "║     ║",
        "║     ║",
        "║ 1 2 ║",
        "║     ║",
        "║f-f-f║",
        "║-f-f-║"

    };

    private static string[] Start =
    {
        "║║║║║║║",
        "       ",
        "B D F H",
        "       ",
        "A C E G",
        "       ",
        "║║║║║║║",

    };

    private static string[] StartUp =
    {
        "║ A B ║",
        "║     ║",
        "║ C D ║",
        "║     ║",
        "║ E F ║",
        "║     ║",
        "║ G H ║",

    };

    private static string[] CornerWestToNorth =
    {
        "║     ║",
        "      ═",
        "  1   ║",
        "      ═",
        "   2  ║",
        "      ═",
        "║═║═║═║",

    };

    private static string[] CornerWestToSouth =
    {
        "║═║═║═║",
        "      ═",
        "   2  ║",
        "      ═",
        "  1   ║",
        "      ═",
        "║     ║",

    };

    private static string[] CornerEastToSouth =
    {
        "║═║═║═║",
        "═      ",
        "║  2   ",
        "═      ",
        "║   1  ",
        "═      ",
        "║     ║",

    };

    private static string[] CornerEastToNorth =
    {
        "║     ║",
        "═      ",
        "║   1  ",
        "═      ",
        "║  2   ",
        "═      ",
        "║═║═║═║",

    };

    #endregion

    private static char TeamColorToChar(TeamColors teamColor)
    {
        switch (teamColor)
        {
            case TeamColors.Red:
                return 'r';
            case TeamColors.Blue:
                return 'b';
            case TeamColors.Green:
                return 'g';
            case TeamColors.Yellow:
                return 'y';
            case TeamColors.Orange:
                return 'o';
            case TeamColors.Grey:
                return 'h';
            default:
                return ' ';
        }
    }

    //Race ended, start new race if exists
    public static void RaceEnded(object sender, EventArgs eventArgs)
    {
        Data.NextRace();
        Data.currentRace.DriversChanged += Visualisation.DriversChanged;
        Data.currentRace.RaceEnded += RaceEnded;
        Visualisation.Initialize();
    }
}