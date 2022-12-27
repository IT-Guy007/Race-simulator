using Controller;

using Model;

namespace MAUI;

public partial class CompetitionPage : ContentPage {
    
    //Layout
    private StackLayout _verticalStackLayout;
    private StackLayout _racesVerticalLayout;
    private Grid _grid;
    private Grid _racesGrid;
    
    //Elements
    private Button _startRace;


    public CompetitionPage() {
        
        //Initialise
        _verticalStackLayout = new StackLayout {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#ffffff")
        };

        _racesVerticalLayout = new StackLayout {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#FF6242"),
            
        };

        _grid = new Grid {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#ffffff"),

            //6 columns
            ColumnDefinitions = {
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
            },

            //1 label row
            RowDefinitions = {
                new RowDefinition { Height = new GridLength(50) },
            }
        };

        _racesGrid = new Grid {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#FF6242"),

            //Left race name, right done, start, wait
            ColumnDefinitions = {
                new ColumnDefinition(),
                new ColumnDefinition()
            },

            //The amount of races, can't loop not supported
            RowDefinitions = {
                new RowDefinition { Height = new GridLength(50) },
                new RowDefinition { Height = new GridLength(50) },
                new RowDefinition { Height = new GridLength(50) }
            }
        }; 
        _startRace = new Button {
            Text = "Start",
            BackgroundColor = Color.FromArgb("#FF6242"),
            TextColor = Colors.White,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        _startRace.Clicked += (_,_) => RaceStart();
        _startRace.Clicked += (_,_) => _startRace.Text = "In progress";

        //The page
        Title = "Competition";

        //Gridlabels
        _grid.Add(new Label
        {
            Text = "Pos",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);

        _grid.Add(new Label
        {
            Text = "Driver",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 1, 0);

        _grid.Add(new Label
        {
            Text = "Points",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 2, 0);

        _grid.Add(new Label
        {
            Text = "Team",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 3, 0);


        Label races = new Label
        {
            Text = "The races",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        _grid.SetRow(races, 0);
        _grid.SetColumn(races, 5);
        _grid.SetColumnSpan(races, 2);
        _grid.Add(races);


        
        //Data
        
        //Get the list sorted by points using lambda
        int posDriver = 1;
        IOrderedEnumerable<IParticipant> drivers = Data.currentCompetition.Participants.OrderByDescending(x => x.Points);
        foreach (IParticipant driver in drivers)
        {
            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25)});
            _grid.Add(new Label {
                Text = (posDriver).ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 0, posDriver);


            //Driver names

            _grid.Add(new Label {
                Text = driver.Name,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 1, posDriver);

            //Points
            _grid.Add(new Label {
                Text = driver.Points.ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 2, posDriver);

            //Team
            _grid.Add(new Label {
                Text = driver.TeamColor.ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 3, posDriver);
            posDriver++;
        }
        


        //The races
        _grid.SetRow(_racesVerticalLayout,1);
        _grid.SetColumn(_racesVerticalLayout,5);
        _grid.SetColumnSpan(_racesVerticalLayout,2);
        _grid.Add(_racesVerticalLayout);
        
        //The raceGrid
        _racesVerticalLayout.Add(_racesGrid);
        
        
        //Looping the races to start races
        int i = 0;
        
        //Calculate amount of races done
        int toDo = Data.currentCompetition.Tracks.Count;
        int tracksDone = Data.currentCompetition.TracksDone.Count;
        
        //Done races
        foreach (Track race in Data.currentCompetition.TracksDone) {
            //Race name
            _racesGrid.Add(new BoxView {
                Color = race.backgroundColorMaui
            },0,i);

            _racesGrid.Add(new Label {
                Text = race.name,
                TextColor = Colors.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(10,10,10,10)
            }, 0, i);

            if (i == tracksDone) {
                _racesGrid.Add(new Label {
                    Text = "In progress",
                    TextColor = Colors.Orange,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(10, 10, 10, 10)
                }, 1, i);
            } else {
                _racesGrid.Add(new Label {
                    Text = "Done",
                    TextColor = Colors.Green,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(10, 10, 10, 10)
                }, 1, i);
            }
        }

        //Upcoming races
        foreach (Track race in Data.currentCompetition.Tracks) {
            
            //Race name
            _racesGrid.Add(new BoxView {
                Color = race.backgroundColorMaui
            },0,i);

            _racesGrid.Add(new Label {
                Text = race.name,
                TextColor = Colors.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(10,10,10,10)
            }, 0, i);
            
            
            //Startbutton
            _racesGrid.Add(new BoxView {
                Color = race.backgroundColorMaui
            },1,i);

            if (Data.currentCompetition.Tracks.Peek() == race) {
                _racesGrid.Add(_startRace, 1, i);
            } else if (Data.currentCompetition.Tracks.Contains(race)) {
                _racesGrid.Add(new Label {
                    Text = "To Do",
                    TextColor = Colors.White,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(10,10,10,10)
                }, 1, i);
            } else if (!Data.currentCompetition.Tracks.Contains(race)) {
                _racesGrid.Add(new Label {
                    Text = "Done",
                    TextColor = Colors.White,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(10,10,10,10)
                }, 1, i);
            }
            i++;
            toDo--;
        }

        _verticalStackLayout.Add(_grid);
        Content = _verticalStackLayout;
        
    }

    private void RaceStart() {
        if (!Data.currentCompetition.RaceInProgress) {
            Data.currentCompetition.RaceInProgress = true;
            Data.NextRace();
            
            RaceSimulator raceSimulator = new RaceSimulator();
            Window raceWindow = new Window(raceSimulator);
            Application.Current.OpenWindow(raceWindow);
            Navigation.PushAsync(new RaceStatisticsPage());
        }
    }
    
}