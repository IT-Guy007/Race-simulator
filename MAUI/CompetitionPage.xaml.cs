﻿namespace MAUI;

public partial class CompetitionPage : ContentPage
{
    private RacePage _racePage;
    private StatisticsPage _statisticsPage;
    
    private Window _raceWindow;
    private Window _statisticsWindow;
    
    public CompetitionPage() {
        InitializeComponent();
        _racePage = new RacePage();
        _statisticsPage = new StatisticsPage();
        
        _raceWindow = new Window{Page = _racePage};
        _statisticsWindow = new Window{Page = _statisticsPage};
    }

    private void RaceStart(object sender, EventArgs e) {
        Application.Current.OpenWindow(_raceWindow);
        Application.Current.OpenWindow(_statisticsWindow);
    }
}