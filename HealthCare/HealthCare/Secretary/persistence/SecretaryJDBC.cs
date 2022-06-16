using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.Json;

namespace HealthCare.Secretary
{
    public class SecretaryJDBC
    {

        public SecretaryJDBC()
        {

        }


        //DATABASE METHODES-------------------------------------------------
        public static List<SecretaryService> Deserialize()
        {
            string path = "../../../Data/SecretariesData.json";
            string jsonText = File.ReadAllText(path);
            List<SecretaryService> secretaries = JsonSerializer.Deserialize<List<SecretaryService>>(jsonText);
            return secretaries;
        }

        public static void Serialize(List<SecretaryService> secretaries)
        {
            File.WriteAllText("../../../Data/SecretariesData.json", JsonSerializer.Serialize(secretaries));
        }

        public List<Patient.Patient> patientDeserialization()
        {
            string fileName = "../../../Data/Patients.json";
            string PatientFileData = "";
            PatientFileData = File.ReadAllText(fileName);
            string[] Patients = PatientFileData.Split('\n');
            List<Patient.Patient> PatientsList = new List<Patient.Patient>();
            foreach (String s in Patients)
            {
                if (s != "")
                {
                    Patient.Patient? patient = JsonSerializer.Deserialize<Patient.Patient>(s);
                    if (patient != null)
                        PatientsList.Add(patient);

                }
            }

            return PatientsList;
        }

        public void DeleteFromPatients(string username)
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient.Patient> blockedPatients = patientDeserialization();
            string json = "";
            foreach (Patient.Patient blockedPatient in blockedPatients)
            {
                if (blockedPatient.Username != username)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(blockedPatient) + "\n";
                }
            }

            File.WriteAllText(fileName, json);
        }

        public void SerializePatient(MedicalRecord account)
        {
            string fileName = "../../../Data/MedicalRecord.json";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(account) + "\n";
            File.AppendAllText(fileName, jsonString);

        }

        public List<MedicalRecord> MedicalRecordDeserialization()
        {
            string fileName = "../../../Data/MedicalRecord.json";
            string medicalRecordData = "";
            medicalRecordData = File.ReadAllText(fileName);
            string[] medicalRecords = medicalRecordData.Split('\n');
            List<MedicalRecord> medicalRecordList = new List<MedicalRecord>();
            foreach (String s in medicalRecords)
            {
                if (s != "")
                {
                    MedicalRecord? medicalRecord = System.Text.Json.JsonSerializer.Deserialize<MedicalRecord>(s);
                    if (medicalRecord != null)
                        medicalRecordList.Add(medicalRecord);
                }
            }
            return medicalRecordList;

        }

        public void DeleteFromMedicalRecord(string username)
        {
            string fileName = "../../../Data/MedicalRecord.json";
            List<MedicalRecord> medicalRecords = MedicalRecordDeserialization();
            string json = "";
            foreach (MedicalRecord MedicalRecord in medicalRecords)
            {
                if (MedicalRecord.Username != username)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(MedicalRecord) + "\n";
                }
            }
            File.WriteAllText(fileName, json);
        }
        //--------------------------------------------------------------------

    }
}
