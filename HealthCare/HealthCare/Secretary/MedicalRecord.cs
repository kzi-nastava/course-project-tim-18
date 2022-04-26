using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare
{
    public class MedicalRecord
    {
        private string name;
        private string lastname;
        private string address;
        private string username;
        private string password;
        private string email;
        private string id;


        public MedicalRecord(string name, string lastname, string address, string username, string password, string email, string id)
        {
            this.name = name;
            this.lastname = lastname;
            this.address = address;
            this.username = username;
            this.password = password;
            this.email = email;
            this.id = id;

        }

        public string Name {
            get { return name; }
            set { name = value; }
        }
        public string Lastname {
            get { return lastname; }
            set { lastname = value; }
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

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string ToString()
        {
            return  "Ime: " + Name + "\nPrezime: " + Lastname + "\nAdresa: " + Address + "\nKorisnicko ime:" + Username + "\nLozinka: " + Password + "\nEmail: " + Email + "\nId: " + Id;
        }
    }
}
