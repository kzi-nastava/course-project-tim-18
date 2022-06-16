﻿using HealthCare.Doctor;
using System.Globalization;
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
        
        //GENERAL FUNCTIONS----------------------------------------------------------
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

        public string Input(string text)
        {
            Console.Write(text);
            string option = Console.ReadLine();
            return option;
        }
        //----------------------------------------------------------------------------

        //CRUD------------------------------------------------------------------------
        public void CreatePatientAccount()
        {
            MedicalRecord newMedicalRecord = new MedicalRecord();
            MedicalRecord medicalRecord = newMedicalRecord.CreateInput();

            newMedicalRecord.PrintMedicalRecord(medicalRecord);
            newMedicalRecord.SerializePatient(medicalRecord);

            Patient.Patient patient = new Patient.Patient(medicalRecord.Username, medicalRecord.Password, medicalRecord);
            patient.serializePatient();
        }

        public void ReadPatientAccount()
        {
            string username = InputUsername();
            MedicalRecord newMedicalRecord = new MedicalRecord();

            List<Patient.Patient> patientList = Patient.Patient.patientDeserialization();
            foreach (Patient.Patient patient in patientList)
            {
                if (patient.Username == username)
                {
                    newMedicalRecord.PrintMedicalRecord(patient.MedicalRecord);
                    string userResponse = Input("Da li zelite blokirati ovaj nalog? ");
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
                if (patient.Username == username)
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
            Patient.Patient newPatient = new Patient.Patient(patient.Username, patient.Password, patient.MedicalRecord);
            newPatient.DeleteFromPatients(patient.Username);

            Patient.BlockedPatients newBlockedPatient = new Patient.BlockedPatients(Patient.BlockedType.Secretary, newPatient);
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
                    Patient.Patient patient = new Patient.Patient(blockedPatient.Patient.Username, blockedPatient.Patient.Password, blockedPatient.Patient.MedicalRecord);
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

            string doctor = Input("Unesite ime doktora: ");
            string date = Input("Unesite datum i vrijeme");

            foreach (Patient.AppointmentRequests appointment in appointmentlist)
            {
                if (appointment.NewAppointment.TimeOfAppointment == date && appointment.NewAppointment.Doctor == doctor)
                {
                    MenageRequestes(appointment);
                }
            }
        }

        private void MenageRequestes(Patient.AppointmentRequests appointmentRequest)
        {

            Patient.Appointment newAppointment = new Patient.Appointment(appointmentRequest.NewAppointment.TimeOfAppointment, appointmentRequest.NewAppointment.Doctor, appointmentRequest.NewAppointment.Patient);
            Patient.Appointment oldAppointment = new Patient.Appointment(appointmentRequest.OldAppointment.TimeOfAppointment, appointmentRequest.OldAppointment.Doctor, appointmentRequest.OldAppointment.Patient);

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
            Console.WriteLine("1  Manipulisanje nalogom");
            Console.WriteLine("2  Blokiraj naloga");
            Console.WriteLine("3  Odblokiraj naloga");
            Console.WriteLine("4  Pregled zahtjeva");
            Console.WriteLine("5  Zakazivanje pregleda");
            Console.WriteLine("6  Nabavka dinamicke opreme");
            Console.WriteLine("7  Rasporedjivanje dinamicke opreme");
            Console.WriteLine("8  Pregled zahtjeva za slobodne dane");
            Console.WriteLine("9  Exit");

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

        public bool WriteManu(Manager manager)
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
                case "5":
                    MakingAnAppointment();
                    return true;
                case "6":
                    manager.Load();
                    manager.DynamicEquipmentRequests();
                    manager.Save();
                    return true;
                case "7":
                    manager.Load();
                    manager.DynamicEquipmentDistribution();
                    manager.Save();
                    return true;
                case "8":
                    ViewDayOffRequests();
                    return true;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
            }


        }
        //----------------------------------------------------------------------------

        //INSTRUCTIONS----------------------------------------------------------------
        public void MakingAnAppointment()
        {
           string username = InputUsername();
           List<Patient.Patient> patientList = Patient.Patient.patientDeserialization();

            foreach (Patient.Patient patient in patientList)
            {
                if (patient.Username == username && patient.MedicalRecord.Doktor != "")
                {
                    Console.WriteLine("Doktor: " + patient.MedicalRecord.Doktor);
                    string date = Input("\nUnesite datum pregleda: ");
                    Boolean isTrue = CheckDate(date);
                    if (isTrue)
                    {
                        Patient.Appointment newAppointment = new Patient.Appointment(date, patient.MedicalRecord.Doktor, patient.Username);
                        newAppointment.serializeAppointment();
                    }
                }      
            }

        }

        public bool CheckDate(string period)
        {
            DateTime dt;
            if (period == "" || !DateTime.TryParseExact(period, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            if (CheckIfAvailable(DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null)))
            {
                Console.WriteLine("Doktor nije dostupan za dat termin.");
                return false;
            }
            return true;
        }

        public bool CheckIfAvailable(DateTime appointmentDate)
        {
            List<Patient.Appointment> patientList = Patient.Appointment.appointmentsDeserialization();
            foreach (Patient.Appointment appointment in patientList)
            {
                DateTime dt = DateTime.ParseExact(appointment.TimeOfAppointment, "dd/MM/yyyy HH:mm", null);
                if ((dt - appointmentDate).TotalMinutes < 15)
                {
                    return true;
                }
            }
            return false;
        }
        //----------------------------------------------------------------------------


        //EMERGENCY APPOINTMENT REQUESTS----------------------------------------------
        public void MakingAnEmergencyAppointment()
        {
            string username = InputUsername();
            List<Patient.Patient> patientList = Patient.Patient.patientDeserialization();

            foreach (Patient.Patient patient in patientList)
            {
                if (patient.Username == username)
                {
                    string specialization =Input("Unesite specijalnost doktora: ");
                    FindValidDoctor(specialization);
                }
            }

        }

        private void FindValidDoctor(string specialization)
        {
            List<Doctor.Doctor> doctorList = Doctor.Doctor.Deserialize();
            Doctor.DoctorSpecialization docSpecialization = (Doctor.DoctorSpecialization)Enum.Parse(typeof(Doctor.DoctorSpecialization), specialization);
            foreach (Doctor.Doctor doctor in doctorList)
            {
                if (doctor.Specialization == docSpecialization)
                {
                    Console.WriteLine(doctor.Name);
                    FindAvailableAppointment(doctor);
                }
            }
        }

        private void FindAvailableAppointment(Doctor.Doctor doctor)
        {
            List<Patient.Appointment> patientList = Patient.Appointment.appointmentsDeserialization();
            foreach (Patient.Appointment appointment in patientList)
            {
                if (appointment.Doctor == doctor.Name)
                {
                   

                }

            }
        }
        //----------------------------------------------------------------------------


        //DAYS OFF REQUESTS-----------------------------------------------------------
        public void ViewDayOffRequests()
        {
            List<Doctor.DaysOffRequest> daysOffList = Doctor.DaysOffRequest.Deserialize();
            int i = 1;

            foreach (Doctor.DaysOffRequest request in daysOffList)
            {
                if (request.State.ToString() == "AwaitingDecision")
                {
                    PrintDayOffRequests(request,i);
                }
                i++;

            }
            string answer = Input("Da li zelite odobriti/odbiti zahtjev? ");
            if (answer == "da")
            {
                string numberOfRequest = Input("Unesite broj zahtjeva: ");
                AcceptOrRejectDaysOffRequest(numberOfRequest);
            }

        }

        private void PrintDayOffRequests(DaysOffRequest request ,int i)
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("ZAHTJEV " + i);
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine(request.VacationStart);
            Console.WriteLine(request.VacationEnd);
            Console.WriteLine(request.DoctorName);
            Console.WriteLine(request.State);
            Console.WriteLine("----------------------------------------------\n");
        }

        private void AcceptOrRejectDaysOffRequest(string? number)
        {
            int numberOfRequest = Int32.Parse(number);
            int i = 1;

            string option = Input("Unesite opciju odbij/prihvati: ");

            List<Doctor.DaysOffRequest> daysOffList = Doctor.DaysOffRequest.Deserialize();
            foreach (Doctor.DaysOffRequest request in daysOffList)
            {
                if(i == numberOfRequest)
                {
                    if (option == "odbij")
                    {
                        RejectDaysOffRequest(request,daysOffList);
                        
                    }
                    if (option == "prihvati")
                    {
                        AcceptDaysOffRequest(request, daysOffList);
                        
                    }
                }
                i++;
            }
        }

        private void AcceptDaysOffRequest(DaysOffRequest request, List<DaysOffRequest> daysOffList)
        {
            Doctor.RequestState requestStateAccepted = Doctor.RequestState.Accepted;
            request.State = requestStateAccepted;

            Doctor.DaysOffRequest daysOffRequest = new Doctor.DaysOffRequest(request.VacationStart, request.VacationEnd, request.IsUrgent, request.RequestMessage, request.State, request.DoctorName);
            daysOffRequest.DaysOffSerialization(daysOffList);
        }

        private void RejectDaysOffRequest(DaysOffRequest request, List<DaysOffRequest> daysOffList)
        {
            Doctor.RequestState requestStateDenied = Doctor.RequestState.Denied;
            string explanation = Input("Unesite obrazlozenje: ");

            request.RequestMessage = explanation;
            request.State = requestStateDenied;

            Doctor.DaysOffRequest daysOffRequest = new Doctor.DaysOffRequest(request.VacationStart, request.VacationEnd, request.IsUrgent, request.RequestMessage, request.State, request.DoctorName);
            daysOffRequest.DaysOffSerialization(daysOffList);
        }

        public static void SendNotificationToDoctor(string doctorName)
        {
            Console.WriteLine("Notifikacija za slobodne dane");
            Console.WriteLine("-----------------------------------");

            List<Doctor.DaysOffRequest> daysOffList = Doctor.DaysOffRequest.Deserialize();
            foreach (Doctor.DaysOffRequest request in daysOffList)
            {
                if (request.DoctorName == doctorName)
                {
                    PrintNotification(request);
                }
            }
        }

        private static void PrintNotification(DaysOffRequest request)
        {
            if (request.State.ToString() == "Accepted")
            {
                Console.WriteLine("Odobren zahtjev za slobodne dane!");
            }
            if (request.State.ToString() == "Denied")
            {
                Console.WriteLine("Odbijen zahtjev za slobodne dane!");
            }
        }
        //----------------------------------------------------------------------------
    }
}
