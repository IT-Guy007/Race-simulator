using System;
using Controller;
using Microsoft.Maui.Controls;
using Model;

namespace MAUI;
public static class Images {

    public static Direction Direction;
    private static Random _random;
    
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
    private const string CarBroken = "car_broken.png";
    
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

    //Initialize
    public static void Initialize() {
        Direction = Data.CurrentRace.Track.startDirection;
        _random = new Random();
        CalculateCanvas();
        
    }
    
    //Returns path to image source
    public static ImageSource GetImageSource(Section section) {
        switch (section.SectionType) {
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

    //Calculate the size of the canvas
    private static void CalculateCanvas() {
       //Calculate canvas size
        foreach (var varSection in Data.CurrentRace.Track.Sections) {
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
       switch (section.SectionType) {

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

   //Set new x,y for the first section
   public static void SetStartLocation() {
       X = Data.CurrentRace.Track.startX;
       //+ 1 for the label's
       Y = Data.CurrentRace.Track.startY + 1;
   }

   //Add cars to the track
   public static string AddDrivers(IParticipant driverLeft, IParticipant driverRight) {

       if(driverLeft == null && driverRight == null) {
           return "";
       }
       
       //If left is not null, add driver left
       if (driverLeft != null && driverRight == null) {
           if(driverLeft.Equipment.IsBroken) {
               return CarBroken;
           } 
           return GetCarFromDriver(driverLeft);
           
       }
       //if right is not null, add driver right
       if (driverRight != null && driverLeft == null) {
           if(driverRight.Equipment.IsBroken) {
               return CarBroken;
           } 
           return GetCarFromDriver(driverRight);
           
       }
       
       if(!driverLeft.Equipment.IsBroken && driverRight.Equipment.IsBroken) {
           return GetCarFromDriver(driverLeft);
       }
       
       if(!driverRight.Equipment.IsBroken && driverLeft.Equipment.IsBroken) {
           return GetCarFromDriver(driverRight);
       }
       
       //If both are not null, add random driver
       int selectedDriver = _random.Next(0, 2);
       if(!driverLeft.Equipment.IsBroken && !driverRight.Equipment.IsBroken) {
           if (selectedDriver == 1) {
               return GetCarFromDriver(driverLeft);
           } 
           return GetCarFromDriver(driverRight);
           
       }

       return "";
   }

   private static string GetCarFromDriver(IParticipant driver) {
       switch (driver.TeamColor) {
           case TeamColors.Red:
               return CarRed;
           case TeamColors.Green:
               return CarGreen;
           case TeamColors.Yellow:
               return CarYellow;
           case TeamColors.Grey:
               return CarDarkblue;
           case TeamColors.Blue:
               return CarBlue;
           case TeamColors.Orange:
               return CarOrange;
           default:
              return CarBroken;
       }
   }

   //Rotate the car to the correct direction
   public static Double GetRotationOfCurrentDirection() {
       switch (Direction) {
           case Direction.North:
               return 270;
           case Direction.East:
               return 00;
           case Direction.South:
               return 90;
           case Direction.West:
               return 180;
       }

       return 270;
   }

}