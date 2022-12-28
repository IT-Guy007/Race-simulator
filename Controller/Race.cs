using System.Timers;
using Model;


namespace Controller;

public class Race {
    public Track Track { get; }
    public List<IParticipant> Participants { get; }
    private readonly Random _random;
    private readonly Dictionary<IParticipant, int> _roundsCompleted;
    public Dictionary<Section, SectionData> Positions { get; }
    public readonly System.Timers.Timer _timer;
    private bool _startNextRace;
    
    public event EventHandler<DriversChanged> DriversChanged;
    public event EventHandler<EventArgs> RaceEnded;
    
    
    public Race(Track track, List<IParticipant> participants) {
        Track = track;
        Participants = participants;
        Positions = new Dictionary<Section, SectionData>();
        _roundsCompleted = new Dictionary<IParticipant, int>();

        _random = new Random();
        AddSectionsToPositions(track);
        RandomEquipment();
        
        _timer = new System.Timers.Timer(500);
        _timer.Elapsed += TimeEvent;
        _timer.AutoReset = true;
    }

    public void PlaceDriversInQue() {
        foreach( Section section in Track.Sections.Reverse()) {
            
            if (section.sectionType == SectionTypes.StartGrid) {
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

    private void AddSectionsToPositions(Track track) {
        foreach (Section section in track.Sections) {
            Positions.Add(section, new SectionData());
        }
    }

    public void RandomEquipment() {
        foreach (var participant in Participants) {
            participant.Equipment.Performance = _random.Next(80, 100);
            participant.Equipment.IsBroken = false;
            participant.Equipment.Quality = _random.Next(80, 100);
            participant.Equipment.Speed = _random.Next(80, 100);
        }
    }
    
    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        DriversChanged = null!;
    }

    private void TimeEvent(object? source, ElapsedEventArgs e) {

        bool rerender = false;
        
        for (int index = Track.Sections.Count - 1; index >= 0; index--) 
        {
            // Get current and +1 section
            Section section = Track.Sections.ElementAt(index);
            SectionData sectionData = GetSectionData(section);
            Section nextSection;

            try {
                nextSection = Track.Sections.ElementAt(index + 1);
            } catch (Exception) {
                nextSection = Track.Sections.ElementAt(0);
            }

            SectionData nextSectionData = GetSectionData(nextSection);

            //Breaking the car
            if (sectionData.Left != null) {
                
                int passedDistance = sectionData.Left.Equipment.Distance();
                sectionData.DistanceLeft += passedDistance;
                
                if (_random.Next(0, sectionData.Left.Equipment.Quality) == 0) {
                    sectionData.Left.Equipment.IsBroken = true;
                    Console.WriteLine($"-----------{sectionData.Left.Name} is broken-------");
                    rerender = true;
                }
                
                if (!sectionData.Left.Equipment.IsBroken && sectionData.DistanceLeft > Section.SectionLength) {
                    
                    if (nextSectionData.Left == null) {

                        if (section.sectionType == SectionTypes.Finish)
                        {
                            if (_roundsCompleted.ContainsKey(sectionData.Left))
                            {
                                _roundsCompleted[sectionData.Left]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }


                        if (_roundsCompleted.ContainsKey(sectionData.Left) && _roundsCompleted[sectionData.Left] == Track.laps) {
                            sectionData.Left = null;
                        } else {

                            nextSectionData.Left = sectionData.Left;
                            nextSectionData.DistanceLeft = sectionData.DistanceLeft - Section.SectionLength;


                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }

                        rerender = true;
                    } else if (nextSectionData.Right == null) {
                        if (section.sectionType == SectionTypes.Finish) {
                            if (_roundsCompleted.ContainsKey(sectionData.Left)) {
                                _roundsCompleted[sectionData.Left]++;
                            } else {
                                _roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }
                        
                        if (_roundsCompleted.ContainsKey(sectionData.Left) &&
                            _roundsCompleted[sectionData.Left] == Track.laps) {
                            sectionData.Left = null;
                        } else {
                            nextSectionData.Right = sectionData.Left;
                            nextSectionData.DistanceRight = sectionData.DistanceLeft - Section.SectionLength;
                            
                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }

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
                if (_random.Next(0, sectionData.Right.Equipment.Quality) == 0)
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
                            if (_roundsCompleted.ContainsKey(sectionData.Right))
                            {
                                _roundsCompleted[sectionData.Right]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }

                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Right) &&
                            _roundsCompleted[sectionData.Right] == Track.laps)
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
                            if (_roundsCompleted.ContainsKey(sectionData.Right))
                            {
                                _roundsCompleted[sectionData.Right]++;
                            }
                            else
                            {
                                _roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }

                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Right) &&
                            _roundsCompleted[sectionData.Right] == Track.laps)
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
                    if (_random.Next(0, 20) == 0)
                    {
                        sectionData.Right.Equipment.IsBroken = false;
                        rerender = true;
                    }
                }
            }
        }
        
        if (rerender) {
            DriversChanged.Invoke(this, new DriversChanged(Track));
        }

        if (!_startNextRace) {
            _startNextRace = true;
            RaceEnded.Invoke(this, EventArgs.Empty);
        }
    }

    private SectionData GetSectionData(Section section) {
        SectionData result = Positions.GetValueOrDefault(section, null);

        if (result == null) {
            result = new SectionData();
            Positions.Add(section, result);
        }

        return result;
    }
}
