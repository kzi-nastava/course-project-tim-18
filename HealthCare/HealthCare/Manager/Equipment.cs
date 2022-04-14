using System;
namespace HealthCare
{
    public class Equipment
    {
        private EquipmentType equipmentType;
        private string name;
        private int amount = 0;

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

    }
}
