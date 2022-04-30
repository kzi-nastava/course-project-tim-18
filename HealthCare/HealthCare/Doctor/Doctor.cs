
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthCare.Secretary;

namespace HealthCare.Doctor
{
    class Doctor : User
    {
        private string name;
        private string surname;
        private List<Patient.Appointment>? appointments;

        public Doctor() {
            username = "";
            password = "";
            appointments = new List<Patient.Appointment>();
            name = "";
            surname  = "";
        }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }

        public List<Patient.Appointment> Appointments { get => appointments; set => appointments = value; }
        

        [JsonConstructor]
        public Doctor(string username, string password,string name, string surname,List<Patient.Appointment> appointments)
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
        public void AddAppointment(Patient.Appointment appointment)
        {
            this.appointments.Add(appointment);
        }
        public bool CreateAppointment()
        {
            Console.Write("Unesite korisnicko ime pacijenta:  ");
            string patient = Console.ReadLine();
            if (patient == "") // TODO or check if username exists
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            Console.Write("Unesite datum i vreme pregleda pregleda(format = dd/MM/yyyy HH:mm):  ");
            string period = Console.ReadLine();
            DateTime dt;
            if (period == "" || !DateTime.TryParseExact(period, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            if (checkIfAvailable(DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null))) // TODO regex for datetime
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
            this.appointments.Add(new Patient.Appointment(this.username, patient, period, (AppointmentType)Int32.Parse(type)-1));
            return true;
        }

        private bool checkIfAvailable(DateTime appointmentDate)
        {
            foreach (var a in appointments)
            {
                DateTime dt = DateTime.ParseExact(a.TimeOfAppointment, "dd/MM/yyyy HH:mm", null);
                if ((dt - appointmentDate).TotalMinutes < 15)
                {
                    return false;
                }
            }
            return true;
        }
        public void readAppointments()
        {
            foreach (Patient.Appointment a in appointments)
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

        public void Serialize()
        {
            List<Doctor> doctors = Deserialize();
            for (int i = 0; i < doctors.Count; i++)
            {
                if (doctors[i].username == this.username)
                {
                    doctors[i] = this;
                }
            }
            Serialize(doctors);
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
                    foreach (Patient.Appointment a in d.appointments)
                    {
                        if (a.Patient == patient && a.TimeOfAppointment == date)
                        {
                            d.appointments.Remove(a);
                            Serialize(doctors);
                            return;
                        }
                    }
                }
            }
            
        }
        private void deleteAppointmentMenu(){}

        private void checkScheduleMenu()
        {
            Console.Write("Izaberite datum za prikaz( format dd/MM/yyyy HH:mm) ili 'danas' za danasnji dan: ");
            string choice = Console.ReadLine();
            DateTime dt;
            if (choice == "danas")
            {
                printSchedule(null);
                return;
            }
            if (choice == "" || !DateTime.TryParseExact(choice, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Neodgovarajuc unos");
                return;
            }
            printSchedule(dt);
        }

        private void printSchedule(DateTime? chosenDate)
        {
            if (!chosenDate.HasValue)
            {
                chosenDate = DateTime.Today;
            }
            
            for (int i = 0;i < appointments.Count;i++)
            {
                DateTime dt = DateTime.ParseExact(appointments[i].TimeOfAppointment, "dd/MM/yyyy HH:mm", null);
                if ((dt - chosenDate) < TimeSpan.FromDays(4) && dt > chosenDate)
                {
                    List<Patient.Patient> patients = Patient.Patient.patientDeserialization();
                    Patient.Patient patient = new Patient.Patient();
                    foreach (Patient.Patient p in patients)
                    {
                        if (appointments[i].Patient == p.Username)
                        {
                            patient = p;
                            break;
                        }
                    }
                    Console.WriteLine("Pregled " + i+1 + ". ");
                    Console.Write("Datum pregleda/operacije: ");
                    Console.WriteLine(dt);
                    Console.Write("Pacijent: ");
                    Console.WriteLine(appointments[i].Patient); 
                    Console.WriteLine("Zdravstveni karton pacijenta: ");
                    patient.MedicalRecord.ViewMedicalRecord(patient.MedicalRecord);
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
            Console.WriteLine("===============================================================");
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