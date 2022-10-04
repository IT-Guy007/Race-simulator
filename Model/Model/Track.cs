using System;
namespace Model {
    public class Track {
       
        public String name { get; set; }
        public LinkedList<Section> Sections { get; set; }


        public Track(string trackName, LinkedList<Section> trackSections) {
            this.name = trackName;
            this.Sections = trackSections;
        }

        public Track() { }
        
    }
}

