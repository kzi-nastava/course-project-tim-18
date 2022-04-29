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

        public override string ToString()
        {
            return TimeOfAppointment + "," + Doctor + ","+ Patient;
        }

        public List<Appointment> appointmentsDeserialization()
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

        public void deletingAppointment(Appointment oldAppointment)
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
