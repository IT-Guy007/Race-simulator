using Model;

namespace MAUI;

public partial class RaceStatisticsPage : ContentPage {
    
    StackLayout _verticalStackLayout;

    public RaceStatisticsPage() {
        
        
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
                new ColumnDefinition()
            },
            
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
            
            Text = "Round time",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 2, 0);
        
        grid.Add(new Label {
            
            Text = "Equipment",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 3, 0);
        
        //Numbers
        for (int i = 0; i != 7; i++) {
            grid.Add(new Label {
                Text = (i + 1).ToString(),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 0, i + 1);
        }
        
        //Drivers
        foreach (IParticipant driver in Controller.Data.currentCompetition.Participants) {
            grid.Add(new Label {
                Text = driver.Name,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, 1, grid.RowDefinitions.Count - 1);
        }
        
        

        _verticalStackLayout.Add(grid);
        Content = _verticalStackLayout;
        
    }
}
