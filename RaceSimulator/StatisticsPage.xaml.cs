using Microsoft.Maui.Controls;

namespace MAUI;

public partial class StatisticsPage : ContentPage {

    public StatisticsPage() {
        
        Title = "Race statistics";
       
        Grid grid = new Grid {

            ColumnDefinitions = {
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition(),
                new ColumnDefinition()
            },

            RowDefinitions = {
                new RowDefinition(),
                new RowDefinition()
            }
        };
        
        //Labels
        grid.Add(new Label {
            
            Text = "Position",
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);
        
        grid.Add(new Label {
            
            Text = "Drivers",
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 1, 0);
        
        grid.Add(new Label {
            
            Text = "Round time",
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 2, 0);
        
        grid.Add(new Label {
            
            Text = "Equipment",
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }, 3, 0);
        
        
        
        Content = new VerticalStackLayout {
            IsVisible = true,
            Children = {
                grid
            }
        };
    }
}
