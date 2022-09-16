using System;
namespace Model {
    public class Track {
        public String naam { get; set; }
        public LinkedList<Section> Sections { get; set; }

        Track(String name, SectionData SectionTypes[]) {

        }

        public Track() {

        }
    }
}

