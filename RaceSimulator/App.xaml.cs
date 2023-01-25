using Microsoft.Maui.Controls;

namespace MAUI;

public partial class App {
    
    public App() {
        
        InitializeComponent();

        MainPage = new NavigationPage(new MainPage());
    }
}