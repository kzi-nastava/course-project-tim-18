
using System.Globalization;
using System.Text.Json;
using HealthCare.Secretary;

namespace HealthCare.Patient
{
    public class Patient : User
    {
        private MedicalRecord medicalRecord;

        public MedicalRecord MedicalRecord
        {
            get => medicalRecord;
            set => medicalRecord = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Patient(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public Patient()
        {
        }

        public Patient(string username, string password, MedicalRecord medicalRecord)
        {
            this.username = username;
            this.password = password;
            this.medicalRecord = medicalRecord;
        }

        public static List<Patient> patientDeserialization()
        {
            string fileName = "../../../Data/Patients.json";
            string PatientFileData = "";
            PatientFileData = File.ReadAllText(fileName);
            string[] Patients = PatientFileData.Split('\n');
            List<Patient> PatientsList = new List<Patient>();
            foreach (String s in Patients)
            {
                if (s != "")
                {
                    Patient? patient = JsonSerializer.Deserialize<Patient>(s);
                    if (patient != null)
                        PatientsList.Add(patient);

                }
            }
            return PatientsList;
        }

        public void blockingPatient()
        {
            string fileName = "../../../Data/Appointments.json";
            List<Patient> patients = patientDeserialization();
            string json = "";
            foreach (Patient patient in patients)
            {
                if (patient.username != this.username && patient.password != this.password)
                    json += JsonSerializer.Serialize(patient) + "\n";
            }
            File.WriteAllText(fileName, json);
            BlockedPatients blocked = new BlockedPatients(BlockedType.Patient, this);
            blocked.serializeBlockedPatient();
        }

        public void serializePatient()
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient> Patients = patientDeserialization();
            string json = "";
            foreach (Patient Patient in Patients)
            {
                json += JsonSerializer.Serialize(Patient) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }



        static public void Serialize(List<Patient> Patients)
        {
            string fileName = "../../../Data/Patients.json";
            string json = "";
            foreach (Patient Patient in Patients)
            {
                json += JsonSerializer.Serialize(Patient) + "\n";
            }
            File.WriteAllText(fileName, json);
        }

        public void makingAppointment()
        {
            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            AntiTrolCounter counter = new AntiTrolCounter(this.username, dateInString, typeOfChange.Create);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Console.WriteLine("Unesite ime doktora kod koga zelite tretman: ");
                string doctor = Console.ReadLine();
                Console.WriteLine("Unesite vreme tretmana koji zelite da zakazete:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                if (Appointment.isAppointmentValid(timeOfAppointment, doctor) == true)
                {
                    Appointment appointment = new Appointment(timeOfAppointment, doctor, this.username, HealthCare.Doctor.AppointmentType.Examination);
                    appointment.serializeAppointment();
                }
            }
            else
            {
                Console.WriteLine("Prevelik broj zakazivanja novih tretmana vas nalog ce sada biti blokiran: ");
            }
        }

        public void changingAppointment()
        {

            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            AntiTrolCounter counter = new AntiTrolCounter(this.username, dateInString, typeOfChange.Update);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Appointment.printingAppointment();
                Console.WriteLine("Unesite vreme tretmana koji zelite da izmenite:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                Console.WriteLine("Unesite ime doktora kod koga zelite da izmenite tretman: ");
                string doctor = Console.ReadLine();
                List<Appointment> appointments = Appointment.appointmentsDeserialization();
                bool validationOfNewAppointment = false;
                Appointment oldAppointment = new Appointment();
                int indexOfRequest = 0;
                for (int i = 0; i < appointments.Count; i++)
                {
                    if (appointments[i].TimeOfAppointment == timeOfAppointment && appointments[i].Doctor == doctor)
                    {
                        indexOfRequest = i;
                        Console.WriteLine("Unesite broj ispred opcije:\n1 Promena doktora\n2 Promena vremena termina:\nSve drugo za kraj");
                        string option = Console.ReadLine();
                        if (option == "1")
                        {
                            Console.WriteLine("Unesite ime doktora kod koga zelite novi termin: ");
                            string newDoctor = Console.ReadLine();
                            oldAppointment = appointments[i];
                            appointments[i].Doctor = newDoctor;
                            if (Appointment.isAppointmentValid(appointments[i].TimeOfAppointment, appointments[i].Doctor))
                                validationOfNewAppointment = true;
                        }
                        if (option == "2")
                        {
                            Console.WriteLine("Unesite vreme novog termina: ");
                            string newTimeOfAppointment = Console.ReadLine();
                            oldAppointment = appointments[i];
                            appointments[i].TimeOfAppointment = newTimeOfAppointment;
                            if (Appointment.isAppointmentValid(appointments[i].TimeOfAppointment, appointments[i].Doctor))
                            {
                                validationOfNewAppointment = true;
                                timeOfAppointment = newTimeOfAppointment;
                            }
                        }
                    }
                }
                DateTime timeChecked = DateTime.ParseExact(timeOfAppointment, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                TimeSpan timeDifference = timeChecked.Subtract(DateTime.Now);
                if (validationOfNewAppointment == true)
                    if (timeDifference.TotalDays > 2)
                        Appointment.serializingListOfAppointments(appointments);
                    else
                    {
                        AppointmentRequests appointmentRequest = new AppointmentRequests(oldAppointment, appointments[indexOfRequest], typeOfChange.Update);
                        appointmentRequest.serializeAppointmentRequest();
                    }
            }
            else
            {
                this.blockingPatient();
                Console.WriteLine("Prevelik broj promena vas nalog je sada blokiran: ");
            }
        }

        public void deletingAppointment()
        {
            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            AntiTrolCounter counter = new AntiTrolCounter(this.username, dateInString, typeOfChange.Update);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Console.WriteLine("Unesite ime doktora kod koga zelite da izbrisete tretman: ");
                string doctor = Console.ReadLine();
                Console.WriteLine("Unesite vreme tretmana koji zelite da izbrisete:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                Appointment appointment = new Appointment(timeOfAppointment, doctor, this.username);
                DateTime timeChecked = DateTime.ParseExact(timeOfAppointment, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                TimeSpan timeDifference = timeChecked.Subtract(DateTime.Now);
                if(timeDifference.TotalDays > 2)
                    appointment.deletingAppointment();
                else
                {
                    AppointmentRequests appointmentRequest = new AppointmentRequests(appointment, appointment, typeOfChange.Delete);
                    appointmentRequest.serializeAppointmentRequest();
                }

            }
            else
            {
                this.blockingPatient();
                Console.WriteLine("Prevelik broj brisanja vas nalog je sada blokiran: ");
            }
        }

        public void DeleteFromPatients(string username)
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient> blockedPatients = patientDeserialization();
            string json = "";
            foreach (Patient blockedPatient in blockedPatients)
            {
                if (blockedPatient.username != username)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(blockedPatient) + "\n";
                }
            }
            File.WriteAllText(fileName, json);
        }

        public void patientMenu()
        {
            string option;
            while (true)
            {

                Console.WriteLine("Izaberite opciju koju zelite da izaberete:\n1 Zakazivanje termina\n2 Izmena termina\n3 Brisanje termina\n4 Prikaz termina\n5 Izalazak iz menua");
                option = Console.ReadLine();

                if (option == "5")
                    break;

                if (option == "1")
                    this.makingAppointment();
                else if (option == "2")
                    this.changingAppointment();
                else if (option == "3")
                    this.deletingAppointment();
                else if (option == "4")
                    Appointment.printingAppointment();
            }
        }
    }
}
