
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCare.Doctor
{
    class Doctor : User
    {
        private string name;
        private string surname;
        private List<Appointment>? appointments;

        public Doctor() {
            username = "";
            password = "";
            appointments = new List<Appointment>();
            name = "";
            surname  = "";
        }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }

        public List<Appointment> Appointments { get => appointments; set => appointments = value; }
        

        [JsonConstructor]
        public Doctor(string username, string password,string name, string surname,List<Appointment> appointments)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.appointments = appointments;
        }
    
        public override string ToString()
        {
            return String.Format("Doctor( Name: {0}, Surname: {1}, Username: {2}, Password: {3}, Appointments: [{4}])", name, surname, username, password, String.Join("; ",appointments));
        }
        public void AddAppointment(Appointment appointment)
        {
            this.appointments.Add(appointment);
        }
        public bool CreateAppointment()
        {
            Console.Write("Unesite  ime i prezime pacijenta:  ");
            string patientFullName = Console.ReadLine();
            if (patientFullName == "") // TODO or check if username exists
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            Console.Write("Unesite datum i vreme pregleda pregleda(format = dd/MM/yyyy HH:mm):  ");
            string period = Console.ReadLine();
            if (period == "")
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            if (checkIfAvailable(DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null))) // TODO regex for datetime and check if doctor is available
            {
                Console.WriteLine("Doktor nije dostupan za dat termin.");
                return false;
            }
            Console.WriteLine("Izaberite tip termina:");
            Console.WriteLine("1. Operacija");
            Console.WriteLine("2. Pregled");
            string type = Console.ReadLine();
            if (type != "1" || type != "2")
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            // TODO patient = new Patient(); 
            this.appointments.Add(new Appointment(this.username, patientFullName, DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null), (AppointmentType)Int32.Parse(type)-1));
            return true;
        }

        private bool checkIfAvailable(DateTime date)
        {
            foreach (var a in appointments)
            {
                if ((a.DateTime - date).Minutes > 0  && (a.DateTime - date).Minutes < 15)
                {
                    return false;
                }
            }
            return true;
        }
        public void readAppointments()
        {
            foreach (Appointment a in appointments)
            {
                Console.WriteLine(a);
            }
        }

        public static List<Doctor> Deserialize()
        {
            string path = "../../../Data/DoctorsData.json";
            string jsonText = File.ReadAllText(path);
            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(jsonText);
            return doctors;
        }
        public static void Serialize(List<Doctor> doctors)
        {
            File.WriteAllText("../../../Data/DoctorsData.json", JsonSerializer.Serialize(doctors));
        }
        public static void deleteAppointment(string patient, string doctor, string date)
        {
            List<Doctor> doctors = Deserialize();
            foreach (Doctor d in doctors)
            {
                if (d.username == doctor)
                {
                    foreach (Appointment a in d.appointments)
                    {
                        if (a.Patient == patient)
                        {
                            d.appointments.Remove(a);
                            Serialize(doctors);
                            return;
                        } // and date
                    }
                }
            }
            
        }
        private void deleteAppointmentMenu(){}

        private void checkScheduleMenu()
        {
        }

        public void CheckSchedule(DateTime? chosenDate)
        {
            if (!chosenDate.HasValue)
            {
                chosenDate = DateTime.Today;
            }
            
            for (int i = 0;i < appointments.Count;i++)
            {
                if ((appointments[i].DateTime - chosenDate) < TimeSpan.FromDays(4) && appointments[i].DateTime > chosenDate)
                {
                    Console.WriteLine(i);
                    Console.Write("Datum pregleda/operacije: ");
                    Console.WriteLine(appointments[i].DateTime);
                    Console.WriteLine("Zdravstveni karton pacijenta: ");
                    Console.WriteLine(appointments[i].Patient); // TODO add patient info from class
                }
            }
            
        }

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
            Console.WriteLine("1. CRUD pregled/operaciju");
            Console.WriteLine("2. Prikaz rasporeda");
            Console.WriteLine("3. Izvodjenje pregleda/opracije");
            Console.WriteLine("4. Exit");
            Console.Write("Izaberite opciju: ");

        }
        private void CRUDMenuPrint()
        {
            Console.WriteLine("\n1. Kreiraj pregled/opraciju");
            Console.WriteLine("2. Prikaz pregleda/opracija");
            Console.WriteLine("3. Izmijena pregleda/opracije");
            Console.WriteLine("4. Brisanje pregleda/opraciju");
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
                    this.CreateAppointment();
                    return true;
                case "2":
                    this.readAppointments();
                    return true;
                case "3":
                    // TODO update appointment
                    return true;
                case "4":
                    this.deleteAppointmentMenu();
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
                    checkScheduleMenu();
                    return true;
                case "3":
                    // TODO do an appointment
                    return true;
                case "4":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos!\n");
                    return true;
                    
            }

        }
    }
    
}