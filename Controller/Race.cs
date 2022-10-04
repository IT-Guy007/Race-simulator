using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;


namespace Controller
{
    public class Race
    {
        public Track Track { get; set; } = new Track();
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random;
        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();
        private System.Timers.Timer Rondetijd;
        public event EventHandler DriverChanged;
        public int gehaald = 0;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
        }

        public void Start() {
            Rondetijd.Enabled = true;
        }

        public SectionData GetSectionData(Section sectie)
        {
            SectionData returnstring = new SectionData();
            foreach (KeyValuePair<Section, SectionData> kvp in _positions)
            {
                if (kvp.Key == sectie)
                {
                    returnstring = kvp.Value;
                }
            }
            return returnstring;
        }
        
        
        public void RandomizeEquipment()
        {
            foreach (IEquipment auto in Participants)
            {
                auto.quality = _random.Next();
                auto.performance = _random.Next();
            }
        }
    }
}