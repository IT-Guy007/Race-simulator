using System.Diagnostics;using Controller;using Model;namespace MAUI;public partial class RaceSimulator {        private Grid _grid;    //Labels    private Label _title;    public RaceSimulator() {        Images.Initialize();        InitialiseGrid();        DrawTrack();        Initialize();        Images.Direction = Data.currentRace.Track.startDirection;        Title = ("The great " + Data.currentRace.Track.name + " Race of " + DateTime.Now.Year);        Content = _grid;    }    //Add event handlers    private void Initialize() {        Data.currentRace.DriversChanged += DriversChanged;    }    //Create a new grid    private void InitialiseGrid() {        _grid = new Grid();        //For the track name        _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(75) });        //Set columns        for(int i = 0; i != Images.XSize; i++) {            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });        }                //Set rows        for(int i = 0; i != Images.YSize; i++) {            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });        }                //Race title        _title = new Label {            Text = Data.currentRace.Track.name,            FontSize = 30,            HorizontalOptions = LayoutOptions.Center,            VerticalOptions = LayoutOptions.Center,            TextColor = Colors.Black        };        _grid.SetColumnSpan(_title, Images.XSize);        _grid.Add(_title);        _grid.BackgroundColor = Color.FromArgb(Data.currentRace.Track.backgroundColorMaui.ToArgbHex());    }    //Insert the track on the grid    private void DrawTrack() {        //Set start position        Images.SetStartLocation();                //For each tile        try {            foreach (Section section in Data.currentRace.Track.Sections) {                DrawSection(section);                switch (Images.Direction) {                    case Direction.North:                        Images.Y--;                        break;                    case Direction.East:                        Images.X++;                        break;                    case Direction.South:                        Images.Y++;                        break;                    case Direction.West:                        Images.X--;                        break;                }            }                        //Reset content to let MAUI know that the grid has changed and the UI updates            _grid.BackgroundColor = Color.FromArgb(Data.currentRace.Track.backgroundColorMaui.ToArgbHex());            Content = _grid;        } catch (Exception e) {            Console.WriteLine("The track is not valid: ("+ Images.X + "," + Images.Y + ")");            Console.WriteLine(e);        }            }    //Draw a section on the grid    private void DrawSection(Section section) {        _grid.Add(new Image {            Source = Images.GetImageSource(section)        },  Images.X,  Images.Y);                //Add the car        string carImage = Images.AddDrivers(Data.currentRace.SectionsSectionData[section].Left, Data.currentRace.SectionsSectionData[section].Right);        if (carImage != null) {            _grid.Add(new Image {                Source = carImage,                Rotation = Images.GetRotationOfCurrentDirection(),                Scale = 0.25                            },  Images.X,  Images.Y);        }        Images.Direction = Images.SetDirection(section);    }        //When the drivers change, update the grid    private void DriversChanged(object sender, DriversChanged e) {        //Draw new track on the main thread        Console.Write(" Drawing new track");        Application.Current?.Dispatcher.Dispatch(() => {            InitialiseGrid();            DrawTrack();        });    }}