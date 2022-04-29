using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
    public class Patient : User
    {

        public Patient()
        {
        }
        public Patient(string username,string password)
        {
            this.username = username;
            this.password = password;
        }
        public string username { get; set; }
        public string password { get; set; }
        public override string ToString()
        {
            return username + "," + password;
        }

        public List<Patient> PatientDeserialization()
        {
            string fileName = "../../../Data/Patient.json";
            string PatientFileData = "";
            PatientFileData = File.ReadAllText(fileName);
            string[] Patients = PatientFileData.Split('\n');
            List<Patient> PatientsList = new List<Patient>();
            foreach (String s in Patients)
            {
                if (s != "")
                {
                    Patient? Patient = JsonSerializer.Deserialize<Patient>(s);
                    if (Patient != null)
                        PatientsList.Add(Patient);

                }
            }
            return PatientsList;
        }

        public void SerializePatient()
        {
            string fileName = "../../../Data/Patient.json";
            List<Patient> Patients = PatientDeserialization();
            string json = "";
            foreach (Patient Patient in Patients)
            {
                json += JsonSerializer.Serialize(Patient) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }

        public void DeleteFromPatients(string username)
        {
            string fileName = "../../../Data/Patient.json";
            List<Patient> blockedPatients =PatientDeserialization();
            string json = "";
            foreach (Patient blockedPatient in blockedPatients)
            {
                if (blockedPatient.username != username)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(blockedPatient) + "\n";
                }
            }
            File.WriteAllText(fileName, json);
        }
        
    }
}
