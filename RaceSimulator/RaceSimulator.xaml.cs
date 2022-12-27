using Controller;
using Model;

namespace MAUI;

public partial class RaceSimulator{
    
    private Grid _grid;
    private readonly Direction _direction;
    private int x;
    private int y;

    public RaceSimulator() {
        InitialiseGrid();
        DrawTrack();
        _direction = Data.currentRace.Track.startDirection;

        Title = ("The great " + Data.currentRace.Track.name + " Race of " + DateTime.Now.Year);

        _grid.Add(new Label {
            Text = Data.currentRace.Track.name,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        }, 0, 0);
        

        _grid.BackgroundColor = Color.FromArgb(Data.currentRace.Track.backgroundColorMaui.ToArgbHex());
        Content = _grid;

    }

    private void InitialiseGrid() {
        _grid = new Grid();
        _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        
        //For the trackname
        _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
        
        //CanvasDimensions
        int[] canvasDimensions = Data.calculateCanvas();
        
        //Set columns
        for(int i = 0; i < canvasDimensions[0]; i++) {
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }
        
        //Set rows
        for(int i = 0; i < canvasDimensions[1]; i++) {
            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }
    }

    private void DrawTrack() {
        int[] size = Data.calculateCanvas();
        x = 5 - size[2];
        y = 5 - size[3];
        
        //For each tile
        foreach (Section section in Data.currentRace.Track.Sections) {
            DrawSection(section,x,y);
            switch (_direction) {
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
    }

    private void DrawSection(Section section, int row, int column) {
        _grid.Add(new Image {
            Source = Images.GetImageSource(section,_direction)
        }, column, row);
        Data.SetDirection(section);
    }
    
    
}