using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace HealthCare
{
    public class Hospital
    {
        private string name;
        private List<Room> roomList = new List<Room>() { new Room(RoomType.Undefined, "Magacin") };

        public Hospital(string name)
        {
            this.name = name;
        }

        [JsonConstructor]
        public Hospital(string name, List<Room> roomList)
        {
            this.name = name;
            this.roomList = roomList;
        }




        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<Room> RoomList
        {
            get => roomList;
            set => roomList = value ?? throw new ArgumentNullException(nameof(value));
        }




        public bool EquipmentExist(string name)
        {
            for (int i = 0; i < roomList.Count; i++)
                if (roomList[i].EquipmentExist(name) == true)
                    return true;


            return false;
        }


  



        public bool RoomExist(string name)
        {
            for (int i = 0; i < roomList.Count; i++)
                if (roomList[i].Name == name)
                    return true;
            

            return false;
        }


        public Room GetRoom(string name)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].Name == name)
                    return roomList[i];
            }

            return null;
        }
    }
}
