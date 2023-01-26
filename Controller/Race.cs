using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace Controller;

public class Race {
    //The current track
    public Track Track { get; }
    
    //The current participants
    public ObservableCollection<IParticipant> Participants { get; }
    
    //Randomizer even though it is not completely random because that is impossible in computer science
    private readonly Random _random;
    
    //Track laps
    private readonly Dictionary<IParticipant, int> _roundsCompleted;
    
    //SectionData for each section
    public Dictionary<Section, SectionData> SectionsSectionData;
    
    //Timer for sleep and counting
    private readonly Timer Timer;

    //SectionsSectionData of the participants
    private static Dictionary<int, IParticipant> Position;

    //Event Handler to call the UI to update
    public event EventHandler<DriversChanged> DriversChanged;
    
    //Event handler to call when the race is finished
    public event EventHandler<EventArgs> RaceEnded;

    //Start next race
    private bool StartNextRace;

    public Race(Track track, List<IParticipant> participants) {
        Track = track;
        Participants = new ObservableCollection<IParticipant>(participants);
        SectionsSectionData = new Dictionary<Section, SectionData>(); 
        _roundsCompleted = new Dictionary<IParticipant, int>();
        Position = new Dictionary<int, IParticipant>();
        StartNextRace = false;
        
        _random = new Random();
        AddDriversToPositionsList();
        AddSectionDataToSections(track);
        RandomEquipment();

        Timer = new Timer(500);
        Timer.Elapsed += TimedEvent;
        Timer.AutoReset = true;
    }

    public void Stop() {
        Data.CurrentRace.Stop();
        Data.CurrentRace.DriversChanged = null!;
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

    public SectionData GetSectionData(Section section) {
        SectionData result = SectionsSectionData.GetValueOrDefault(section, null);

        if (result == null) {
            result = new SectionData();
            SectionsSectionData.Add(section, result);
        }

        return result;
    }

    private void TimedEvent(object? source, ElapsedEventArgs e) {

        for (int index = Track.Sections.Count - 1; index >= 0; index--) {

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

                if (_random.Next(0, sectionData.Left.Equipment.Quality + 100) == 0) {
                    sectionData.Left.Equipment.IsBroken = true;
                    Participants[Participants.IndexOf(sectionData.Left)].RaceStatus = "Crashed";
                    //Console.WriteLine(Participants[Participants.IndexOf(sectionData.Left)].Name + " is " + Participants[Participants.IndexOf(sectionData.Left)].RaceStatus);
                }

                if (!sectionData.Left.Equipment.IsBroken && sectionData.DistanceLeft > Section.SectionLength) {

                    if (nextSectionData.Left == null) {

                        if (section.SectionType == SectionTypes.Finish) {
                            if (_roundsCompleted.ContainsKey(sectionData.Left)) {
                                _roundsCompleted[sectionData.Left]++;
                            } else {
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

                    } else if (nextSectionData.Right == null) {
                        if (section.SectionType == SectionTypes.Finish) {
                            if (_roundsCompleted.ContainsKey(sectionData.Left)) {
                                _roundsCompleted[sectionData.Left]++;
                            } else {
                                _roundsCompleted.Add(sectionData.Left, 0);
                            }
                        }

                        if (_roundsCompleted.ContainsKey(sectionData.Left) && _roundsCompleted[sectionData.Left] == Track.laps) {
                            Participants[Participants.IndexOf(sectionData.Left)].RaceStatus = "Finished";
                            //Console.WriteLine(Participants[Participants.IndexOf(sectionData.Left)].Name + " is " + Participants[Participants.IndexOf(sectionData.Left)].RaceStatus);
                            sectionData.Left = null;
                        } else {
                            nextSectionData.Right = sectionData.Left;
                            nextSectionData.DistanceRight = sectionData.DistanceLeft - Section.SectionLength;

                            sectionData.Left = null;
                            sectionData.DistanceLeft = 0;
                        }
                    }
                }
            } else if (sectionData.Right != null) {

                // add driven distance
                int passedDistance = sectionData.Right.Equipment.Distance();
                sectionData.DistanceRight += passedDistance;

                // break the car
                if (_random.Next(0, sectionData.Right.Equipment.Quality + 100) == 0) {
                    sectionData.Right.Equipment.IsBroken = true;
                    Participants[Participants.IndexOf(sectionData.Right)].RaceStatus = "Crashed";
                    //Console.WriteLine(Participants[Participants.IndexOf(sectionData.Right)].Name + " is " + Participants[Participants.IndexOf(sectionData.Right)].RaceStatus);

                }

                // check if able to go to next section
                if (!sectionData.Right.Equipment.IsBroken && sectionData.DistanceRight > Section.SectionLength) {
                    // try straight ahead
                    if (nextSectionData.Right == null) {
                        // add points if lapped
                        if (section.SectionType == SectionTypes.Finish) {
                            if (_roundsCompleted.ContainsKey(sectionData.Right)) {
                                _roundsCompleted[sectionData.Right]++;
                            } else {
                                _roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }

                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Right) && _roundsCompleted[sectionData.Right] == Track.laps) {
                            sectionData.Right = null;
                        } else {
                            // copy data to next section
                            nextSectionData.Right = sectionData.Right;
                            nextSectionData.DistanceRight = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;
                        }

                    }
                    // check diagonal
                    else if (nextSectionData.Left == null) {
                        // add points if lapped
                        if (section.SectionType == SectionTypes.Finish) {
                            if (_roundsCompleted.ContainsKey(sectionData.Right)) {
                                _roundsCompleted[sectionData.Right]++;
                            } else {
                                _roundsCompleted.Add(sectionData.Right, 0);
                            }
                        }

                        // remove from track when finished
                        if (_roundsCompleted.ContainsKey(sectionData.Right) && _roundsCompleted[sectionData.Right] == Track.laps) {

                            Participants[Participants.IndexOf(sectionData.Right)].RaceStatus = "Finished";
                            //Console.WriteLine(Participants[Participants.IndexOf(sectionData.Right)].Name + " is " + Participants[Participants.IndexOf(sectionData.Right)].RaceStatus);
                            sectionData.Right = null;
                        } else {
                            // copy data to next section
                            nextSectionData.Left = sectionData.Right;
                            nextSectionData.DistanceLeft = sectionData.DistanceRight - Section.SectionLength;

                            // clear data for current section
                            sectionData.Right = null;
                            sectionData.DistanceRight = 0;
                        }

                    }
                }
            }
        }

        bool nextTrack = true;

        for (int i = 0; i < Track.laps; i++)
        {
            if (_roundsCompleted.ContainsValue(i))
            {
                nextTrack = false;
            }
        }

        if (_roundsCompleted.Count != Participants.Count) {
            nextTrack = false;
        }
        

        if (nextTrack) {
            Data.CurrentRace = null!;
            Stop();
            AwardPoints();
            Data.CurrentCompetition.RaceInProgress = false;
            RaceEnded.Invoke(this, EventArgs.Empty);

        } else  {
            //Console.WriteLine("-----");
            //foreach (var m in Data.CurrentRace.DriversChanged.GetInvocationList()) {
            //    Console.WriteLine(m.Method.Name);
            //
            //}
            //Console.WriteLine("-----");
            DriversChanged.Invoke(this, new DriversChanged(Track));
        }

    }

    private static void AwardPoints() {
        foreach (KeyValuePair<int, IParticipant> racer in Race.Position)
        {
            int participant;
            switch (racer.Key)
            {
                case 1:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 25;
                    break;
                case 2:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 18;
                    racer.Value.Points += 18;
                    break;
                case 3:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 15;
                    break;
                case 4:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 12;
                    break;
                case 5:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 10;
                    racer.Value.Points += 10;
                    break;
                case 6:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 8;
                    racer.Value.Points += 8;
                    break;
                case 7:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 6;
                    racer.Value.Points += 6;
                    break;
                case 8:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 4;
                    racer.Value.Points += 4;
                    break;
                case 9:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 2;
                    racer.Value.Points += 2;
                    break;
                case 10:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 1;
                    racer.Value.Points += 1;
                    break;
                default:
                    participant = Data.CurrentCompetition.Participants.FindIndex(x => x == racer.Value);
                    Data.CurrentCompetition.Participants[participant].Points += 0;
                    break;
            }
        }
    }

    private void AddDriversToPositionsList() {
        for(int i = 0; i < Data.CurrentCompetition.Participants.Count; i++) {
            Position.Add(i + 1, Data.CurrentCompetition.Participants.ElementAt(i));
        }
        
    }
}