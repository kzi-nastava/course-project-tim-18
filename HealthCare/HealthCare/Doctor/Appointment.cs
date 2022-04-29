

using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace HealthCare.Doctor
{
    class Appointment
    {
        private string? doctor;
        private string? patient { get; set; }
        private DateTime dateTime { get; set; }
        private AppointmentType type { get; set; }

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
        [JsonConstructor]
        public Appointment(string doctor, string patient, DateTime dateTime, AppointmentType type)
        {
            this.doctor = doctor;
            this.patient = patient;
            this.dateTime = dateTime;
            this.type = type;
        }

        public override string ToString()
        {
            return String.Format("Appointment( Doctor: {0}, Patient: {1}, Date&Time: {2}, AppointmentType: {3})", doctor, patient, dateTime, type);

        }
    }
}
