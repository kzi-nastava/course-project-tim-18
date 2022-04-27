using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Patient;

namespace HealthCare.Doctor
{
    class Doctor : User
    {
        public string Name;
        public string Surname;
        private List<Appointment> doctorAppointmentList;

        public Doctor() {
            username = "";
            password = "";
            doctorAppointmentList = null;
        }

        public Doctor(string username, string password, List<Appointment> doctorAppointmentList)
        {
            this.username = username;
            this.password = password;
            this.doctorAppointmentList = doctorAppointmentList;
        }
    }
}
