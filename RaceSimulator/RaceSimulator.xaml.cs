using Controller;
namespace MAUI;

public partial class RaceSimulator : ContentPage {

    private StackLayout _verticalStackLayout;

    public RaceSimulator() {

        _verticalStackLayout = new StackLayout {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.FromArgb("#ffffff")
        };

        Title = ("The great " + Data.currentRace.Track.name + " Race");

        Label trackNameLabel = new Label {
            Text = Data.currentRace.Track.name,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };
        
        
        
        
        _verticalStackLayout.Add(trackNameLabel);
        Content = _verticalStackLayout;

    }
    
}