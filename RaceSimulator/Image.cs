using System.Drawing.Imaging;
using Controller;
using Model;

namespace MAUI;
using System.Drawing;
#pragma warning disable CA1416
public static class Images {
    private static Dictionary<string, Bitmap> _images;
    private static int xSize = 0;
    private static int ySize = 0;
    private static Direction direction = Data.currentRace.Track.startDirection;

    public static Bitmap GetImage(string name) {
        if (_images == null) { _images = new Dictionary<string, Bitmap>();}


        if (!_images.ContainsKey(name)) {
            _images[name] = new Bitmap(name);
        }

        return _images[name].Clone() as Bitmap;
    }

    private static void Clear() { _images = new Dictionary<string, Bitmap>();}

    private static Bitmap emptyImage(int x, int y) {
        Bitmap bmp = new Bitmap(x, y);
        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                bmp.SetPixel(i, j, Color.Black);
            }
        }
        return bmp;
    }

    private static void startImage(Track track) {
        int x = 5;
        int y = 5;
        
        int xSize;
        int ySize;
        int xMax = x;
        int yMax = y;
        int xMin = x;
        int yMin = y;
        
        foreach (var varSection in track.Sections) {
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
    }
    
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
}