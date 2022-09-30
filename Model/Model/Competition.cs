using System;
namespace Model {
    public class Competition {
        public List<IParticipant> Participants {get; set;}
        public Queue<Track> Tracks { get; set; }
        public int atTrackItem = 0;

        public Track nextTrack() { return Tracks.Peek(); }
    }
    
}

