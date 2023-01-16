using Controller;

namespace MAUI;

public partial class RaceStatisticsPage {
    
    StackLayout _verticalStackLayout;

    public RaceStatisticsPage() {
        Initialize();
    }
    
    private void UpdateStatistics(object sender, EventArgs eventArgs) {
        Initialize();
    }
    
    private void RaceEnded(object sender, EventArgs eventArgs) {
        Console.WriteLine("Race ended: pushing competitionPage");
        Data.currentCompetition.RaceInProgress = false;
        Images.AwardPoints();
        Data.currentRace = null!;
        Navigation.PushAsync(new CompetitionPage());
    }
    
    private void Initialize() {
        //Data.currentRace.DriversChanged += UpdateStatistics;
        Data.currentRace.RaceEnded += RaceEnded;
        
        _verticalStackLayout = new StackLayout {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            Padding = new Thickness(10, 10, 10, 10)
        };

        Title = "Race statistics";

        Label title = new Label {
            Text = "Race Statistics",
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };
        _verticalStackLayout.Add(title);
        
        
        Grid grid = new Grid {

            ColumnDefinitions = {
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition()
            },
            
            RowDefinitions = {
                new RowDefinition()
            }
            
        };

        //Labels
        grid.Add(new Label {
            
            Text = "Pos",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);
        
        grid.Add(new Label {
            
            Text = "Drivers",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 1, 0);
        
        
        grid.Add(new Label {
            
            Text = "Quality",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 2, 0);        
        
        grid.Add(new Label {
            
            Text = "Speed",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 3, 0);        
        
        grid.Add(new Label {
            
            Text = "Performance",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 4, 0);
        
        grid.Add(new Label {
            
            Text = "Status",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 5, 0);

        //Drivers
        for(int i = 0; i != Data.currentCompetition.Participants.Count; i++) {
            grid.RowDefinitions.Add(new RowDefinition());
            //Number
            grid.Add(new Label {
                Text = (i + 1).ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 0, i + 1);
            
            //Driver name
            grid.Add(new Label {
                Text = Controller.Data.currentCompetition.Participants[i].Name,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 1, i + 1);
            
            
            //Quality
            grid.Add(new Label {
                Text = Controller.Data.currentCompetition.Participants[i].Equipment.Quality.ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 2, i + 1);
            
            //Speed
            grid.Add(new Label {
                Text = Controller.Data.currentCompetition.Participants[i].Equipment.Speed.ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 3, i + 1);
            
            //Performance
            grid.Add(new Label {
                Text = Controller.Data.currentCompetition.Participants[i].Equipment.Performance.ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 4, i + 1);
            
            //Status
            var statusText = "In race";
            if (Data.currentRace.FinishedDrivers.Contains(Data.currentCompetition.Participants[i])) {
                statusText = "Finished";
            } else if (Data.currentRace.CrashedDrivers.Contains(Data.currentCompetition.Participants[i])) {
                statusText = "Crashed";
            }

            grid.Add(new Label {
                Text = statusText,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 5, i + 1);
        
        }
        
        _verticalStackLayout.Add(grid);
        Content = _verticalStackLayout;
    }
    
}
