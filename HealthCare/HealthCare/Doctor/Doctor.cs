using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Doctor(string username, string password, Hospital hospital, List<Appointment> doctorAppointmentList)
        {
            this.username = username;
            this.password = password;
            this.ManagerRequestList = doctorAppointmentList;
        }
    }
}
