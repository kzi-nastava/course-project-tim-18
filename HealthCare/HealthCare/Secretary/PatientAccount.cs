using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Secretary
{
    public class PatientAccount
    {
        private string name;
        private string lastname;
        private int id;
        private string address;
        private string username;
        private string password;
        private string email;

       
        public PatientAccount(string name,string lastname,int id, string address,string username,string password,string email)
        {
            this.name = name;
            this.lastname = lastname;
            this.id = id;
            this.address = address;
            this.username = username;
            this.password = password;
            this.email = email;

        }

        public string Name { 
            get { return name; }
            set { name = value; }
        }
        public string Lastname { 
            get { return lastname; } 
            set { lastname = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }

        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }

        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

    }
}
