using System.ComponentModel;
using Controller;
using Model;

namespace Controller;

public class RaceStatisticsViewModel : INotifyPropertyChanged {
    //The event that is fired when a property is changed
    public event PropertyChangedEventHandler PropertyChanged;
    
    //Using lambda so that the data is shown in the view
    public List<string> DriverNames => Data.CurrentRace.Participants.Select(x => $"{x.Name}").ToList();
    public List<string> EquipmentSpeed => Data.CurrentRace.Participants.Select(x => $"{x.Equipment.Speed}").ToList();
    public List<string> EquipmentQuality => Data.CurrentRace.Participants.Select(x => $"{x.Equipment.Quality}").ToList();
    public List<string> EquipmentPerformance => Data.CurrentRace.Participants.Select(x => $"{x.Equipment.Performance}").ToList();
    public List<string> DriverStatus => Data.CurrentRace.Participants.Select(x => $"{x.RaceStatus}").ToList();

    public RaceStatisticsViewModel() {
        if (Data.CurrentRace != null) {
            Data.CurrentRace.DriversChanged += DriversChangedInvoker;
        }
    }

    private void DriversChangedInvoker(object s, DriversChanged e) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        Data.CurrentRace.DriversChanged += DriversChangedInvoker;
    }

}