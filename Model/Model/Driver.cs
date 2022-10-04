using System;
namespace Model {
    public class Driver : IParticipant {
        public IEquipment Equipment { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public TeamColors TeamColor { get; set; }

        public Driver(string name, TeamColors color) {
            this.Name = name;
            this.TeamColor = color;
        }
    }
}

