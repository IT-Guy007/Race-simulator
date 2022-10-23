namespace Model;

public class DriversChanged: EventArgs {
    
    public Track Track { get; set; }
    
    public DriversChanged(Track track) {
        Track = track;
    }
    
}