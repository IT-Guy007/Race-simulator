using System;
namespace Model {
    public interface IEquipment {
        public int quality { get; set; }
        public int speed { get; set; }
        public int performance { get; set; }
        public bool isBroken { get; set; }
    }
}

