using System;
namespace Model {
    public class Driver : IParticipant {
        public IEquipment Equipment { get; set; }
        public int DriverNumber { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public TeamColors TeamColor { get; set; }
        public string RaceStatus { get; set; }

        public Driver(string name, TeamColors color, int driverNumber, IEquipment equipment) {
            Name = name;
            TeamColor = color;
            DriverNumber = driverNumber;
            Equipment = equipment;
            Points = 0;
            RaceStatus = "In race";
        }
    }
}

