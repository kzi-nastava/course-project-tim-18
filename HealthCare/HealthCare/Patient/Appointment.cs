
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace HealthCare.Patient
{
    public class Appointment
    {
        private string timeOfAppointment;
        private string doctor;
        private string patient;
        private Doctor.AppointmentType appointmentType;
        private string roomId;
        

        public string TimeOfAppointment
        {
            get => timeOfAppointment;
            set => timeOfAppointment = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Doctor
        {
            get => doctor;
            set => doctor = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Patient
        {
            get => patient;
            set => patient = value ?? throw new ArgumentNullException(nameof(value));
        }

        public HealthCare.Doctor.AppointmentType AppointmentType
        {
            get => appointmentType;
            set => appointmentType = value;
        }
        
        public string RoomId
        {
            get => roomId;
            set => roomId = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Appointment()
        {
            timeOfAppointment = "";
            doctor = "";
            patient = "";
            appointmentType = HealthCare.Doctor.AppointmentType.Examination;
        }
        

        [JsonConstructor]
        public Appointment(string timeOfAppointment, string doctor, string patient, HealthCare.Doctor.AppointmentType appointmentType)
        {
            this.timeOfAppointment = timeOfAppointment;
            this.doctor = doctor;
            this.patient = patient;
            this.appointmentType = appointmentType;
            this.roomId = HealthCare.Doctor.Doctor.DoctorsRoom(doctor);
        }

        public Appointment(string timeOfAppointment, string doctor, string patient)
        {
            this.timeOfAppointment = timeOfAppointment;
            this.doctor = doctor;
            this.patient = patient;
            this.appointmentType = HealthCare.Doctor.AppointmentType.Examination;
            this.roomId = HealthCare.Doctor.Doctor.DoctorsRoom(doctor);
        }
       

        public static DateTime stringToDateTime(string date)
        {
            string[] splitToDateAndTime = date.Split(" ");
            string[] dateAsArray = splitToDateAndTime[0].Split('/');
            string[] timeAsArray = splitToDateAndTime[1].Split(':');
            int day = int.Parse(dateAsArray[0]);
            int month = int.Parse(dateAsArray[1]);
            int year = int.Parse(dateAsArray[2]);
            int hour = int.Parse(timeAsArray[0]);
            int minute = int.Parse(timeAsArray[1]);
            DateTime dateTime = new DateTime(year, month, day);
            dateTime = dateTime.AddHours(hour);
            dateTime = dateTime.AddMinutes(minute);
            //Console.WriteLine(dateTime.ToString("dd/MM/yyyy HH:mm"));
            return dateTime;
        }

        public static List<Appointment> appointmentsDeserialization()
        {
            string fileName = "../../../Data/Appointments.json";
            string appointmentFileData = "";
            appointmentFileData = File.ReadAllText(fileName);
            string[] appointments = appointmentFileData.Split('\n');
            List<Appointment> appointmentsList = new List<Appointment>();
            foreach (String s in appointments)
            {
                if (s != "")
                {
                    Appointment? appointment = JsonSerializer.Deserialize<Appointment>(s);
                    if (appointment != null)
                        appointmentsList.Add(appointment);

                }
            }
            return appointmentsList;
        }

        public void serializeAppointment()
        {
            string fileName = "../../../Data/Appointments.json";
            List<Appointment> appointments = appointmentsDeserialization();
            string json = "";
            foreach (Appointment appointment in appointments)
            {
                json += JsonSerializer.Serialize(appointment) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }
    
        public static void printingAppointment()
        {
            string fileName = "../../../Data/Appointments.json";
            string appointmentFileData = "";
            appointmentFileData = File.ReadAllText(fileName);
            Console.WriteLine(appointmentFileData);
        }

        public void deletingAppointment()
        {
            string fileName = "../../../Data/Appointments.json";
            List<Appointment> appointments = appointmentsDeserialization();
            string json = "";
            foreach (Appointment appointment in appointments)
            {
                if(appointment.Doctor != this.Doctor && appointment.TimeOfAppointment != this.TimeOfAppointment)
                    json += JsonSerializer.Serialize(appointment) + "\n";
                else
                {
                    HealthCare.Doctor.Doctor.deleteAppointment(this.patient, this.doctor, this.TimeOfAppointment);
                }
            }
            File.WriteAllText(fileName, json);
        }

        public static bool isAppointmentValid(string timeOfAppointment, string doctor)
        {
            List<Appointment> appointments = Appointment.appointmentsDeserialization();
            DateTime timeWanted = stringToDateTime(timeOfAppointment);
            foreach (Appointment appointment in appointments)
            {
                DateTime timeChecked = stringToDateTime(appointment.timeOfAppointment);
                TimeSpan timeDifference = timeWanted.Subtract(timeChecked);
                if ((-16 < timeDifference.TotalMinutes && timeDifference.TotalMinutes < 16) && doctor == appointment.Doctor)
                    return false;
            }
            return true;
        }

        public static void serializingListOfAppointments(List<Appointment> listOfAppointments)
        {
            string fileName = "../../../Data/Appointments.json";
            string json = "";
            foreach (Appointment appointment in listOfAppointments)
            {
                json += JsonSerializer.Serialize(appointment) + "\n";
            }

            File.WriteAllText(fileName, json);
        }

        public void deletingAppointmentFromSecretary(Appointment oldAppointment)
        {
            string fileName = "../../../Data/Appointments.json";
            List<Appointment> appointments = appointmentsDeserialization();
            string json = "";
            foreach (Appointment appointment in appointments)
            {
                if (appointment.Doctor != oldAppointment.Doctor && appointment.TimeOfAppointment != oldAppointment.TimeOfAppointment)
                    json += JsonSerializer.Serialize(appointment) + "\n";
                else
                {
                    HealthCare.Doctor.Doctor.deleteAppointment(oldAppointment.patient, oldAppointment.doctor, oldAppointment.TimeOfAppointment);
                }
            }
            File.WriteAllText(fileName, json);
        }
        public override string ToString()
        {
            string s;
            if (appointmentType == 0)
            {
                s = "Operacija";
            }
            else
            {
                s = "Pregled";
            }
            return String.Format("Termin( Doktor: {0}, Pacijent: {1}, Datum i Vreme: {2}, Tip: {3}", doctor, patient, timeOfAppointment, s);

        }
        
        
    }
}


