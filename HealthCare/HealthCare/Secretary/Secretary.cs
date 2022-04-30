using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCare.Secretary
{
    public class Secretary : User
    {



        [JsonConstructor]
        public Secretary(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        string medicalRecordFile = "../../../Data/MedicalRecord.json";
        string blockedPatientsFile = "../../../Data/BlockedPatients.json";
        string patientFile = "../../../Data/Patient.json";
        string appointmentFile = "../../../Data/Appointment.json";

        MedicalRecord medRecord = new MedicalRecord();
        Patient.Patient patient = new Patient.Patient();




        public static List<Secretary> Deserialize()
        {
            string path = "../../../Data/SecretariesData.json";
            string jsonText = File.ReadAllText(path);
            List<Secretary> secretaries = JsonSerializer.Deserialize<List<Secretary>>(jsonText);
            return secretaries;
        }


        public static void Serialize(List<Secretary> secretaries)
        {
            File.WriteAllText("../../../Data/SecretariesData.json", JsonSerializer.Serialize(secretaries));
        }


        public string InputUsername()
        {
            Console.Write("\nUnesite korisnicko ime pacijenta: ");
            string username = Console.ReadLine();
            return username;
        }

        //CRUD------------------------------------------------------------------------
        public void CreatePatientAccount()
        {
            MedicalRecord newMedicalRecord = new MedicalRecord();
            MedicalRecord medicalRecord = newMedicalRecord.CreateInput();

            newMedicalRecord.PrintMedicalRecord(medicalRecord);
            newMedicalRecord.SerializePatient(medicalRecord);

            Patient.Patient patient = new Patient.Patient(medicalRecord.Username, medicalRecord.Password,medicalRecord);
            patient.serializePatient();
        }

        public void ReadPatientAccount()
        {
            string username = InputUsername();

            string account = "";
            MedicalRecord newMedicalRecord = new MedicalRecord();
            List<Patient.Patient> patientList = Patient.Patient.patientDeserialization();

                foreach(Patient.Patient patient in patientList)
                    {
                        if (patient.Username == username)
                        {
                            newMedicalRecord.PrintMedicalRecord(patient.MedicalRecord);
                            Console.Write("Da li zelite blokirati ovaj nalog? ");
                            string userResponse = Console.ReadLine();
                            if (userResponse == "da")
                            {
                                BlockingPatientAccount(patient);
                            }
                        } 
                    }
        }

        public void DeletePatientAccount()
        {

            string username = InputUsername();

            
            MedicalRecord newMedicalRecord = new MedicalRecord();
            List<Patient.Patient> patientList = Patient.Patient.patientDeserialization();

            foreach (Patient.Patient patient in patientList)
            {
                if(patient.Username == username)
                    newMedicalRecord.DeleteFromMedicalRecord(username);
                    patient.DeleteFromPatients(username);
               
            }
            Console.WriteLine("Uspjesno obrisan korisnik!");
        }

        public void UpdatePatientAccount()
        {
            DeletePatientAccount();
            CreatePatientAccount();
        }
        //----------------------------------------------------------------------------

        //BLOCKING--------------------------------------------------------------------
        public void BlockingPatientAccount(Patient.Patient patient)
        {
            Patient.Patient newPatient = new Patient.Patient(patient.Username,patient.Password,patient.MedicalRecord);
            newPatient.DeleteFromPatients(patient.Username);

            Patient.BlockedPatients newBlockedPatient = new Patient.BlockedPatients(Patient.BlockedType.Secretary,newPatient);
            newBlockedPatient.serializeBlockedPatient();
        }

        public void UnblockingPatientsAccount()
        {
            MedicalRecord newMedicalRecord = new MedicalRecord();
            Patient.BlockedPatients newBlockedPatient = new Patient.BlockedPatients();
            List<Patient.BlockedPatients> blockedPatientsList = Patient.BlockedPatients.blockedPatientsDeserialization();
           
            foreach (Patient.BlockedPatients blockedPatient in blockedPatientsList)
            {
                newMedicalRecord.ViewMedicalRecord(blockedPatient.Patient.MedicalRecord);
            }

            string unblock = InputUsername();

            foreach (Patient.BlockedPatients blockedPatient in blockedPatientsList)
            {
                if (blockedPatient.Patient.Username == unblock)
                {
                    Patient.Patient patient = new Patient.Patient(blockedPatient.Patient.Username, blockedPatient.Patient.Password,blockedPatient.Patient.MedicalRecord);
                    newBlockedPatient.DeleteFromBlockedPatients(blockedPatient.Patient.Username);
                    patient.serializePatient();
                }
            }


        }
        //----------------------------------------------------------------------------

        //PATIENT REQUESTS------------------------------------------------------------
        public void ViewingPatientRequests()
        {
            Patient.AppointmentRequests newAppointmentRequest = new Patient.AppointmentRequests();
            List<Patient.AppointmentRequests> appointmentlist = newAppointmentRequest.appointmentsRequestDeserialization();
            foreach (Patient.AppointmentRequests appointment in appointmentlist)
                { 
                    Console.WriteLine(appointment);
                }

            
            Console.Write("Unesite ime doktora: ");
            string doctor = Console.ReadLine();
            
            
            Console.Write("Unesite datum i vrijeme: ");
            string date = Console.ReadLine();
            
            
            foreach (Patient.AppointmentRequests appointment in appointmentlist)
            {
                if(appointment.NewAppointment.TimeOfAppointment == date && appointment.NewAppointment.Doctor == doctor)
                {
                    MenageRequestes(appointment);
                } 
            }
        }

        private void MenageRequestes(Patient.AppointmentRequests appointmentRequest)
        {

            Patient.Appointment newAppointment = new Patient.Appointment(appointmentRequest.NewAppointment.TimeOfAppointment, appointmentRequest.NewAppointment.Doctor, appointmentRequest.NewAppointment.Patient);
            Patient.Appointment oldAppointment = new Patient.Appointment(appointmentRequest.OldAppointment.TimeOfAppointment,appointmentRequest.OldAppointment.Doctor,appointmentRequest.OldAppointment.Patient);
            
            if (appointmentRequest.TypeOfChange == Patient.typeOfChange.Delete)
            {
                oldAppointment.deletingAppointmentFromSecretary(oldAppointment);
                appointmentRequest.DeletingAppointmentRequest();
            }
            if (appointmentRequest.TypeOfChange == Patient.typeOfChange.Update)
            {
                oldAppointment.deletingAppointmentFromSecretary(oldAppointment);
                newAppointment.serializeAppointment();
                appointmentRequest.DeletingAppointmentRequest();
            }
            
        }
        //----------------------------------------------------------------------------

        //MANU------------------------------------------------------------------------
        public void PrintHeader(string title)
        {
            Console.WriteLine("\n-------------------------" + title + "-----------------------------");
        }

        public void PrintMainManu()
        {
            Console.WriteLine("-------------OPCIJE---------------");
            Console.WriteLine("1. Manipulisanje nalogom");
            Console.WriteLine("2. Blokiraj naloga");
            Console.WriteLine("3. Odblokiraj naloga");
            Console.WriteLine("4. Pregled zahtjeva");
            Console.WriteLine("5. Exit");
            Console.Write("\r\nUnesite broj opcije: ");

        }

        public void PrintCRUDManu()
        {
            Console.WriteLine("\n1. Keiraj nalog");
            Console.WriteLine("2. Procitaj nalog");
            Console.WriteLine("3. Izmijeni nalog");
            Console.WriteLine("4. Obrisi nalog");
            Console.WriteLine("5. Vratite se nazad");
            Console.Write("\r\nUnesite broj opcije: ");

        }

        public void CheckInput()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = WriteCRUDManu();
            }
        }

        public bool WriteCRUDManu()
        {
            PrintCRUDManu();
            switch (Console.ReadLine())
            {
                case "1":
                    PrintHeader("KREIRAJ NALOG");
                    CreatePatientAccount();
                    return true;
                case "2":
                    PrintHeader("PREGLEDAJ NALOG");
                    ReadPatientAccount();
                    return true;
                case "3":
                    PrintHeader("IZMIJENI NALOG");
                    UpdatePatientAccount();
                    return true;
                case "4":
                    PrintHeader("OBRISI NALOG");
                    DeletePatientAccount();
                    return true;
                case "5":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
            }
        }

        public bool WriteManu()
        {
            PrintMainManu();
            switch (Console.ReadLine())
            {
                case "1":
                    CheckInput();
                    return true;
                case "2":
                    ReadPatientAccount();
                    return true;
                case "3":
                    UnblockingPatientsAccount();
                    return true;
                case "4":
                    ViewingPatientRequests();
                    return true;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
            }

        }
        //----------------------------------------------------------------------------
    }
}
