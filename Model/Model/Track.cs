using System;
namespace Model {
    public class Track {
        public String naam { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public SectionTypes[] Sections1 { get; }
        public SectionData SectionTypes { get; }

        public Track(string naam, SectionTypes[] sections)
        {
            this.naam = naam;
            Sections1 = sections;
        }

        public Track()
        {
        }
    }
}

