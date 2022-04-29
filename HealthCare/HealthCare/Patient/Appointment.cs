using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare
{
    public class Appointment
    {
        private Patient patient;
        public Appointment(Patient patient)
        {
            this.patient = patient;
        }
        public Patient Patient
        {
            get { return patient; }
            set { patient = value; }
        }
        public override string ToString()
        {
            return patient.username + "\\\\" + patient.password;
        }
    }
}
