using HealthCare.Doctor;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCare.Secretary
{
    public class SecretaryService : User
    {
        SecretaryJDBC secretaryJDBC = new SecretaryJDBC();
        public SecretaryService(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public SecretaryService()
        {
        }
        //GENERAL FUNCTIONS----------------------------------------------------------
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
        public MedicalRecord CreateInput()
        {
            Console.Write("\nIme: ");
            string name = Console.ReadLine();

            Console.Write("Prezime: ");
            string lastname = Console.ReadLine();

            Console.Write("Adresa: ");
            string address = Console.ReadLine();

            Console.Write("Korisnicko ime: ");
            string username = Console.ReadLine();

            Console.Write("Lozinka: ");
            string password = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Visina: ");
            string height = Console.ReadLine();

            Console.Write("Tezina: ");
            string weight = Console.ReadLine();

            Console.Write("Krvna grupa: ");
            string bloodType = Console.ReadLine();


            MedicalRecord account = new MedicalRecord(name, lastname, address, username, password, email, height, weight, bloodType);
            return account;

        }

        public void CreatePatientAccount()
        {
            MedicalRecord newMedicalRecord = CreateInput();
            //newMedicalRecord.CreateInput();
            //SecretaryJDBC newMedicalRecord = new SecretaryJDBC();
            //SecretaryJDBC medicalRecord = new newMedicalRecord.CreateInput();

            PrintMedicalRecord(newMedicalRecord);
            secretaryJDBC.SerializePatient(newMedicalRecord);

            Patient.Patient patient = new Patient.Patient(newMedicalRecord.Username, newMedicalRecord.Password, newMedicalRecord);
            patient.serializePatient();
        }

        public void ReadPatientAccount()
        {
            string username = InputUsername();
            MedicalRecord newMedicalRecord = new MedicalRecord();

            List<Patient.Patient> patientList = secretaryJDBC.patientDeserialization();
            foreach (Patient.Patient patient in patientList)
            {
                if (patient.Username == username)
                {
                    PrintMedicalRecord(patient.MedicalRecord);
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
            
            List<Patient.Patient> patientList = secretaryJDBC.patientDeserialization();
            foreach (Patient.Patient patient in patientList)
            {
                if (patient.Username == username)
                    secretaryJDBC.DeleteFromMedicalRecord(username);
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

            Patient.CreatingAppointment.BlockedPatients newBlockedPatient = new Patient.CreatingAppointment.BlockedPatients(Patient.CreatingAppointment.BlockedType.Secretary, newPatient);
            newBlockedPatient.serializeBlockedPatient();
        }

        public void UnblockingPatientsAccount()
        {
            MedicalRecord newMedicalRecord = new MedicalRecord();
            Patient.CreatingAppointment.BlockedPatients newBlockedPatient = new Patient.CreatingAppointment.BlockedPatients();
            List<Patient.CreatingAppointment.BlockedPatients> blockedPatientsList = Patient.CreatingAppointment.BlockedPatients.blockedPatientsDeserialization();

            foreach (Patient.CreatingAppointment.BlockedPatients blockedPatient in blockedPatientsList)
            {
                ViewMedicalRecord(blockedPatient.Patient.MedicalRecord);
            }

            string unblock = InputUsername();

            foreach (Patient.CreatingAppointment.BlockedPatients blockedPatient in blockedPatientsList)
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
            Patient.CreatingAppointment.AppointmentRequests newAppointmentRequest = new Patient.CreatingAppointment.AppointmentRequests();
            List< Patient.CreatingAppointment.AppointmentRequests > appointmentlist = newAppointmentRequest.appointmentsRequestDeserialization();
            foreach (Patient.CreatingAppointment.AppointmentRequests appointment in appointmentlist)
            {
                Console.WriteLine(appointment);
            }

            string doctor = Input("Unesite ime doktora: ");
            string date = Input("Unesite datum i vrijeme");

            foreach (Patient.CreatingAppointment.AppointmentRequests appointment in appointmentlist)
            {
                if (appointment.NewAppointment.TimeOfAppointment == date && appointment.NewAppointment.Doctor == doctor)
                {
                    MenageRequestes(appointment);
                }
            }
        }

        private void MenageRequestes(Patient.CreatingAppointment.AppointmentRequests appointmentRequest)
        {

            Patient.CreatingAppointment.Appointment newAppointment = new Patient.CreatingAppointment.Appointment(appointmentRequest.NewAppointment.TimeOfAppointment, appointmentRequest.NewAppointment.Doctor, appointmentRequest.NewAppointment.Patient);
            Patient.CreatingAppointment.Appointment oldAppointment = new Patient.CreatingAppointment.Appointment(appointmentRequest.OldAppointment.TimeOfAppointment, appointmentRequest.OldAppointment.Doctor, appointmentRequest.OldAppointment.Patient);

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
                        Patient.CreatingAppointment.Appointment newAppointment = new Patient.CreatingAppointment.Appointment(date, patient.MedicalRecord.Doktor, patient.Username);
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
            List<Patient.CreatingAppointment.Appointment> patientList = Patient.CreatingAppointment.Appointment.appointmentsDeserialization();
            foreach (Patient.CreatingAppointment.Appointment appointment in patientList)
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
                    string specialization = Input("Unesite specijalnost doktora: ");
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
            List<Patient.CreatingAppointment.Appointment> patientList = Patient.CreatingAppointment.Appointment.appointmentsDeserialization();
            foreach (Patient.CreatingAppointment.Appointment appointment in patientList)
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
                    PrintDayOffRequests(request, i);
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

        private void PrintDayOffRequests(DaysOffRequest request, int i)
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
                if (i == numberOfRequest)
                {
                    if (option == "odbij")
                    {
                        RejectDaysOffRequest(request, daysOffList);

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


        //MEDICAL RECORD//------------------------------------------------------------
        public void ViewMedicalRecord(MedicalRecord record)
        {
            string[] title = { "Ime", "Prezime", "Adresa", "Korisnicko ime", "Lozinka", "Email", "Visina", "Tezina", "Krvna grupa" };
            string[] population = { record.Name, record.Lastname, record.Address, record.Username, record.Password, record.Email, record.Height, record.Weight, record.BloodType };
            var sb = new System.Text.StringBuilder();
            sb.Append(String.Format("\n{0,-20} {1,-10}\n---------------------------------\n", "Labela", "Podaci"));
            for (int index = 0; index < title.Length; index++)
                sb.Append(String.Format("{0,-20} {1,-10}\n", title[index], population[index]));
            sb.Append("---------------------------------");
            Console.WriteLine(sb);
        }

        public void PrintMedicalRecord(MedicalRecord account)
        {
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("                   ZDRAVSTVENI KARTON                 ");
            Console.WriteLine("-------------------------------------------------------------");
            ViewMedicalRecord(account);
            Console.WriteLine("-------------------------------------------------------------");
        }
        //----------------------------------------------------------------------------
    }

}
