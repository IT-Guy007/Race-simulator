using System.Timers;
using Model;


namespace Controller;

public class Race {
    public Track Track { get; set; }
    public List<IParticipant> Participants { get; set; }
    private readonly Random Random;
    private Dictionary<IParticipant, int> roundsCompleted;
    public Dictionary<Section, SectionData> Positions { get; }
    public System.Timers.Timer Timer;
    private bool StartNextRace;
    
    public event EventHandler<DriversChanged> DriversChanged;
    public event EventHandler<EventArgs> RaceEnded;
    
    
    public Race(Track track, List<IParticipant> participants) {
        Track = track;
        Participants = participants;
        Positions = new Dictionary<Section, SectionData>();
        roundsCompleted = new Dictionary<IParticipant, int>();

        Random = new Random();
        addSectionsToPositions(track);
        RandomEquipment();
        
        Timer = new System.Timers.Timer(500);
        Timer.Elapsed += TimeEvent;
        Timer.AutoReset = true;
    }

    public void PlaceDriversInQue() {
        
        foreach( Section section in Track.Sections.Reverse()) {
            
            if (section.sectionType == SectionTypes.StartGrid)
            {
                int i = 0;
                foreach (IParticipant participant in Participants) {
                    if (Positions[Track.Sections.ElementAt(i)].Left == null) {
                        Positions[Track.Sections.ElementAt(i)].Left = participant;
                    } else {
                        Positions[Track.Sections.ElementAt(i)].Right = participant;
                        i++;
                    }
                }
            }
            
        }
        
        for (int i = 0; i <= 7; i++) {
            Positions[Track.Sections.ElementAt(i)].Left = Participants[i];
            i += 1;
            Positions[Track.Sections.ElementAt(i)].Right = Participants[i];
            
        }
        
    }

    public void addSectionsToPositions(Track track) {
        foreach (Section section in track.Sections) {
            Positions.Add(section, new SectionData());
        }
    }

    public void RandomEquipment() {
        foreach (var participant in Participants) {
            participant.Equipment.Performance = Random.Next(80, 100);
            participant.Equipment.IsBroken = false;
            participant.Equipment.Quality = Random.Next(80, 100);
            participant.Equipment.Speed = Random.Next(80, 100);
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

    private void TimeEvent(object source, ElapsedEventArgs e)
    {

        bool rerender = false;
        
        for (int index = Track.Sections.Count - 1; index >= 0; index--)
        {
            // Get current and +1 section
            Section section = Track.Sections.ElementAt(index);
            SectionData sectionData = GetSectionData(section);
            Section nextSection;

            try
            {
                nextSection = Track.Sections.ElementAt(index + 1);
            }
            catch (Exception)
            {
                nextSection = Track.Sections.ElementAt(0);
            }

            SectionData nextSectionData = GetSectionData(nextSection);

            if (sectionData.Left != null) {

                // Add driven distance
                int passedDistance = sectionData.Left.Equipment.Distance();
                sectionData.DistanceLeft += passedDistance;

                // Break the car if distance is equal to 0
                if (Random.Next(0, sectionData.Left.Equipment.Quality) == 0)
                {
                    sectionData.Left.Equipment.IsBroken = true;
                    rerender = true;
                }

                // Check if able to go to next section
                if (!sectionData.Left.Equipment.IsBroken && sectionData.DistanceLeft > Section.SectionLength)
                {
                    // check left side
                    if (nextSectionData.Left == null)
                    {
                        // // Add points if driver goes over the finish
                        if (section.sectionType == SectionTypes.Finish)
                        {
                            if (roundsCompleted.ContainsKey(sectionData.Left))
                            {
                                roundsCompleted[sectionData.Left]++;
                            }
                            else
                            {
                                roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }

                        // Remove from track when finished
                        if (roundsCompleted.ContainsKey(sectionData.Left) &&
                            roundsCompleted[sectionData.Left] == Track.laps)
                        {
                            sectionData.Left = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Left;
                            nextSectionData.DistanceLeft = sectionData.DistanceLeft - Section.SectionLength;

                            // clear data for current section
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }

                        rerender = true;
                    }
                    //Check right side
                    else if (nextSectionData.Right == null)
                    {
                        if (section.sectionType == SectionTypes.Finish)
                        {
                            if (roundsCompleted.ContainsKey(sectionData.Left))
                            {
                                roundsCompleted[sectionData.Left]++;
                            }
                            else
                            {
                                roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }

                        // remove from track when finished
                        if (roundsCompleted.ContainsKey(sectionData.Left) &&
                            roundsCompleted[sectionData.Left] == Track.laps)
                        {
                            sectionData.Left = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Left;
                            nextSectionData.DistanceRight = sectionData.DistanceLeft - Section.SectionLength;

                            // clear data for current section
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }

                        rerender = true;
                    }
                }
                else
                {
                    // fix car 5% of the time
                    if (Random.Next(0, 20) == 0)
                    {
                        sectionData.Left.Equipment.IsBroken = false;
                        rerender = true;
                    }
                }
            }
            else if (sectionData.Right != null)
            {
                // int driverCode = sectionData.Right.GetHashCode();

                // add driven distance
                int passedDistance = sectionData.Right.Equipment.Distance();
                sectionData.DistanceRight += passedDistance;

                // break the car
                if (Random.Next(0, sectionData.Right.Equipment.Quality) == 0)
                {
                    sectionData.Right.Equipment.IsBroken = true;
                    rerender = true;
                }

                // check if able to go to next section
                if (!sectionData.Right.Equipment.IsBroken && sectionData.DistanceRight > Section.SectionLength)
                {
                    // try straight ahead
                    if (nextSectionData.Right == null)
                    {
                        // add points if lapped
                        if (section.sectionType == SectionTypes.Finish)
                        {
                            if (roundsCompleted.ContainsKey(sectionData.Right))
                            {
                                roundsCompleted[sectionData.Right]++;
                            }
                            else
                            {
                                roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }

                        // remove from track when finished
                        if (roundsCompleted.ContainsKey(sectionData.Right) &&
                            roundsCompleted[sectionData.Right] == Track.laps)
                        {
                            sectionData.Right = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Right;
                            nextSectionData.DistanceRight = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;
                        }

                        rerender = true;
                    }
                    // check diagonal
                    else if (nextSectionData.Left == null)
                    {
                        // add points if lapped
                        if (section.sectionType == SectionTypes.Finish)
                        {
                            if (roundsCompleted.ContainsKey(sectionData.Right))
                            {
                                roundsCompleted[sectionData.Right]++;
                            }
                            else
                            {
                                roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }

                        // remove from track when finished
                        if (roundsCompleted.ContainsKey(sectionData.Right) &&
                            roundsCompleted[sectionData.Right] == Track.laps)
                        {
                            sectionData.Right = null;
                        }
                        else
                        {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Right;
                            nextSectionData.DistanceLeft = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;
                        }

                        rerender = true;
                    }
                }
                else
                {
                    // fix car 5% of the time
                    if (Random.Next(0, 20) == 0)
                    {
                        sectionData.Right.Equipment.IsBroken = false;
                        rerender = true;
                    }
                }
            }
        }
        
        if (rerender)
        {
            DriversChanged.Invoke(this, new DriversChanged(Track));
        }
        
        bool nextTrack = true;

        for (int i = 0; i < Track.laps; i++)
        {
            if (roundsCompleted.ContainsValue(i))
            {
                nextTrack = false;
            }
        }

        if (roundsCompleted.Count != Participants.Count)
        {
            nextTrack = false;
        }

        if (!StartNextRace) {
            StartNextRace = true;
            RaceEnded.Invoke(this, EventArgs.Empty);
        }
    }

    private SectionData GetSectionData(Section section) {
        SectionData result = Positions.GetValueOrDefault(section, null);

        if (result == null)
        {
            result = new SectionData();
            Positions.Add(section, result);
        }

        return result;
    }
}
