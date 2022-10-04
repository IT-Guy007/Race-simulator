using System;
using Model;

namespace Controller {
    public static class Data {

        public static Competition currentCompetition { get; set; }
        public static Race currentRace { get; set; }



        public static void Initialize() {
            currentCompetition = new Competition();
            currentCompetition.Participants = new List<IParticipant>();
            addTracks();
            currentRace = new Race(currentCompetition.Tracks.Dequeue(), currentCompetition.Participants);


            addDriver("Max Verstappen", TeamColors.Yellow);
            addDriver("Sergio Perez", TeamColors.Yellow);
            addDriver("Charles Lecrerc", TeamColors.Red);
            addDriver("Carlos Sainz", TeamColors.Red);
            addDriver("Lewis Hamilton", TeamColors.Grey);
            addDriver("George Russell", TeamColors.Grey);

        }

        public static void NextRace() {
            currentRace = new Race(currentCompetition.Tracks.Dequeue(), currentCompetition.Participants);

        }

        public static void addDriver(string name, TeamColors color) {
            currentCompetition.Participants.Add(new Driver(name,color));
        }

        public static void addTracks() {
            SectionTypes[] zandvoortSections = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner };
            SectionTypes[] monzaSections = { SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner };
            SectionTypes[] spaSections = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.Straight, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner };

            currentCompetition.Tracks.Enqueue(new Track("Zandvoort", createSections(zandvoortSections)));
            currentCompetition.Tracks.Enqueue(new Track("Monza", createSections(monzaSections)));
            currentCompetition.Tracks.Enqueue(new Track("Spa", createSections(spaSections)));
        }

        public static LinkedList<Section> createSections(SectionTypes[] trackSections) {
            LinkedList<Section> sectionList = new LinkedList<Section>();

            for (int i = 0; i != trackSections.Length; i++)
            {
                sectionList.AddLast(new Section(trackSections[i]));
            }

            return sectionList;

        }


    }
}

