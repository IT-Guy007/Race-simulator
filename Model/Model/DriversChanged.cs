namespace Model;

public class DriversChanged: EventArgs {
    private Track Track { get; }
    
    public DriversChanged(Track track) {
        Track = track;
    }
    
}