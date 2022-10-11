using Model;
using System.Timers;
using Timer = System.Timers.Timer;


namespace Controller;

public class Race {
    public Track Track { get; set; }
    public List<IParticipant> Participants { get; set; }
    private Random _random;
    private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();
    private System.Timers.Timer Rondetijd;
    public Dictionary<Section, SectionData> Positions { get; }
    public event EventHandler DriversChanged;
    public Timer Timer;
    

    public Race(Track track, List<IParticipant> participants) {
        Track = track;
        Participants = participants;
        Positions = new Dictionary<Section, SectionData>();

        foreach (Section section in track.Sections)
        {
            Positions.Add(section, new SectionData());
        }
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
    
    public void Start()
    {
        Timer.Start();
    }

    public void Stop()
    {
        Timer.Stop();
        DriversChanged = null;
    }
}
