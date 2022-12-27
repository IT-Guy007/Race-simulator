using Controller;
using Model;

namespace MAUI;
public static class Images {

    public static Direction Direction;
    
    //Selected section
    public static int X = 5;
    public static int Y = 5;
    
    //Canvas
    public static int XSize;
    public static int YSize;
    private static int _xMax = X;
    private static int _yMax = Y;
    private static int _xMin = X;
    private static int _yMin = Y;

    //Cars
    private const string CarBlue = "car_blue.png";
    private const string CarGreen = "car_green.png";
    private const string CarOrange = "car_orange.png";
    private const string CarRed = "car_red.png";
    private const string CarYellow = "car_yellow.png";
    private const string CarDarkblue = "car_darkblue.png";
    
    //Corners
    private const string CornerEastToNorth = "cornereasttonorth.jpeg";
    private const string CornerEastToSouth = "cornereasttosouth.jpeg";
    private const string CornerWestToNorth = "cornerwesttonorth.jpeg";
    private const string CornerWestToSouth = "cornerwesttosouth.jpeg";
    
    //Finishes
    private const string Finish= "finish.png";
    private const string FinishUp = "finish_up.png";

    //Start
    private const string Start = "start.png";
    private const string StartUp = "start_up.png";
    
    //Straight
    private const string Straight = "straight.jpeg";
    private const string StraightUp = "straight_up.jpeg";

    public static void Initialize() {
        Direction = Data.currentRace.Track.startDirection;
        CalculateCanvas();
    }
    
    public static ImageSource GetImageSource(Section section) {
        switch (section.sectionType) {
            case SectionTypes.Straight:
                switch(Direction) {
                    case Direction.North:
                        return ImageSource.FromFile(StraightUp);
                    case Direction.South:
                        return ImageSource.FromFile(StraightUp);
                    case Direction.East:
                        return ImageSource.FromFile(Straight);
                    case Direction.West:
                        return ImageSource.FromFile(Straight);
                }
                break;

            case SectionTypes.LeftCorner:
                switch(Direction) {
                    case Direction.North:
                        return ImageSource.FromFile(CornerWestToSouth);
                    case Direction.South:
                        return ImageSource.FromFile(CornerEastToNorth);
                    case Direction.East:
                        return ImageSource.FromFile(CornerWestToNorth);
                    case Direction.West:
                        return ImageSource.FromFile(CornerEastToSouth);
                }
                break;
            
            case SectionTypes.RightCorner:
                switch(Direction) {
                    case Direction.North:
                        return ImageSource.FromFile(CornerEastToSouth);
                    case Direction.South:
                        return ImageSource.FromFile(CornerWestToNorth);
                    case Direction.East:
                        return ImageSource.FromFile(CornerWestToSouth);
                    case Direction.West:
                        return ImageSource.FromFile(CornerEastToNorth);
                }
                break;
            case SectionTypes.StartGrid:
                switch(Direction) {
                    case Direction.North:
                        return ImageSource.FromFile(StartUp);
                    case Direction.South:
                        return ImageSource.FromFile(StartUp);
                    case Direction.East:
                        return ImageSource.FromFile(Start);
                    case Direction.West:
                        return ImageSource.FromFile(Start);
                }
                break;

            case SectionTypes.Finish:
                switch(Direction) {
                    case Direction.North:
                        return ImageSource.FromFile(FinishUp);
                    case Direction.South:
                        return ImageSource.FromFile(FinishUp);
                    case Direction.East:
                        return ImageSource.FromFile(Finish);
                    case Direction.West:
                        return ImageSource.FromFile(Finish);
                }
                break;
        }
        return null;
    }

    private static void CalculateCanvas() {
       //Calculate canvas size
        foreach (var varSection in Data.currentRace.Track.Sections) {
            Direction = SetDirection(varSection);

            switch (Direction) {
                case Direction.North:
                    Y--;
                    break;
                case Direction.East:
                    X++;
                    break;
                case Direction.South:
                    Y++;
                    break;
                case Direction.West:
                    X--;
                    break;
            }

            if (X > _xMax) {
                _xMax = X;
            }

            if (Y > _yMax) {
                _yMax = Y;
            }

            if (X < _xMin) {
                _xMin = X;
            }

            if (Y < _yMin) {
                _yMin = Y;
            }
            
        }
            
        //For the track name label
        XSize = _xMax - _xMin + 1;
        YSize = _yMax - _yMin + 1;
    }
   
   //Set new direction for next section
   public static Direction SetDirection(Section section) {
       switch (section.sectionType) {

           case SectionTypes.LeftCorner:
               switch(Direction) {
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
               switch (Direction) {
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

       return Direction;
   }

   public static void SetStartLocation() {
       X = Data.currentRace.Track.startX;
       Y = Data.currentRace.Track.startY + 1;
   }


}