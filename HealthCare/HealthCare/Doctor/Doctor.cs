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
        private string id;
        private List<Appointment> appointments;

        public Doctor() {
            username = "";
            password = "";
            appointments = null;
            id = "";
            Name = "";
            Surname  = "";
        }

        public Doctor(string username, string password,string name, string surname, string id, List<Appointment> appointments)
        {
            this.username = username;
            this.password = password;
            this.Name = name;
            this.Surname = surname;
            this.id = id;
            this.appointments = appointments;
        }
    }
}