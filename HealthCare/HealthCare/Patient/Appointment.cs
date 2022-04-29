using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace HealthCare.Patient
{
    public class Appointment
    {
        string timeOfAppointment;

        string doctor;

        string patient;

        Doctor.AppointmentType appointmentType;
        

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
        }

        public Appointment(string timeOfAppointment, string doctor, string patient)
        {
            this.timeOfAppointment = timeOfAppointment;
            this.doctor = doctor;
            this.patient = patient;
            this.appointmentType = HealthCare.Doctor.AppointmentType.Examination;
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
            }
            File.WriteAllText(fileName, json);
        }

        public static bool isAppointmentValid(string timeOfAppointment, string doctor)
        {
            List<Appointment> appointments = Appointment.appointmentsDeserialization();
            DateTime timeWanted = DateTime.ParseExact(timeOfAppointment, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            foreach (Appointment appointment in appointments)
            {
                DateTime timeChecked = DateTime.ParseExact(appointment.TimeOfAppointment, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
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
            }
            File.WriteAllText(fileName, json);
        }
    }
}


