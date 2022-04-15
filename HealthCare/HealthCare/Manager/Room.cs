﻿using System;
using System.Collections.Generic;

namespace HealthCare
{
    public class Room
    {
        private RoomType roomType;
        private string name;
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


        public RoomType RoomType
        {
            get => roomType;
            set => roomType = value;
        }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<Equipment> EquipmentList
        {
            get => equipmentList;
            set => equipmentList = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
