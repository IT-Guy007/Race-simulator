using System;
using Controller;
using Microsoft.Maui.Controls;
using Model;

namespace MAUI;

public partial class MainPage {
    
    public MainPage() {
        InitializeComponent();
    }

    private void Clicked(object sender, EventArgs e) {
        Console.WriteLine("Starting new competition");
        //Initialise the data
        Data.Initialize();
        Navigation.PushAsync(new CompetitionPage());
    }
}