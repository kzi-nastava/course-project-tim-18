using System;
using System.Collections.Generic;

namespace HealthCare
{
    public class Room
    {
        private string name;
        private RoomType roomType;
        private List<Equipment> equipmentList = new List<Equipment>();

        public Room(RoomType roomType, string name)
        {
            this.roomType = roomType;
            this.name = name;
        }

        public Room(RoomType roomType, string name, List<Equipment> equipmentList)
        {
            this.roomType = roomType;
            this.name = name;
            this.equipmentList = equipmentList;
        }
    }
}
