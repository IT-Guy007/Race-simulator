using System;
namespace Model {
    public class Competition {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }


        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }

        public Track NextTrack() {
            return new Track();
        }

    }
}

