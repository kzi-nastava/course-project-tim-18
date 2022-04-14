using System;
using System.Collections.Generic;

namespace HealthCare
{
    public class Hospital
    {
        private string name;
        private List<Room> roomList = new List<Room>();
        private List<Equipment> stockroom = new List<Equipment>();

        public Hospital(string name)
        {
            this.name = name;
        }

        public Hospital(string name, List<Room> roomList)
        {
            this.name = name;
            this.roomList = roomList;
        }

        public Hospital(string name, List<Room> roomList, List<Equipment> stockroom)
        {
            this.name = name;
            this.roomList = roomList;
            this.stockroom = stockroom;

        }
    }
}
