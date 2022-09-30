using System;
using Model;

namespace Controller {

    public static class Data {
        public static Competition? comp { get; set; }
        public static Race? currentRace { get; set; }

        public static void initialize() {
            comp = new Competition();
            comp.Participants = new List<IParticipant>();

        }

        public static void addParticipants(string naam) {
            comp.Participants.Add(new Driver(naam));
        }

        public static void addTrack(String naam, SectionTypes[] sections) {
            comp.Tracks.Enqueue(new Track(naam, sections));
        }

        public static void nextRace() {
            if(comp.nextTrack() != null){
                currentRace = new Race(track: comp.nextTrack(), comp.Participants);
            } 
        }
    }
}

