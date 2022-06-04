using System.Text.Json;
using System.Text.Json.Serialization;
using HealthCare.Doctor;
using HealthCare.Patient;

namespace HealthCare
{
    public class Manager : User
    {
        private Hospital? hospital;
        private List<ManagerRequest> managerRequests = new List<ManagerRequest>();
        private List<RenovationRequest> renovationRequest = new List<RenovationRequest>();

 

        public Manager()
        {
            username = "";
            password = "";
            hospital = null;        
        }



        [JsonConstructor]
        public Manager(string username, string password, Hospital hospital, List<ManagerRequest> managerRequests, List<RenovationRequest> renovationRequest)
        {
            this.username = username;
            this.password = password;
            this.hospital = hospital;
            this.managerRequests = managerRequests;
            this.renovationRequest = renovationRequest;

        }


        public Hospital Hospital
        {
            get => hospital;
            set => hospital = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<ManagerRequest>? ManagerRequests { get => managerRequests; set => managerRequests = value; }
        public List<RenovationRequest> RenovationRequest { get => renovationRequest; set => renovationRequest = value; }

        public void DoctorMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenuWrite();
            }
        }
        private void MainMenuPrint()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("1. CRUD operacije za bolnicu");
            Console.WriteLine("2. Pretraga i filtriranje");
            Console.WriteLine("3. Pregled opreme bolnice");
            Console.WriteLine("4. Raspoređivanje opreme po prostorijama");
            Console.WriteLine("5. Zakazivanje renoviranja sobe");
            Console.WriteLine("6. Zakazivanje slozenog renoviranja sobe (spajanje)");
            Console.WriteLine("7. Zakazivanje slozenog renoviranja sobe (razdvajanje)");
            Console.WriteLine("8. Predlaganje leka");
            Console.WriteLine("9. Exit");
            Console.Write("Izaberite opciju: ");

        }
        private void CRUDMenuPrint()
        {
            Console.WriteLine("\n1. Kreiraj prostoriju");
            Console.WriteLine("2. Prikaz prostorija");
            Console.WriteLine("3. Izmena prostorije");
            Console.WriteLine("4. Brisanje prostorije");
            Console.WriteLine("5. Vratite se nazad");
            Console.Write("Izaberite opciju: ");
        }
        private void CRUDMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = CRUDMenuWrite();
            }
        }
        private bool CRUDMenuWrite()
        {
            CRUDMenuPrint();
            switch (Console.ReadLine())
            {
                case "1":
                    CreateRoom();
                    return true;
                case "2":
                    ReadRooms();
                    return true;
                case "3":
                    UpdateRoom();
                    return true;
                case "4":
                    DeleteRoom();
                    return true;
                case "5":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos.\n");
                    return true;
            }
        }

    

        private bool MainMenuWrite()
        {
            MainMenuPrint();
            switch (Console.ReadLine())
            {
                case "1":
                    CRUDMenu();
                    return true;
                case "2":
                    Search();
                    return true;
                case "3":
                    PrintEquipment();
                    return true;
                case "4":
                    MakeRequest();
                    return true;
                case "5":
                    MakeRenovationRequest();
                    return true;
                case "6":
                    MakeComplexRenovationJoinRequest();
                    return true;
                case "7":
                    MakeComplexRenovationSplitRequest();
                    return true;
                case "8":
                    SuggestMedication();
                    return true;
                case "9":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos!\n");
                    return true;

            }

        }

        public void CreateRoom()
        {

            Console.WriteLine("Dodavanje prostorije");
            Console.WriteLine("======================");
            Console.WriteLine();

            string userResponse;

            RoomType roomType;

            Console.WriteLine("1.Operacione sala");
            Console.WriteLine("2.Sala za pregled");
            Console.WriteLine("3.Soba za odmor");
            Console.WriteLine("4.Ostalo");
            Console.WriteLine("Unesi redni broj:");


            userResponse = Console.ReadLine();

            if (userResponse == "1")
                roomType = RoomType.OperationRoom;
            else if (userResponse == "2")
                roomType = RoomType.MedicalExaminationRoom;
            else if (userResponse == "3")
                roomType = RoomType.RestingRoom;
            else
                roomType = RoomType.Undefined;

            Console.WriteLine("Unesi naziv sobe: ");

            userResponse = Console.ReadLine();

            if (RoomExist(userResponse) == true)
            {
                Console.WriteLine("Ime sobe zauzeto.");
                return;
            }

            string roomName = userResponse;


            hospital.Rooms.Add(new Room(roomType, roomName));


        }


        public void ReadRooms()
        {

            foreach (Room room in hospital.Rooms)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Naziv prostorije: " + room.Name);
                Console.WriteLine("Tip prostorije: " + room.Name);
                Console.WriteLine("-----------------------------");

            }

        }
        public void DeleteRoom()
        {
            string userResponse;

            Console.WriteLine("Unesi naziv sobe: ");

            userResponse = Console.ReadLine();

            if (RoomExist(userResponse) == false)
            {
                Console.WriteLine("Soba sa tim nazivom ne postoji.");
                return;
            }


            foreach (Room room in hospital.Rooms)
            {
                if (room.Name == userResponse)
                {
                    hospital.Rooms.Remove(room);
                    return;
                }    
            }
        }


        public void UpdateRoom()
        {
            string userResponse;

            Console.WriteLine("Unesi naziv sobe: ");

            string roomName = Console.ReadLine();

            if (RoomExist(roomName) == false)
            {
                Console.WriteLine("Soba sa tim nazivom ne postoji.");
                return;
            }


            Console.WriteLine("Unesi novi  naziv sobe: ");

            userResponse = Console.ReadLine();

            string newRoomName = userResponse;

            RoomType roomType;

            Console.WriteLine("1.Operacione sala");
            Console.WriteLine("2.Sala za pregled");
            Console.WriteLine("3.Soba za odmor");
            Console.WriteLine("4.Ostalo");
            Console.WriteLine("Unesi novi tip prostorije: ");

            userResponse = Console.ReadLine();

            if (userResponse == "1")
                roomType = RoomType.OperationRoom;
            else if (userResponse == "2")
                roomType = RoomType.MedicalExaminationRoom;
            else if (userResponse == "3")
                roomType = RoomType.RestingRoom;
            else
                roomType = RoomType.Undefined;


            foreach (Room room in hospital.Rooms)
            {
                if (room.Name == roomName)
                {
                    room.Name = newRoomName;
                    room.RoomType = roomType;
                    return;
                }
            }
        }

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

            managerRequests.Add(new ManagerRequest(oldRoom, newRoom, equipment, amountInt, executionDate, false));


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

            if (managerRequests == null)
                return;


            for (int i = 0; i < managerRequests.Count; i++)
            {
                ManagerRequest managerRequest = managerRequests[i];

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


        public void PrintEquipment()
        {
            foreach (Room room in hospital.Rooms)
            {
                foreach (Equipment equipment in room.EquipmentList)
                {

                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("Naziv prostorije: " + room.Name);
                    Console.WriteLine("Naziv opreme: " + equipment.Name);
                    Console.WriteLine("Količina opreme: " + equipment.Amount);
                    Console.WriteLine("------------------------------------");

                }
            }
        }



        public void Search()
        {

            bool roomTypeImportant = false;
            bool equipmentTypeImportant = false;
            bool equipmentNameImportant = false;
            bool amountImportant = false;


            RoomType roomType = RoomType.Undefined;
            EquipmentType equipmentType = EquipmentType.HallwayEquipment;
            string equipmentName = "";
            int minAmonut = 0;
            int maxAmount = int.MaxValue;

            string userResponse;

            Console.WriteLine("Da li ti je bitan tip prosotorije? (da/ne)");

            userResponse = Console.ReadLine();

            if (userResponse == "da")
            {
                roomTypeImportant = true;

                Console.WriteLine("1.Operacione sala");
                Console.WriteLine("2.Sala za pregled");
                Console.WriteLine("3.Soba za odmor");
                Console.WriteLine("4.Ostalo");
                Console.WriteLine("Unesi redni broj:"); 


                userResponse = Console.ReadLine();

                if (userResponse == "1")
                    roomType = RoomType.OperationRoom;
                else if (userResponse == "2")
                    roomType = RoomType.MedicalExaminationRoom;
                else if (userResponse == "3")
                    roomType = RoomType.RestingRoom;
                else
                    roomType = RoomType.Undefined;


                
            }

            Console.WriteLine("Da li ti je bitan tip opreme? (da/ne)");

            userResponse = Console.ReadLine();

            if (userResponse == "da")
            {
                equipmentTypeImportant = true;


                Console.WriteLine("1.Oprema za pregled");
                Console.WriteLine("2.Oprema za operaciju");
                Console.WriteLine("3.Sobni nameštaj");
                Console.WriteLine("4.Oprema za hodnik");
                Console.WriteLine("Unesi redni broj:");


                userResponse = Console.ReadLine();

                if (userResponse == "1")
                    equipmentType = EquipmentType.MedicalExaminationTools;
                else if (userResponse == "2")
                    equipmentType = EquipmentType.OperationTools;
                else if (userResponse == "3")
                    equipmentType = EquipmentType.RoomFurniture;
                else
                    equipmentType = EquipmentType.HallwayEquipment;


            }



            Console.WriteLine("Da li ti je bitan naziv opreme? (da/ne)");

            userResponse = Console.ReadLine();

            if (userResponse == "da")
            {
                equipmentNameImportant = true;

                Console.WriteLine("Unesi naziv opreme (pazi na velika i mala slova): ");

                equipmentName = Console.ReadLine();


            }



            Console.WriteLine("Da li ti je bitna količina opreme? (da/ne)");

            userResponse = Console.ReadLine();


            if (userResponse == "da")
            {
                amountImportant = true;

                Console.WriteLine("1.Nema na stanju");
                Console.WriteLine("2.Od 1 dod 10");
                Console.WriteLine("3.Više od 10");
                Console.WriteLine("Unesi redni broj:");

                userResponse = Console.ReadLine();

                if (userResponse == "1")
                {
                    minAmonut = 0;
                    maxAmount = 0;
                }
                else if (userResponse == "2")
                {

                    minAmonut = 1;
                    maxAmount = 10;
                }

                else
                {
                    minAmonut = 10;
                    maxAmount = int.MaxValue;

                }


            }


            foreach (Room room in hospital.Rooms)
            {
                if (roomTypeImportant == true && room.RoomType != roomType)
                    continue;

                foreach (Equipment equipment in room.EquipmentList)
                {
                    if (equipmentTypeImportant == true && equipmentType != equipment.EquipmentType)
                        continue;

                    if (equipmentNameImportant == true && equipmentName != equipment.Name)
                        continue;

                    if (amountImportant == true && (equipment.Amount < minAmonut || equipment.Amount > maxAmount))
                        continue;

                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("Naziv prostorije: " + room.Name);
                    Console.WriteLine("Naziv opreme: " + equipment.Name);
                    Console.WriteLine("Količina opreme: " + equipment.Amount);
                    Console.WriteLine("------------------------------------");


                }

            }

        }


        public void MakeRenovationRequest()
        {

            List<Appointment> appointments =  Appointment.appointmentsDeserialization();




            Console.Write("Unesi naziv prostorije koje se renovira - ");
            string roomName = Console.ReadLine();
            if (RoomExist(roomName) == false)
            {
                Console.WriteLine("Ne postoji prostorija sa tim nazivom.");
                return;
            }


            Console.WriteLine("Unesi datum početka renoviranja u formatu godina/mesec/dan - ");
            DateTime renovationStart;
            if (DateTime.TryParse(Console.ReadLine(), out renovationStart) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            Console.WriteLine("Unesi datum završetka renoviranja u formatu godina/mesec/dan - ");
            DateTime renovationEnd;
            if (DateTime.TryParse(Console.ReadLine(), out renovationEnd) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            foreach (Appointment appointment in appointments)
            {
                DateTime appointmentDate = Appointment.stringToDateTime(appointment.TimeOfAppointment);

                if (appointment.RoomId != roomName)
                    continue; 

                if (appointmentDate >= renovationStart && appointmentDate <= renovationEnd)
                {
                    Console.WriteLine("Postoje pregledi u tom periodu, nemoguće renoviranje.");
                    return;
                }


            }


            renovationRequest.Add(new RenovationRequest(roomName, renovationStart, renovationEnd));


        }




        public void MakeComplexRenovationSplitRequest()
        {

            List<Appointment> appointments = Appointment.appointmentsDeserialization();

            Console.Write("Unesi naziv prostorije koje se renovira - ");
            string roomName = Console.ReadLine();
            if (RoomExist(roomName) == false)
            {
                Console.WriteLine("Ne postoji prostorija sa tim nazivom.");
                return;
            }

            Console.WriteLine("Unesi datum početka renoviranja u formatu godina/mesec/dan - ");
            DateTime renovationStart;
            if (DateTime.TryParse(Console.ReadLine(), out renovationStart) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            Console.WriteLine("Unesi datum završetka renoviranja u formatu godina/mesec/dan - ");
            DateTime renovationEnd;
            if (DateTime.TryParse(Console.ReadLine(), out renovationEnd) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            foreach (Appointment appointment in appointments)
            {
                DateTime appointmentDate = Appointment.stringToDateTime(appointment.TimeOfAppointment);

                if (appointment.RoomId != roomName)
                    continue;

                if (appointmentDate >= renovationStart && appointmentDate <= renovationEnd)
                {
                    Console.WriteLine("Postoje pregledi u tom periodu, nemoguće renoviranje.");
                    return;
                }


            }

            Console.Write("Unesi naziv nove prostorije prve - ");
            string roomNameFirst = Console.ReadLine();


            Console.Write("Unesi naziv nove prostorije druge - ");
            string roomNameSecond = Console.ReadLine();



            Room room = GetRoom(roomName);

            List<Equipment> roomFirstEquipment = new List<Equipment>();
            List<Equipment> roomSecondEquipment = new List<Equipment>();


            foreach (Equipment equipment in room.EquipmentList)
            {
                roomFirstEquipment.Add(new Equipment(equipment.EquipmentType, equipment.Name, equipment.Amount / 2));
                roomSecondEquipment.Add(new Equipment(equipment.EquipmentType, equipment.Name, equipment.Amount - equipment.Amount / 2));
            }

            Room roomFirst = new Room(room.RoomType, roomNameFirst, roomFirstEquipment);
            Room roomSecond = new Room(room.RoomType, roomNameSecond, roomSecondEquipment);

            hospital.Rooms.Remove(room);

            hospital.Rooms.Add(roomFirst);
            hospital.Rooms.Add(roomSecond);

            renovationRequest.Add(new RenovationRequest(roomName, renovationStart, renovationEnd));
            renovationRequest.Add(new RenovationRequest(roomNameFirst, renovationStart, renovationEnd));
            renovationRequest.Add(new RenovationRequest(roomNameSecond, renovationStart, renovationEnd));


        }


        public void MakeComplexRenovationJoinRequest()
        {

            List<Appointment> appointments = Appointment.appointmentsDeserialization();

            Console.Write("Unesi naziv prve prostorije koje se renovira - ");
            string roomNameFirst = Console.ReadLine();
            if (RoomExist(roomNameFirst) == false)
            {
                Console.WriteLine("Ne postoji prostorija sa tim nazivom.");
                return;
            }

            Console.Write("Unesi naziv druge prostorije koje se renovira - ");
            string roomNameSecond = Console.ReadLine();
            if (RoomExist(roomNameSecond) == false)
            {
                Console.WriteLine("Ne postoji prostorija sa tim nazivom.");
                return;
            }


            Console.WriteLine("Unesi datum početka renoviranja u formatu godina/mesec/dan - ");
            DateTime renovationStart;
            if (DateTime.TryParse(Console.ReadLine(), out renovationStart) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            Console.WriteLine("Unesi datum završetka renoviranja u formatu godina/mesec/dan - ");
            DateTime renovationEnd;
            if (DateTime.TryParse(Console.ReadLine(), out renovationEnd) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            foreach (Appointment appointment in appointments)
            {
                DateTime appointmentDate = Appointment.stringToDateTime(appointment.TimeOfAppointment);

                if (appointment.RoomId != roomNameFirst  && appointment.RoomId != roomNameSecond)
                    continue;

                if (appointmentDate >= renovationStart && appointmentDate <= renovationEnd)
                {
                    Console.WriteLine("Postoje pregledi u tom periodu, nemoguće renoviranje.");
                    return;
                }


            }

            Console.Write("Unesi naziv nove prostorije - ");
            string newRoomName = Console.ReadLine();



            Room roomFirst = GetRoom(roomNameFirst);
            Room roomSecond = GetRoom(roomNameSecond);



            Dictionary<string, Equipment> equipmentCounter = new Dictionary<string, Equipment>();

            foreach (Equipment equipment in roomFirst.EquipmentList)
            {
                equipmentCounter[equipment.Name] = equipment;
            }


            foreach (Equipment equipment in roomSecond.EquipmentList)
            {
                if (equipmentCounter.ContainsKey(equipment.Name))
                {
                    equipmentCounter[equipment.Name].Amount += equipment.Amount;
                }
                else
                {
                    equipmentCounter[equipment.Name] = equipment;

                }
            }



            Room newRoom = new Room(roomFirst.RoomType, newRoomName, equipmentCounter.Values.ToList());


            hospital.Rooms.Remove(roomFirst);
            hospital.Rooms.Remove(roomSecond);


            hospital.Rooms.Add(newRoom);


            renovationRequest.Add(new RenovationRequest(newRoomName, renovationStart, renovationEnd));


        }


        void SuggestMedication()
        {


            Console.Write("Unesi naziv leka - ");
            string medicationName = Console.ReadLine();
     
            
            Console.Write("Unesi koliko puta lek treba da se uzima dnevno  - ");
            int timesADay;
            string timesADayString = Console.ReadLine();
            if (Int32.TryParse(timesADayString, out timesADay) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }



            Console.WriteLine("1.Pre obroka");
            Console.WriteLine("2.Tokom obroka");
            Console.WriteLine("3.Posle obroka");
            Console.WriteLine("4.Nebitno");
            Console.WriteLine("Unesi vreme kad lek treba da se unese:");




            string userResponse = Console.ReadLine();
            TimeForMedicine timeForMedicine;


            if (userResponse == "1")
                timeForMedicine = TimeForMedicine.BeforeTheMeal;
            else if (userResponse == "2")
                timeForMedicine = TimeForMedicine.DuringTheMeal;
            else if (userResponse == "3")
                timeForMedicine = TimeForMedicine.AfterTheMeal;
            else
                timeForMedicine = TimeForMedicine.Irrelevant;


            Console.WriteLine("1.Penicilin");
            Console.WriteLine("2.Antibiotik");
            Console.WriteLine("3.Sulfonamid");
            Console.WriteLine("4.Antikonvulziv");
            Console.WriteLine("5.NSAIL");
            Console.WriteLine("Unesi redni broj sajstojka koji je sadržan u leku:");


             userResponse = Console.ReadLine();
            Allergy allergy;


            if (userResponse == "1")
                allergy = Allergy.Penicilin;
            else if (userResponse == "2")
                allergy = Allergy.Antibiotic;
            else if (userResponse == "3")
                allergy = Allergy.Sulfonamides;
            else if (userResponse == "4")
                allergy = Allergy.Anticonvulsants;
            else
                allergy = Allergy.NSAIDs;

             List<string> ingredients = new List<string>();


            while (true)
            {

               Console.Write("Unesi ime sastojka(ako si završio unesi stop)  - ");
               
                userResponse = Console.ReadLine();

                if (userResponse == "stop")
                    break;

                ingredients.Add(userResponse);

            }

           Medication.addMedicationSuggestion(new Medication(medicationName, timesADay, timeForMedicine, new List<Allergy>() { allergy}, ingredients, ""));

        }


    public void Load()
        {
            string file = "../../../Data/ManagerData.json";
            string json = File.ReadAllText(file);
            Manager m = JsonSerializer.Deserialize<Manager>(json)!;
            username = m.Username;
            password = m.Password;
            hospital = m.Hospital;
            managerRequests = m.managerRequests;
            ExecuteTodayRequests();

        }   

        public void Save()
        {
            ExecuteTodayRequests();

            string file = "../../../Data/ManagerData.json";
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(file, json);
        }
    }
}
