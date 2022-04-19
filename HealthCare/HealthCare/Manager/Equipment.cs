using System;
using System.Text.Json.Serialization;

namespace HealthCare
{
    public class Equipment
    {
        private EquipmentType equipmentType;
        private string name;
        private int amount = 0;



        [JsonConstructor]
        public Equipment(EquipmentType equipmentType, string name, int amount)
        {
            this.name = name;
            this.amount = amount;
            this.equipmentType = equipmentType;
        }

        public Equipment(EquipmentType equipmentType, string name)
        {
            this.equipmentType = equipmentType;
            this.name = name;
        }

        public EquipmentType EquipmentType
        {
            get => equipmentType;
            set => equipmentType = value;
        }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Amount
        {
            get => amount;
            set => amount = value;
        }

        

    }
}
