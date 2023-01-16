﻿using System.Timers;
using Model;
using Timer = System.Timers.Timer;

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
    private readonly Timer Timer;
    
    //If race is done
    private bool _startNextRace;

    //SectionsSectionData of the participants
    public static Dictionary<int, IParticipant> Position;

    //Crashed drivers
    public List<IParticipant> CrashedDrivers;
    
    //Finished drivers
    public List<IParticipant> FinishedDrivers;

    //Event Handler to call the UI to update
    public event EventHandler<DriversChanged> DriversChanged;
    
    //Event handler to call when the race is finished
    public event EventHandler<EventArgs> RaceEnded;

    //Start next race
    private bool StartNextRace;

    public Race(Track track, List<IParticipant> participants) {
        Track = track;
        Participants = participants;
        SectionsSectionData = new Dictionary<Section, SectionData>(); 
        _roundsCompleted = new Dictionary<IParticipant, int>();
        Position = new Dictionary<int, IParticipant>();
        CrashedDrivers = new List<IParticipant>();
        FinishedDrivers = new List<IParticipant>();
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
        Data.currentRace.Stop();
        Data.currentRace.DriversChanged = null!;
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

                if (_random.Next(0, sectionData.Left.Equipment.Quality) == 0) {
                    sectionData.Left.Equipment.IsBroken = true;
                    CrashedDrivers.Add(sectionData.Left);
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
                if (_random.Next(0, sectionData.Right.Equipment.Quality) == 0) {
                    sectionData.Right.Equipment.IsBroken = true;
                    CrashedDrivers.Add(sectionData.Right);

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
                Console.WriteLine(" false");
                nextTrack = false;
            }
        }

        if (_roundsCompleted.Count != Participants.Count)
        {
            Console.Write(" false ");
            nextTrack = false;
        }

        Console.Write(nextTrack);

        if (nextTrack)
        {
            Console.Write("---Invoking race ended---");
            Stop();
            RaceEnded.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Console.Write(" Invoking drivers changed");
            DriversChanged.Invoke(this, new DriversChanged(Track));
        }

    }

    private void AddDriversToPositionsList() {
        for(int i = 0; i < Data.currentCompetition.Participants.Count; i++) {
            Position.Add(i + 1, Data.currentCompetition.Participants.ElementAt(i));
        }
        
    }
}