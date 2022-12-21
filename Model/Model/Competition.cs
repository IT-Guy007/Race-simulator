using System;
namespace Model {
    public class Competition {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        
        public bool RaceInProgress { get; set; }


        public Competition() {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            RaceInProgress = false;
        }

        public Track NextTrack() {
            if (Tracks.Count > 0) {
                return Tracks.Dequeue();
            }

            return null;
        }

    }
}

