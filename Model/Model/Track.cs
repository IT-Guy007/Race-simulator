namespace Model;

public class Track {

    public String name { get; set; }
    public int laps { get; set; }
    public LinkedList<Section> Sections { get; set; }
    public Direction startDirection { get; set; }
    public Spectre.Console.Color backgroundColorSpectre { get; set; }
    
    public Microsoft.Maui.Graphics.Color backgroundColorMaui { get; set; } 
    
    public int startX { get; set; }
    public int startY { get; set; }

    public Track(string trackName, LinkedList<Section> trackSections, Direction startDirection, int laps, Spectre.Console.Color backgroundColorSpectre, Microsoft.Maui.Graphics.Color backgroundColorMaui, int startX, int startY) {
        this.name = trackName;
        this.Sections = trackSections;
        this.startDirection = startDirection;
        this.laps = laps;
        this.backgroundColorSpectre = backgroundColorSpectre;
        this.backgroundColorMaui = backgroundColorMaui;
        this.startX = startX;
        this.startY = startY;
    }
    
    public Track() {}
}

public enum Direction {
    North,
    East,
    South,
    West
}


