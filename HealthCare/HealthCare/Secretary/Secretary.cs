using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace HealthCare
{
    public class Secretary : User
    {
        public Secretary()
        {

        }
        public Secretary(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string username { get; set; }
        public string password { get; set; }

        string medicalRecordFile = "../../../Data/MedicalRecord.json";
        string blockedPatientsFile = "../../../Data/BlockedPatients.json";
        string patientFile = "../../../Data/Patient.json";
        string appointmentFile = "../../../Data/Appointment.json";

        MedicalRecord medRecord = new MedicalRecord();

        //PATIENTS--------------------------------------------------------------------
        public void WriteToPatients(Patient patient, string fileName)
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(patient) + "\n";
            File.AppendAllText(fileName, jsonString);

        }
        public void DeleteFromPatients(string username, string fileName)
        {
            List<Patient> blockedPatients = ReadFromPatients(fileName);
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
        public List<Patient> ReadFromPatients(string fileName)
        {
            string blockedPatientsData = "";
            blockedPatientsData = File.ReadAllText(fileName);
            string[] blockedPatients = blockedPatientsData.Split('\n');
            List<Patient> blockedPatientsList = new List<Patient>();
            foreach (String s in blockedPatients)
            {
                if (s != "")
                {
                    Patient? blockedPatient = System.Text.Json.JsonSerializer.Deserialize<Patient>(s);
                    if (blockedPatient != null)
                        blockedPatientsList.Add(blockedPatient);
                }
            }
            return blockedPatientsList;

        }
        //----------------------------------------------------------------------------


        //BLOCKED PATIENTS------------------------------------------------------------
        public void WriteToBlockedPatients(BlockedPatients blockedPatients, string fileName)
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(blockedPatients) + "\n";
            File.AppendAllText(fileName, jsonString);

        }
        public void DeleteFromBlockedPatients(string username, string fileName)
        {
            List<BlockedPatients> blockedPatients = ReadFromBlockedPatients(fileName);
            string json = "";
            foreach (BlockedPatients blockedPatient in blockedPatients)
            {
                if (blockedPatient.Patient.username != username)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(blockedPatient) + "\n";
                }
            }
            File.WriteAllText(fileName, json);
        }
        public List<BlockedPatients> ReadFromBlockedPatients(string fileName)
        {
            string blockedPatientsData = "";
            blockedPatientsData = File.ReadAllText(fileName);
            string[] blockedPatients = blockedPatientsData.Split('\n');
            List<BlockedPatients> blockedPatientsList = new List<BlockedPatients>();
            foreach (String s in blockedPatients)
            {
                if (s != "")
                {
                    BlockedPatients? blockedPatient = System.Text.Json.JsonSerializer.Deserialize<BlockedPatients>(s);
                    if (blockedPatient != null)
                        blockedPatientsList.Add(blockedPatient);
                }
            }
            return blockedPatientsList;

        }
        //----------------------------------------------------------------------------

        //APPOINTMENT-----------------------------------------------------------------
        public List<Appointment> ReadFromAppointment(string fileName)
        {
            string appointmentData = "";
            appointmentData = File.ReadAllText(fileName);
            string[] appointments = appointmentData.Split('\n');
            List<Appointment> appointmentList = new List<Appointment>();
            foreach (String s in appointments)
            {
                if (s != "")
                {
                    Appointment? appointment = System.Text.Json.JsonSerializer.Deserialize<Appointment>(s);
                    if (appointment != null)
                        appointmentList.Add(appointment);
                }
            }
            return appointmentList;

        }

        //----------------------------------------------------------------------------


        public string InputUsername()
        {
            Console.Write("\nUnesite korisnicko ime pacijenta: ");
            string username = Console.ReadLine();
            return username;
        }

        //CRUD------------------------------------------------------------------------
        public void CreatePatient()
        {
            MedicalRecord record = medRecord.CreateInput();
            medRecord.MedicalRecordView(record);
            medRecord.JsonWriteFile(record, medicalRecordFile);

            //KREIRANJEM NALOGA KREIRA SE I PACIJENT?
            Patient patient = new Patient(record.Username, record.Password);
            WriteToPatients(patient, patientFile);
        }

        public void ReadPatient()
        {
            string username = InputUsername();
            string account = "";
            List<MedicalRecord> medicalRecordlist = medRecord.JsonReadFile(medicalRecordFile);
            List<Patient> patientList = ReadFromPatients(patientFile);
            foreach (MedicalRecord record in medicalRecordlist)
                foreach(Patient patient in patientList)
                    {
                        if (record.Username == username && patient.username == username)
                        {
                        //Console.WriteLine(record);
                            medRecord.PrintMedicalRecord(record);
                            Console.Write("Da li zelite blokirati ovaj nalog? ");
                            string blocked = Console.ReadLine();
                            if (blocked == "da")
                            {
                                BlockingPatientAccount(record.Username,record.Password);
                            }
                        } 
                    }
        }

        public void DeletePatient()
        {
            string username = InputUsername();
            List<Patient> patientList = ReadFromPatients(patientFile);
            foreach (Patient patient in patientList)
            {
                if(patient.username == username)
                    medRecord.JsonDeleteFromFile(username, medicalRecordFile);
                    DeleteFromPatients(username, patientFile);

            }
            
        }

        public void UpdatePatient()
        {
            DeletePatient();
            CreatePatient();
        }
        //----------------------------------------------------------------------------


        //BLOCKING--------------------------------------------------------------------
        public void BlockingPatientAccount(string username,string password)
        {
            DeleteFromPatients(username, patientFile);
            
            Patient patient = new Patient(username,password);
            BlockedPatients blockedPatient = new BlockedPatients(BlockedType.Secretary,patient);
            WriteToBlockedPatients(blockedPatient, blockedPatientsFile);
        }

        public void UnblockingPatientsAccount()
        {
            List<BlockedPatients> blockedPatientslist = ReadFromBlockedPatients(blockedPatientsFile);
            List<MedicalRecord> medicalRecordList = medRecord.JsonReadFile(medicalRecordFile);
            foreach (BlockedPatients blockedPatient in blockedPatientslist)
                foreach(MedicalRecord medicalRecord in medicalRecordList)
                    {
                       if(blockedPatient.Patient.username == medicalRecord.Username)
                        medRecord.PrintMedicalRecord(medicalRecord);
                    }
            string unblock = InputUsername();

            foreach (BlockedPatients blockedPatient in blockedPatientslist)
            {
                if (blockedPatient.Patient.username == unblock)
                {
                    DeleteFromBlockedPatients(blockedPatient.Patient.username, blockedPatientsFile);
                    WriteToPatients(blockedPatient.Patient, patientFile);
                }
            }


        }
        //----------------------------------------------------------------------------

        //PATIENT REQUESTS------------------------------------------------------------
        public void ViewingPatientRequests()
        {

            List<Appointment> appointmentlist = ReadFromAppointment(appointmentFile);
            List<MedicalRecord> medicalRecordList = medRecord.JsonReadFile(medicalRecordFile);
            foreach (Appointment appointment in appointmentlist)
                { 
                    Console.WriteLine(appointment); 
                }
                
            string username = InputUsername();
            //if input da obrisi ako je ne onda ostaje 



        }
        //----------------------------------------------------------------------------
    }
}
