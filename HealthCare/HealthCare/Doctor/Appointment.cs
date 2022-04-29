

using System.Text.Json.Serialization;

namespace HealthCare.Doctor
{
    class Appointment
    {
        private Doctor? doctor;
        private string patient;
        private DateTime? dateTime;
        private AppointmentType? type;

        public Appointment()
        {
            doctor = null;
            patient = null;
            dateTime = null;
            type = null;
        }

        [JsonConstructor]
        public Appointment(Doctor doctor, string patient, DateTime dateTime, AppointmentType type)
        {
            this.doctor = doctor;
            this.patient = patient;
            this.dateTime = dateTime;
            this.type = type;
        }
    }
}
