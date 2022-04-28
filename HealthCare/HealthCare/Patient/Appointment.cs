using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCare
{
    public class Appointment
    {
        string timeOfAppointment;

        string doctor;

        string patient;
        

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

        public Appointment()
        {
            timeOfAppointment = "";
            doctor = "";
            patient = "";
        }

        [JsonConstructor]
        public Appointment(string timeOfAppointment, string doctor, string patient)
        {
            this.timeOfAppointment = timeOfAppointment;
            this.doctor = doctor;
            this.patient = patient;
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
            DateTime timeWanted = Program.stringToDateTime(timeOfAppointment);
            foreach (Appointment appointment in appointments)
            {
                DateTime timeChecked = Program.stringToDateTime(appointment.TimeOfAppointment);
                TimeSpan timeDifference = timeWanted.Subtract(timeChecked);
                if ((timeDifference.TotalMinutes < 16 && doctor == appointment.Doctor) || (timeDifference.TotalMinutes > -16 && doctor == appointment.Doctor))
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
    }
}


