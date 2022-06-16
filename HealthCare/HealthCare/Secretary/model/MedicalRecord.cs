using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Secretary
{
    public class MedicalRecord
    {
        private string name;
        private string lastname;
        private string address;
        private string username;
        private string password;
        private string email;
        private string height;
        private string weight;
        private string bloodType;
        private string doktor;
        private List<Doctor.PrescribeMedication.Allergy> allergies;


        public MedicalRecord(string name, string lastname, string address, string username, string password, string email, string height, string weight, string bloodType)
        {
            this.name = name;
            this.lastname = lastname;
            this.address = address;
            this.username = username;
            this.password = password;
            this.email = email;
            this.height = height;
            this.weight = weight;
            this.bloodType = bloodType;

        }
        public MedicalRecord(string name, string lastname, string address, string username, string password, string email, string height, string weight, string bloodType, string doktor)
        {
            this.name = name;
            this.lastname = lastname;
            this.address = address;
            this.username = username;
            this.password = password;
            this.email = email;
            this.height = height;
            this.weight = weight;
            this.bloodType = bloodType;
            this.doktor = doktor;

        }

        public MedicalRecord()
        {

        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Lastname
        {
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

        public string Height
        {
            get { return height; }
            set { height = value; }
        }

        public string Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public string BloodType
        {
            get { return bloodType; }
            set { bloodType = value; }
        }

        public string Doktor
        {
            get { return doktor; }
            set { doktor = value; }
        }

        public List<Doctor.PrescribeMedication.Allergy> Allergies
        {
            get { return allergies; }
            set { allergies = value; }
        }

        public override string ToString()
        {
            return "Ime: " + Name + "\nPrezime: " + Lastname + "\nAdresa: " + Address + "\nKorisnicko ime:" + Username + "\nLozinka: " + Password + "\nEmail: " + Email + "\nVisina: " + Height + "\nTezina: " + Weight + "\nKrvna grupa: " + BloodType + "\n";
        }



    }
}
