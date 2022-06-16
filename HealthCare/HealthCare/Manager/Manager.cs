using System.Text.Json;
using System.Text.Json.Serialization;
using HealthCare.Doctor;
using HealthCare.Doctor.PrescribeMedication;
using HealthCare.Patient;

namespace HealthCare
{
    public class Manager : User
    {
        private Hospital? hospital;
        private List<ManagerRequest> managerRequests = new List<ManagerRequest>();
        private List<RenovationRequest> renovationRequest = new List<RenovationRequest>();
        Room room;


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
            Console.WriteLine("9. Ponvno predlaganje odbijenih lekova");
            Console.WriteLine("10. Pregled rezultata anketa");
            Console.WriteLine("11. Exit");
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
                    SuggestDeclinedMedication();
                    return true;
                case "10":
                    ViewSurveyResults();
                    return true;
                case "11":
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
            if (amountInt > equipment.Amount)
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

                if (appointment.RoomId != roomNameFirst && appointment.RoomId != roomNameSecond)
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




            Medication.addMedicationSuggestion(new Medication(medicationName, timesADay, timeForMedicine, new List<Allergy>() { allergy }, ingredients, ""));

        }

        void SuggestDeclinedMedication()
        {
            List<Medication> declinedMedications = Medication.DeserializeDenials();

            for (int i = 0; i < declinedMedications.Count; i++)
            {

                Console.WriteLine((i + 1).ToString() + ". " + declinedMedications[i].Name);

            }


            Console.Write("Unesi redni broj leka koji revidiraš  - ");
            int index;


            string indexString = Console.ReadLine();
            if (Int32.TryParse(indexString, out index) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }

            if (index <= 0)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return;
            }


            string medicationName = declinedMedications[index - 1].Name;

            declinedMedications.Remove(declinedMedications[index]);

            Medication.SerializeDenials(declinedMedications);


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




            Medication.addMedicationSuggestion(new Medication(medicationName, timesADay, timeForMedicine, new List<Allergy>() { allergy }, ingredients, ""));

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

        //EQUIPMENT REQUESTS----------------------------------------------------------
        public string Input(string text)
        {
            Console.Write(text);
            string option = Console.ReadLine();
            return option;
        }

        public void DynamicEquipmentRequests()
        {
            List<Equipment> equipmentList = new List<Equipment>();
            List<Equipment> oldEquipmentList = new List<Equipment>();
            foreach (Room room in hospital.Rooms)
            {
                foreach (Equipment equipment in room.EquipmentList)
                {
                    if (room.Name == "Magacin" && equipment.Amount == 0)
                    {
                        PrintEquipmentView(equipment);
                        equipmentList.Add(equipment);

                    }
                    oldEquipmentList.Add(equipment);
                }
            }

            string newEqupment = Input("\nUnesite opremu: ");
            string inputQuantity =Input("Unesite kolicinu: ");

            CheckSentRequest(newEqupment, inputQuantity, equipmentList);

        }

        private void CheckSentRequest(string newEqupment, string inputQuantity, List<Equipment> equipmentList)
        {
            bool isTrue = false;
            EquipmentRequest equipmentRequests = new EquipmentRequest();

            List<EquipmentRequest> equipmentRequestList = equipmentRequests.EquipmentRequestDeserialization();
            foreach (EquipmentRequest equipmentRequest in equipmentRequestList)
            {
                if (equipmentRequest.Equipment.Name == newEqupment)
                {
                    isTrue = true;
                }

            }
            if (isTrue == true)
            {
                Console.WriteLine("\nZahtjev je vec poslat!");
                CheckHours(equipmentList);

            }
            if (isTrue == false)
            {
                SendRequest(equipmentList, room, newEqupment, inputQuantity);
                CheckHours(equipmentList);
            }
        }

        private void PrintEquipmentView(Equipment equipment)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Naziv opreme: " + equipment.Name);
            Console.WriteLine("Količina opreme: " + equipment.Amount);
            Console.WriteLine("------------------------------------");
        }

        public void SendRequest(List<Equipment> equipment, Room room, string newEquipment, string inputQuantity)
        {
            int newQuantity = Int32.Parse(inputQuantity);

            foreach (Equipment equipmentt in equipment)
            {
                if (equipmentt.Name == newEquipment)
                {
                    Equipment equipment1 = new Equipment(equipmentt.EquipmentType, equipmentt.Name, newQuantity);
                    EquipmentRequest equipmentRequest = new EquipmentRequest(equipment1, DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                    equipmentRequest.EquipmentRequestSerialization();
                }
            }
        }

        public void CheckHours(List<Equipment> equipment)
        {
            EquipmentRequest equipmentRequests = new EquipmentRequest();

            List<EquipmentRequest> equipmentRequestList = equipmentRequests.EquipmentRequestDeserialization();
            foreach (EquipmentRequest equipmentRequest in equipmentRequestList)
            {
                var parsedDate = DateTime.Parse(equipmentRequest.ExecutionDate);
                var parsedDateNow = DateTime.Parse(DateTime.Now.ToString());

                DateTime parsedDateAfterOneDay = parsedDate.AddDays(1);
                PrintDateTimeOfRequest(equipmentRequest, parsedDateNow, parsedDateAfterOneDay);

                int res = DateTime.Compare(parsedDateNow, parsedDateAfterOneDay);
                if (res == -1)
                {
                    PrintTimeMessage("Nije proslo ");
                    
                }
                if (res == 1)
                {
                    PrintTimeMessage("Proslo je  ");
                 
                    foreach (Equipment equipment1 in equipment)
                    {
                        if (equipment1.Name == equipmentRequest.Equipment.Name)
                        {
                            ReplaceEquipmentAmountValues(equipment1, equipmentRequest);
                        }
                    }
                }
            }
        }

        private void PrintTimeMessage(string v)
        {
            Console.WriteLine(v + "24h od podnesenog zahtjeva");
            Console.WriteLine("=========================================");
        }

        private void ReplaceEquipmentAmountValues(Equipment equipment1, EquipmentRequest equipmentRequest)
        {
            equipment1.Amount = equipmentRequest.Equipment.Amount;
            Room magacin = GetRoom("Magacin");
            Room newMagacin = new Room(magacin.RoomType, magacin.Name);
            Equipment eq = magacin.GetEquipment(equipment1.Name);
            int i = 0;
            i += equipmentRequest.Equipment.Amount;

            DeleteEquipmentRequest(equipmentRequest.Equipment.Name);
        }

        private void PrintDateTimeOfRequest(EquipmentRequest equipmentRequest, DateTime parsedDateNow, DateTime parsedDateAfterOneDay)
        {
            Console.WriteLine("\n=========================================");
            Console.WriteLine("Zahtjev: " + equipmentRequest.Equipment.Name);
            Console.WriteLine("TRENUTNO VRIJEME: " + parsedDateNow.ToString());
            Console.WriteLine("DATUM ZAHTJEVA: " + parsedDateAfterOneDay);
            Console.WriteLine("=========================================");

        }

        public void DeleteEquipmentRequest(string name)
        {
            EquipmentRequest equipmentRequests = new EquipmentRequest();
            equipmentRequests.DeleteFromEquipmentRequest(name);

        }
        //----------------------------------------------------------------------------

        //EQUIPMENT DISTRIBUTION------------------------------------------------------
        public void DynamicEquipmentDistribution()
        {
            string[] dynamicEquipment = { "Kopce", "Zavoj", "Inekcije", "Papir", "Gaze", "Olovke", "Hanzaplast" };

            List<string> dynamicEquipmentList = new List<string>(dynamicEquipment);
            List<Equipment> equipmentList = new List<Equipment>();
            foreach (Room room in hospital.Rooms)
            {
                Console.WriteLine("============================");
                Console.WriteLine(room.Name);
                Console.WriteLine("============================\n");
                CheckUnavailableEquipment(dynamicEquipmentList, room);

            }

            MakeDistribution();

        }

        private void CheckUnavailableEquipment(List<string> dynamicEquipmentList, Room room)
        {
            foreach (String dm in dynamicEquipmentList)
            {
                bool isFound = false;
                foreach (Equipment equipment in room.EquipmentList)
                {
                    if (equipment.Name == dm)
                    {
                        if (equipment.Amount < 5 && equipment.Amount != 0)
                        {
                            Console.WriteLine(equipment.Name + ": " + equipment.Amount);
                        }
                        if (equipment.Amount == 0)
                        {
                            isFound = true;
                        }
                    }
                }
                if (isFound == true)
                {
                    Console.WriteLine(dm + ": Nema na stanju");
                }
            }
        }

        private void MakeDistribution()
        {   
            string oldRoomName =Input("Unesi naziv prostorije iz koje se uzima oprema: ");
            if (RoomExist(oldRoomName) == false)
            {
                Console.WriteLine("Unijeta neispravna vrijednost.");
                return;
            }

            Room oldRoom = GetRoom(oldRoomName);
            string equipmentName = Input("Unesi naziv opreme: ");

            if (oldRoom.EquipmentExist(equipmentName) == false)
            {
                Console.WriteLine("Unijeta neispravna vrijednost.");
                return;
            }
            Equipment oldEquipment = oldRoom.GetEquipment(equipmentName);

            
            int amountInt;
            string amountString = Input("Unesi kolicinu: ");
            if (Int32.TryParse(amountString, out amountInt) == false)
            {
                Console.WriteLine("Unijeta neispravna vrijednost.");
                return;
            }
            if (amountInt > oldEquipment.Amount)
            {
                Console.WriteLine("Unijeta prevelika velicina.");
                return;
            }

            
            string newRoomName =Input("Unesi naziv prostorije u koju se oprema premijesta: ");
            if (RoomExist(newRoomName) == false)
            {
                Console.WriteLine("Unijeta neispravna vrijednost.");
                return;
            }

            ReplaceAmountBetweenTwoRooms(newRoomName, amountInt, oldEquipment, equipmentName);


        }

        private void ReplaceAmountBetweenTwoRooms(string newRoomName, int amountInt, Equipment oldEquipment, string equipmentName)
        {
            Room newRoom = GetRoom(newRoomName);
            Equipment newEquipment = newRoom.GetEquipment(equipmentName);
            newEquipment.Amount += amountInt;
            oldEquipment.Amount -= amountInt;
        }
        //----------------------------------------------------------------------------
        
        private void ViewSurveyResults()
        {
            List<DoctorsGrade> doctorsGrades = DoctorsGrade.DeserializeDoctorsGrade();
            List<HospitalGrade> hospitalGrades = HospitalGrade.DeserializeHospitalGrade();



            string userResponse = "";

            while (userResponse != "1" && userResponse != "2")
            {
                Console.WriteLine("1.Pregled anketa za bolnicu");
                Console.WriteLine("2. Pregled anketa za doktora");
                Console.WriteLine("Unesi opciju: ");
                userResponse = Console.ReadLine();
            }


            if (userResponse == "1")
            {

                double averageWouldYouSuggest = 0;
                int[] countWouldYouSuggest = new int[5];

                double averageHowCleanIs = 0;
                int[] countHowCleanIs = new int[5];

                double averageHowGoodHospital = 0;
                int[] countHowGoodHospital = new int[5];

                double averageHowSatisfiedAreYou = 0;
                int[] countHowSatisfiedAreYou = new int[5];


                int i = 1;
                Console.WriteLine();
                Console.WriteLine("KOMENTARI");
                Console.WriteLine("--------------------------------------------------");
                foreach (HospitalGrade hospitalGrade in hospitalGrades)
                {
                    Console.WriteLine(i.ToString() + ". Komentar - " + hospitalGrade.Comment);
                    i++;

                    averageHowCleanIs += hospitalGrade.HowCleanItIs;
                    countHowCleanIs[hospitalGrade.HowCleanItIs - 1]++;

                    averageWouldYouSuggest += hospitalGrade.WouldYouSuggest;
                    countWouldYouSuggest[hospitalGrade.WouldYouSuggest - 1]++;

                    averageHowSatisfiedAreYou += hospitalGrade.HowSatisfiedAreYou;
                    countHowSatisfiedAreYou[hospitalGrade.HowSatisfiedAreYou - 1]++;

                    averageHowGoodHospital += hospitalGrade.HowGoodHospitalIs;
                    countHowGoodHospital[hospitalGrade.HowGoodHospitalIs - 1]++;

                }
                averageWouldYouSuggest /= hospitalGrades.Count;
                averageHowCleanIs /= hospitalGrades.Count;
                averageHowGoodHospital /= hospitalGrades.Count;
                averageHowSatisfiedAreYou /= hospitalGrades.Count;

                Console.WriteLine("OCENE");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Prosecna ocene");
                Console.WriteLine("Da li bi preporucili: " + averageWouldYouSuggest);
                Console.WriteLine("Koliko Vam se svidja bolnica: " + averageHowGoodHospital);
                Console.WriteLine("Koliko je cista bolnica: " + averageHowCleanIs);
                Console.WriteLine("Koliko ste zadovoljni: " + averageHowSatisfiedAreYou);

                for (i = 0; i < 5; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine("Ocena " + (i + 1));
                    Console.WriteLine("Da li bi preporucili: " + countWouldYouSuggest[i]);
                    Console.WriteLine("Koliko Vam se svidja bolnica: " + countHowGoodHospital[i]);
                    Console.WriteLine("Koliko je cista bolnica: " + countHowCleanIs[i]);
                    Console.WriteLine("Koliko ste zadovoljni: " + countHowSatisfiedAreYou[i]);

                    Console.WriteLine();
                }


            }
            else
            {

                Dictionary<string, List<DoctorsGrade>> counter = new Dictionary<string, List<DoctorsGrade>>();


                foreach (DoctorsGrade doctorsGrade in doctorsGrades)
                {
                    if (counter.ContainsKey(doctorsGrade.Doctor))
                    {
                        counter[doctorsGrade.Doctor].Add(doctorsGrade);
                    }
                    else
                    {
                        counter[doctorsGrade.Doctor] = new List<DoctorsGrade> { doctorsGrade };
                    }
                }

                Dictionary<string, double> sortedDoctors = new Dictionary<string, double>();

                foreach (var item in counter)
                {
                    Console.WriteLine();
                    Console.WriteLine("Doktor - " + item.Key);
                    List<DoctorsGrade> grades = item.Value;

                    double averageWouldYouSuggest = 0;
                    int[] countWouldYouSuggest = new int[5];

                    double averageHowGoodIsDoctor = 0;
                    int[] countHowGoodIsDoctor = new int[5];


                    int i = 1;
                    Console.WriteLine("------------------------------------------------");

                    foreach (DoctorsGrade doctorsGrade in grades)
                    {
                        Console.WriteLine(i.ToString() + ". Komentar - " + doctorsGrade.Comment);
                        i++;


                        averageWouldYouSuggest += doctorsGrade.WouldYouSuggest;
                        countWouldYouSuggest[doctorsGrade.WouldYouSuggest - 1]++;

                        averageHowGoodIsDoctor += doctorsGrade.HowGoodDoctorWas;
                        countHowGoodIsDoctor[doctorsGrade.HowGoodDoctorWas - 1]++;

                    }
                    averageWouldYouSuggest /= grades.Count;
                    averageHowGoodIsDoctor /= grades.Count;

                    Console.WriteLine();
                    Console.WriteLine("Prosecna ocene");
                    Console.WriteLine("Da li bi preporucili: " + averageWouldYouSuggest);
                    Console.WriteLine("Koliko ste zadovoljni doktorom: " + averageHowGoodIsDoctor);
                    Console.WriteLine();

                    sortedDoctors[item.Key] = averageHowGoodIsDoctor;

                    for (i = 0; i < 5; i++)
                    {
                        Console.WriteLine("Ocena " + (i + 1));
                        Console.WriteLine("Da li bi preporucili: " + countWouldYouSuggest[i]);
                        Console.WriteLine("Koliko ste zadovoljni doktorom: " + countHowGoodIsDoctor[i]);
                        Console.WriteLine();
                    }

                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine();


                }
                Console.WriteLine("Da li hoces ispis tri najbolja ocenjena lekara? (da/ne)");
                userResponse = Console.ReadLine();

                if (userResponse == "da")
                {
                    sortedDoctors = sortedDoctors.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    int i = 0;
                    foreach (var item in sortedDoctors)
                    {
                        if (i >= 2)
                            break;

                        Console.WriteLine((i + 1).ToString() + ". " + item.Key + " - " + item.Value);

                        i++;
                    }

                }



                Console.WriteLine("Da li hoces ispis tri najgore ocenjena lekara? (da/ne)");
                userResponse = Console.ReadLine();

                if (userResponse == "da")
                {
                    sortedDoctors = sortedDoctors.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    int i = 0;
                    foreach (var item in sortedDoctors)
                    {
                        if (i >= 2)
                            break;

                        Console.WriteLine((i + 1).ToString() + ". " + item.Key + " - " + item.Value);

                        i++;
                    }

                }


            }



        }
    }
}
