using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCare
{
    public class Manager : User
    {
        private Hospital? hospital;
        private List<ManagerRequest> managerRequestList;


        public Manager()
        {
            username = "";
            password = "";
            hospital = null;
            ManagerRequestList = null;
        }



        [JsonConstructor]
        public Manager(string username, string password, Hospital hospital, List<ManagerRequest> managerRequestList)
        {
            this.username = username;
            this.password = password;
            this.hospital = hospital;
            this.ManagerRequestList = managerRequestList;
        }


        public Hospital Hospital
        {
            get => hospital;
            set => hospital = value ?? throw new ArgumentNullException(nameof(value));
        }
        public List<ManagerRequest>? ManagerRequestList { get => managerRequestList; set => managerRequestList = value; }



        public void MakeRequest()
        {

            Console.Write("Unesi naziv prostorije iz koje se uzima oprema  - ");
            string oldRoomName = Console.ReadLine();
            if (RoomExist(oldRoomName) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            Room oldRoom = GetRoom(oldRoomName);
            Console.Write("Unesi naziv opreme  - ");
            string equipmentName = Console.ReadLine();
            if (oldRoom.EquipmentExist(equipmentName) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }
            Equipment equipment = oldRoom.GetEquipment(equipmentName);



            Console.Write("Unesi kolicinu  - ");
            int amountInt;
            string amountString = Console.ReadLine();
            if (Int32.TryParse(amountString, out amountInt) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }
            if(amountInt > equipment.Amount)
            {
                Console.WriteLine("Uneta prevelika velicina.");
                return;
            }



            Console.Write("Unesi naziv prostorije u koju se oprema premesta  - ");
            string newRoomName = Console.ReadLine();
            if (RoomExist(newRoomName) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }
            Room newRoom = GetRoom(newRoomName);


            Console.WriteLine("Unesi datum u formatu godina/mesec/dan - ");
            DateTime executionDate;
            if (DateTime.TryParse(Console.ReadLine(), out executionDate) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            managerRequestList.Add(new ManagerRequest(oldRoom, newRoom, equipment, amountInt, executionDate, false));


        }




        public bool RoomExist(string name)
        {
            return hospital.RoomExist(name);        
        }

        public Room GetRoom(string name)
        {
            return hospital.GetRoom(name);
        }


        public void ExecuteTodayRequests()
        {

            for (int i = 0; i < managerRequestList.Count; i++)
            {
                ManagerRequest managerRequest = ManagerRequestList[i];

                if (managerRequest.ExecutionDate == DateTime.Today && managerRequest.Executed == false)
                {
                    Room oldRoom = GetRoom(managerRequest.NewRoom.Name);
                    
                    if (oldRoom.EquipmentExist(managerRequest.Equipment.Name) == false)
                        break;

                    Equipment oldRoomEquipment = oldRoom.GetEquipment(managerRequest.Equipment.Name);

                    Room newRoom = GetRoom(managerRequest.NewRoom.Name);

                    Equipment newRoomEquipment;


                    if (oldRoom.EquipmentExist(managerRequest.Equipment.Name) == false)
                        newRoomEquipment = new Equipment(managerRequest.Equipment.EquipmentType, managerRequest.Equipment.Name, 0);
                    else
                        newRoomEquipment = newRoom.GetEquipment(managerRequest.Equipment.Name);

                    while (oldRoomEquipment.Amount > 0 && managerRequest.Amount > 0)
                    {
                        oldRoomEquipment.Amount--;
                        managerRequest.Amount--;
                        newRoomEquipment.Amount++;
                    }

                    managerRequest.Executed = true;

                }
            }
        }

        public void Load()
        {
            string fileName = "../../../Data/ManagerData.json";
            string jsonString = File.ReadAllText(fileName);
            Manager m = JsonSerializer.Deserialize<Manager>(jsonString)!;
            username = m.Username;
            password = m.Password;
            hospital = m.Hospital;
            managerRequestList = m.ManagerRequestList;
            ExecuteTodayRequests();

        }

        public void Save()
        {
            ExecuteTodayRequests();

            string fileName = "../../../Data/ManagerData.json";
            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
