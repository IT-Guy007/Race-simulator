using System.Timers;
using Model;


namespace Controller;

public class Race {
    //The current track
    public Track Track { get; }
    
    //The current participants
    public List<IParticipant> Participants { get; }
    
    //Randomizer even though it is not completely random because that is impossible in computer science
    private readonly Random _random;
    
    //Track laps
    private readonly Dictionary<IParticipant, int> _roundsCompleted;
    
    //SectionData for each section
    public Dictionary<Section, SectionData> SectionsSectionData { get; }
    
    //Timer for sleep and counting
    public readonly System.Timers.Timer Timer;
    
    
    //SectionsSectionData of the participants
    public static Dictionary<int, IParticipant> Position;
    
    //Last lap time
    public static Dictionary<int, TimeSpan> LastLapTime;

    //Event Handler to call the UI to update
    public event EventHandler<DriversChanged> DriversChanged;
    
    //Event handler to call when the race is finished
    public event EventHandler<EventArgs> RaceEnded;

    
    
    public Race(Track track, List<IParticipant> participants) {
        Track = track;
        Participants = participants;
        SectionsSectionData = new Dictionary<Section, SectionData>();
        _roundsCompleted = new Dictionary<IParticipant, int>();
        Position = new Dictionary<int, IParticipant>();

        _random = new Random();
        AddDriversToPositionsList();
        AddSectionDataToSections(track);
        RandomEquipment();

        Timer = new System.Timers.Timer(500);
        Timer.Elapsed += TimedEvent;
        Timer.AutoReset = true;
    }

    public void Stop() {
        Timer.Stop();
        DriversChanged = null!;
    }

    public void Start() {
        Timer.Start();
    }

    public void PlaceDriversOnTrack() {
        try {
            int amountOfDrivers = Participants.Count;
            int i = 0;
            int sectionNumber = Track.Sections.Count - 1;
            while (i != amountOfDrivers) {
                SectionsSectionData[Track.Sections.ElementAt(sectionNumber)].Left = Participants[i];
                i++;
                SectionsSectionData[Track.Sections.ElementAt(sectionNumber)].Right = Participants[i];
                i++;
                sectionNumber--;
            }
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }

    private void AddSectionDataToSections(Track track) {
        foreach (Section section in track.Sections) {
            SectionsSectionData.Add(section, new SectionData());
        }
    }

    public void RandomEquipment() {
        foreach (var participant in Participants) {
            participant.Equipment.Performance = _random.Next(participant.Equipment.Performance, 100);
            participant.Equipment.IsBroken = false;
            participant.Equipment.Quality = _random.Next(participant.Equipment.Quality, 100);
            participant.Equipment.Speed = _random.Next(participant.Equipment.Speed, 100);
        }
    }

    private SectionData GetSectionData(Section section) {
        SectionData result = SectionsSectionData.GetValueOrDefault(section, null);

        if (result == null) {
            result = new SectionData();
            SectionsSectionData.Add(section, result);
        }

        return result;
    }

    private void TimedEvent(object? source, ElapsedEventArgs e) {

        //Update each section, going backwards
        for (int i = Track.Sections.Count - 1; i >= 0; i--) {
            //Get this section
            Section currentSection = Track.Sections.ElementAt(i);

            //Get the next section
            Section nextSection;
            try {
                //Next section
                nextSection = Track.Sections.ElementAt(i + 1);
            } catch (Exception) {
                //Start again
                nextSection = Track.Sections.ElementAt(0);
            }

            //Get the section data
            SectionData currentSectionData = GetSectionData(currentSection);

            //Get the next section data
            SectionData nextSectionData = GetSectionData(nextSection);


            //Overtaking
            if (currentSectionData.Left != null && currentSectionData.Right != null) {
                if(!currentSectionData.Left.Equipment.IsBroken && !currentSectionData.Right.Equipment.IsBroken) {
                    int chance = _random.Next(currentSectionData.Left.Equipment.Speed - currentSectionData.Left.Equipment.Speed, 100);
                    if (chance > 30) {
                        
                        //Jump 2 sections ahead if possible
                        SectionData twoSectionsAdvanced = null;
                        try {
                            twoSectionsAdvanced = GetSectionData(Track.Sections.ElementAt(i + 2));
                        } catch(Exception) {
                            
                        }
                        if (twoSectionsAdvanced != null) {
                            if(twoSectionsAdvanced.Left == null) {
                               twoSectionsAdvanced.Left = currentSectionData.Left;
                               currentSectionData.Left = null;
                            } else {
                                if (twoSectionsAdvanced.Right == null) {
                                    twoSectionsAdvanced.Right = currentSectionData.Left;
                                    currentSectionData.Left = null;
                                }
                            }
                            SectionsSectionData[Track.Sections.ElementAt(i + 2)] = twoSectionsAdvanced;
                        }
                    } else {
                        //Both to next section
                        nextSectionData.Left = currentSectionData.Left;
                        nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - Section.SectionLength;
                        currentSectionData.Left = null;

                        nextSectionData.Right = currentSectionData.Right;
                        nextSectionData.DistanceRight = currentSectionData.DistanceRight - Section.SectionLength;
                        currentSectionData.Right = null;
                    }
                    
                }
            } 
            
            
            //Check if there is a car in this section
            if (currentSectionData.Left != null) {

                //Add the distance passed
                currentSectionData.DistanceLeft += currentSectionData.Left.Equipment.Distance();

                //Break the car based on quality
                if (_random.Next(0, currentSectionData.Left.Equipment.Quality) == 0)
                {
                    currentSectionData.Left.Equipment.IsBroken = true;

                    //Remove participant from currentParticipants
                    Console.WriteLine($"-----------{currentSectionData.Left.Name} has crashed-------");
                    Participants.Remove(currentSectionData.Left);
                }
            }

            if (currentSectionData.Right != null) {
                //Add the distance passed
                currentSectionData.DistanceRight += currentSectionData.Right.Equipment.Distance();
                currentSectionData.Right.Equipment.IsBroken = true;

                //Remove participant from currentParticipants
                Console.WriteLine($"-----------{currentSectionData.Right.Name} has crashed-------");
                Participants.Remove(currentSectionData.Right);
            }

            //Move cars to next section
            if (currentSectionData.Left != null) {
                int positions = 0;

                if (nextSectionData.Left != null)
                {
                    positions++;
                }

                if (nextSectionData.Right != null)
                {
                    positions++;
                }

                //Set positions
                if (positions == 2)
                {
                    nextSectionData.Left = currentSectionData.Left;
                    nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - Section.SectionLength;
                    currentSectionData.Left = null;

                    nextSectionData.Right = currentSectionData.Right;
                    nextSectionData.DistanceRight = currentSectionData.DistanceRight - Section.SectionLength;
                    currentSectionData.Right = null;
                }
                else
                {
                    if (nextSectionData.Left != null)
                    {
                        nextSectionData.Left = currentSectionData.Left;
                        nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - Section.SectionLength;
                        currentSectionData.Left = null;
                    }
                    else
                    {
                        //Force the car to the right, if broken car is there replace
                        nextSectionData.Right = currentSectionData.Right;
                        nextSectionData.DistanceRight = currentSectionData.DistanceRight - Section.SectionLength;
                        currentSectionData.Right = null;
                    }
                }

            }
            else
            {
                if (currentSectionData.Right != null)
                {
                    if (nextSectionData.Right != null)
                    {
                        nextSectionData.Right = currentSectionData.Right;
                        nextSectionData.DistanceRight = currentSectionData.DistanceRight - Section.SectionLength;
                        currentSectionData.Right = null;
                    }
                    else
                    {
                        //Force the car to the left, if broken car is there replace
                        nextSectionData.Left = currentSectionData.Right;
                        nextSectionData.DistanceLeft = currentSectionData.DistanceRight - Section.SectionLength;
                        currentSectionData.Right = null;

                    }

                }
            }

            //Check if the car has finished
            if (currentSection.sectionType == SectionTypes.Finish)
            {
                if (currentSectionData.Left != null)
                {
                    if (_roundsCompleted.ContainsKey(currentSectionData.Left))
                    {
                        _roundsCompleted[currentSectionData.Left]++;
                    }
                    else
                    {
                        _roundsCompleted.Add(currentSectionData.Left, 0);
                    }
                }

                if (currentSectionData.Right != null)
                {
                    if (_roundsCompleted.ContainsKey(currentSectionData.Right))
                    {
                        _roundsCompleted[currentSectionData.Right]++;
                    }
                    else
                    {
                        _roundsCompleted.Add(currentSectionData.Right, 0);
                    }
                }

                //Remove cars from track if on finish and finished
                if (currentSectionData.Left != null) {
                    if (_roundsCompleted.ContainsKey(currentSectionData.Left) && _roundsCompleted[currentSectionData.Left] == Track.laps) {
                        currentSectionData.Left = null;
                    }
                }
                
                if(currentSectionData.Right != null) {
                    if (_roundsCompleted.ContainsKey(currentSectionData.Right) && _roundsCompleted[currentSectionData.Right] == Track.laps) {
                        currentSectionData.Right = null;
                    }
                }
            }
        }
    
        Console.WriteLine(Participants.Count + " drivers left on track");
        if (Participants.Count > 0) {
            DriversChanged.Invoke(this, new DriversChanged(Track));
        } else {
            RaceEnded.Invoke(this,EventArgs.Empty);
        }
    }
    
    private void AddDriversToPositionsList() {
        for(int i = 0; i < Data.currentCompetition.Participants.Count; i++) {
            Position.Add(i + 1, Data.currentCompetition.Participants.ElementAt(i));
        }
        
    }
}
