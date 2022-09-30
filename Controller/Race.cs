using System;
using Model;

namespace Controller {
    public class Race {
        public Track track { get; set; }
        public List<IParticipant> participants { get; set;}
        public DateTime startTime { get; set; }
        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public SectionData GetSectionData(Section section) {
            var result = _positions[section];
            Section newSection = new Section();
            SectionData newSectionData = new SectionData();

            if (result == null) {
                _positions.Add(newSection,newSectionData);
            }

            return result;
        }

        public Race(Track track, List<IParticipant> participants) {
            this.track = track;
            this.participants = participants;
            this._random = new Random(DateTime.Now.Millisecond);

        }

        public void RandomizeEquipment() {
            foreach (IParticipant participant in this.participants) {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
            }
        }

    }
}

