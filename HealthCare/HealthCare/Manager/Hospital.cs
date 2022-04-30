using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace HealthCare
{
    public class Hospital
    {
        private string name;
        private List<Room> rooms = new List<Room>() { new Room(RoomType.Undefined, "Magacin") };

        public Hospital(string name)
        {
            this.name = name;
        }

        [JsonConstructor]
        public Hospital(string name, List<Room> rooms)
        {
            this.name = name;
            this.rooms = rooms;
        }




        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<Room> Rooms
        {
            get => rooms;
            set => rooms = value ?? throw new ArgumentNullException(nameof(value));
        }




        public bool EquipmentExist(string name)
        {
            for (int i = 0; i < rooms.Count; i++)
                if (rooms[i].EquipmentExist(name) == true)
                    return true;


            return false;
        }


        public bool RoomExist(string name)
        {
            for (int i = 0; i < rooms.Count; i++)
                if (rooms[i].Name == name)
                    return true;
            

            return false;
        }


        public Room GetRoom(string name)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Name == name)
                    return rooms[i];
            }

            return null;
        }
    }
}
