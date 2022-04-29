
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Doctor
{
    class Doctor : User
    {
        public string Name;
        public string Surname;
        private string id;
        private List<Appointment>? appointments;

        public Doctor() {
            username = "";
            password = "";
            appointments = new List<Appointment>();
            id = "";
            Name = "";
            Surname  = "";
        }

        public string Id { get; set; }

        public List<Appointment> Appointments { get; set; }

        public Doctor(string username, string password,string name, string surname, string id, List<Appointment> appointments)
        {
            this.username = username;
            this.password = password;
            this.Name = name;
            this.Surname = surname;
            this.id = id;
            this.appointments = appointments;
        }
        bool CreateAppointment()
        {
            Console.Write("Unesite  ime i prezime pacijenta:  ");
            string patientFullName = Console.ReadLine();
            if (patientFullName == "") // TODO or check if username exists
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            Console.Write("Unesite datum i vreme pregleda pregleda(format = DD/MM/YYYY hh:mm):  ");
            string dateTime = Console.ReadLine();
            if (dateTime == "" ) // TODO regex for datetime
            {
                Console.WriteLine("Neodgovarajuc unos");
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
            int z = Int32.Parse(type);
            // TODO patient = new Patient(); 
            this.appointments.Add(new Appointment(this, patientFullName, DateTime.ParseExact(dateTime, "dd/MM/yyyy HH:mm", null), (AppointmentType)Int32.Parse(type)-1));
            return true;
        }
    }
    
}
