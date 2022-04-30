

using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace HealthCare.Doctor
{
    class Appointment
    {
        private string? doctor;
        private string? patient { get; set; }
        private DateTime dateTime;
        private AppointmentType type;

        public string Doctor
        {
            get => doctor;
            set => doctor = value;
        }
        public string Patient
        {
            get => patient;
            set => patient = value;
        }
        public DateTime DateTime
        {
            get => dateTime;
            set => dateTime = value;
        }
        
        public AppointmentType AppointmentType
        {
            get => type;
            set => type = value;
        }


        // [JsonConstructor]
        public Appointment(string doctor, string patient, DateTime dateTime, AppointmentType type)
        {
            this.doctor = doctor;
            this.patient = patient;
            this.dateTime = dateTime;
            this.type = type;
        }
        public Appointment()
        {

        }
        public override string ToString()
        {
            string s;
            if (type == 0)
            {
                s = "Operacija";
            }
            else
            {
                s = "Pregled";
            }
            return String.Format("Termin( Doktor: {0}, Pacijent: {1}, Datum i Vreme: {2}, Tip: {3}", doctor, patient, dateTime, s);

        }
    }
}
