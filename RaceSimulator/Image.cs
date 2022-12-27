using Model;

namespace MAUI;
public static class Images {

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

    public static ImageSource GetImageSource(Section section, Direction direction) {
        switch (section.sectionType) {
            case SectionTypes.Straight:
                switch(direction) {
                    case Direction.North:
                        return ImageSource.FromFile(Straight);
                    case Direction.South:
                        return ImageSource.FromFile(Straight);
                    case Direction.East:
                        return ImageSource.FromFile(StraightUp);
                    case Direction.West:
                        return ImageSource.FromFile(StraightUp);
                }
                break;

            case SectionTypes.LeftCorner:
                switch(direction) {
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
                switch(direction) {
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
                switch(direction) {
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
                switch(direction) {
                    case Direction.North:
                        return ImageSource.FromFile(Finish);
                    case Direction.South:
                        return ImageSource.FromFile(Finish);
                    case Direction.East:
                        return ImageSource.FromFile(FinishUp);
                    case Direction.West:
                        return ImageSource.FromFile(FinishUp);
                }
                break;
        }

        return ImageSource.FromFile(StraightUp);
    }
}