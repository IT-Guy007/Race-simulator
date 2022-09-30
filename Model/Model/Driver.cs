using System;
namespace Model {
    public class Driver : IParticipant {
        private string naam;

        public Driver(string naam)
        {
            this.naam = naam;
        }

        public IEquipment Equipment { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public TeamColors TeamColor { get; set; }
    }
}

