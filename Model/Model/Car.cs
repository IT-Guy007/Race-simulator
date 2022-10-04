using System;
namespace Model {
    public class Car : IEquipment {
        public int quality { get; set; }
        public int speed { get; set; }
        public bool isBroken { get; set; }
        public int performance { get; set; }
    }
}

