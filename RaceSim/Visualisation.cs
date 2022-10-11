using Model;
using Controller;
using Spectre.Console;

namespace RaceSim;

public static class Visualisation
{

    private static int tileSize;
    private static Canvas canvas;
    private static Direction direction;

    public static void Initialize() {
        direction = Data.currentRace.Track.startDirection;
        tileSize = Straight.Length;
    }

    public static void DrawTrack(Track track) {    
        //Start position
        int x = 3;
        int y = 3;
        
        //Canvas
        int xSize;
        int ySize;
        int xMax = x;
        int yMax = y;
        int xMin = x;
        int yMin = y;
        
        foreach (var varSection in Data.currentRace.Track.Sections)
        {
            setDirection(varSection);

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
        direction = Data.currentRace.Track.startDirection;
        x = 3 - xMin;
        y = 3 - yMin;
        
        //Background
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                canvas.SetPixel(i, j, Color.Green);
            }
        }
        
        //Create track
        // draw all tiles
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
        
        canvas.Scale = false;
        // write screen
        AnsiConsole.Clear();
        Table screen = new();
        screen.RoundedBorder();
        screen.AddColumn($"Track: {Data.currentRace.Track.name} | Laps: {Data.currentRace.Track.laps}");
        screen.AddRow(canvas);
        AnsiConsole.Write(screen);
    }

    public static void DrawSection(Section section, int x, int y)
    {

        string[] tile = new string[tileSize];

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

        //tile = addDrivers(tile,Data.currentRace.Positions[section].Left,Data.currentRace.Positions[section].Right);

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

                    //Driver
                    case '|':
                        color = Color.Blue;
                        break;
                    //Finish
                    case 'f':
                        color = Color.Red;
                        break;
                    //Finish
                    case 'g':
                        color = Color.White;
                        break;

                }

                //Set pixel on position
                canvas.SetPixel(piX, pixY, color);
            }
        }
    }
    
    private static string[] addDrivers(string[] tiles, IParticipant pos1, IParticipant pos2)
    {
        string[] tile = new string[tiles.Length];

        tiles.CopyTo(tile, 0);

        for (int i = 0; i < tile.Length; i++)
        {
            string line = tile[i];

            if (pos1 != null)
            {
                if (!pos1.Equipment.isBroken)
                {
                    line = line.Replace('1', 'z');
                }
                else
                {
                    line = line.Replace('1', '#');
                }
            }

            if (pos2 != null)
            {
                if (!pos2.Equipment.isBroken)
                {
                    line = line.Replace('2', 'z');
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

    private static void setDirection(Section section)
    {
        switch (section.sectionType) {
            
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

    #region graphics
        // Create the trackTypes
        
        private static string[] Straight =
        {
            "║║║║║║║",
            "       ", 
            "       ", 
            "       ", 
            "       ",
            "       ",
            "║║║║║║║",

        };
        
        private static string[] StraightUp =
        {
            "║     ║",
            "║     ║", 
            "║     ║", 
            "║     ║", 
            "║     ║",
            "║     ║",
            "║     ║",

        };
        
        private static string[] Finish =
        {
            "║║║║║║║",
            "     fg", 
            "     gf", 
            "     fg", 
            "     gf",
            "     fg",
            "║║║║║║║",

        };
        
        private static string[] FinishUp =
        {
            "║     ║", 
            "║     ║",
            "║     ║",
            "║     ║",
            "║     ║",
            "║fgfgf║", 
            "║gfgfg║"

        };
        private static string[] Start =
        {
            "║║║║║║║",
            "       ", 
            "| | | |", 
            "       ", 
            "| | | |",
            "       ",
            "║║║║║║║",

        };
        
        private static string[] StartUp =
        {
            "║ | | ║",
            "║     ║", 
            "║ | | ║", 
            "║     ║", 
            "║ | | ║",
            "║     ║",
            "║ | | ║",

        };

        private static string[] CornerWestToNorth =
        {
            "║     ║",
            "      ═", 
            "      ║", 
            "      ═", 
            "      ║",
            "      ═",
            "║═║═║═║",

        };
        
        private static string[] CornerWestToSouth =
        {
            "║═║═║═║",
            "      ═", 
            "      ║", 
            "      ═", 
            "      ║",
            "      ═",
            "║     ║",

        };
        
        private static string[] CornerEastToSouth =         
        {
            "║═║═║═║",
            "═      ", 
            "║      ", 
            "═      ", 
            "║      ",
            "═      ",
            "║     ║",

        };
        
        private static string[] CornerEastToNorth =         
        {
            "║     ║",
            "═      ", 
            "║      ", 
            "═      ", 
            "║      ",
            "═      ",
            "║═║═║═║",

        };
        #endregion

}