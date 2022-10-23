namespace Model;

public class Track {

    public String name { get; set; }
    public int laps { get; set; }
    public LinkedList<Section> Sections { get; set; }
    public Direction startDirection { get; set; }
    public Spectre.Console.Color backgroundColor { get; set; }

    public Track(string trackName, LinkedList<Section> trackSections, Direction startDirection, int laps, Spectre.Console.Color backgroundColor) {
        this.name = trackName;
        this.Sections = trackSections;
        this.startDirection = startDirection;
        this.laps = laps;
        this.backgroundColor = backgroundColor;
    }
}

public enum Direction {
    North,
    East,
    South,
    West
}


