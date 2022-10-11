using System;
using System.ComponentModel;

namespace Model;

public class Track
{

    public String name { get; set; }
    public int laps { get; set; }
    public LinkedList<Section> Sections { get; set; }
    public Direction startDirection { get; set; }


    public Track(string trackName, LinkedList<Section> trackSections, Direction startDirection, int laps) {
        this.name = trackName;
        this.Sections = trackSections;
        this.startDirection = startDirection;
        this.laps = laps;
    }

    public Track()
    {
        this.name = "Default";
        this.Sections = new LinkedList<Section>();
    }

    public LinkedList<Section> Tranfomer(SectionTypes[] trackSections)
    {
        LinkedList<Section> sections = new LinkedList<Section>();
        foreach (SectionTypes section in trackSections)
        {
            sections.AddLast(new Section(section));
        }

        return sections;

    }
}

        
public enum Direction {
    North,
    East,
    South,
    West
}


