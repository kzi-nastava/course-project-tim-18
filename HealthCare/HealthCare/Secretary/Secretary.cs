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
        Patient patient = new Patient();

        
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
            newMedicalRecord.PrintMedicalRecordHeader(medicalRecord);
            newMedicalRecord.SerializePatient(medicalRecord, medicalRecordFile);

            Patient patient = new Patient(medicalRecord.Username, medicalRecord.Password);
            patient.SerializePatient();
        }

        public void ReadPatientAccount()
        {
            string username = InputUsername();

            string account = "";
            MedicalRecord newMedicalRecord = new MedicalRecord();
            List<MedicalRecord> medicalRecordList = newMedicalRecord.MedicalRecordDeserialization();
            List<Patient> patientList = patient.PatientDeserialization();

            foreach (MedicalRecord medicalRecord in medicalRecordList)
                foreach(Patient patient in patientList)
                    {
                        if (medicalRecord.Username == username && patient.username == username)
                        {
                            newMedicalRecord.PrintMedicalRecordHeader(medicalRecord);
                            Console.Write("Da li zelite blokirati ovaj nalog? ");
                            string userResponse = Console.ReadLine();
                            if (userResponse == "da")
                            {
                                BlockingPatientAccount(medicalRecord.Username,medicalRecord.Password);
                            }
                        } 
                    }
        }

        public void DeletePatientAccount()
        {

            string username = InputUsername();

            Patient newPatient = new Patient();
            MedicalRecord newMedicalRecord = new MedicalRecord();
            List<Patient> patientList = newPatient.PatientDeserialization();

            foreach (Patient patient in patientList)
            {
                if(patient.username == username)
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
        public void BlockingPatientAccount(string username,string password)
        {
            Patient patient = new Patient(username,password);
            patient.DeleteFromPatients(username);
            BlockedPatients blockedPatient = new BlockedPatients(BlockedType.Secretary,patient);
            blockedPatient.SerializeBlockedPatient();
        }

        public void UnblockingPatientsAccount()
        {
            MedicalRecord newMedicalRecord = new MedicalRecord();
            BlockedPatients newBlockedPatient = new BlockedPatients();

            List<BlockedPatients> blockedPatientsList = newBlockedPatient.BlockedPatientsDeserialization();
            List<MedicalRecord> medicalRecordList = newMedicalRecord.MedicalRecordDeserialization();

            foreach (BlockedPatients blockedPatient in blockedPatientsList)
                foreach(MedicalRecord medicalRecord in medicalRecordList)
                    {
                       if(blockedPatient.Patient.username == medicalRecord.Username)
                        newMedicalRecord.ViewMedicalRecord(medicalRecord);
                    }
            string unblock = InputUsername();

            foreach (BlockedPatients blockedPatient in blockedPatientsList)
            {
                if (blockedPatient.Patient.username == unblock)
                {
                    Patient patient = new Patient(blockedPatient.Patient.username, blockedPatient.Patient.password);
                    newBlockedPatient.DeleteFromBlockedPatients(blockedPatient.Patient.username, blockedPatientsFile);
                    patient.SerializePatient();
                }
            }


        }
        //----------------------------------------------------------------------------

        //PATIENT REQUESTS------------------------------------------------------------
        public void ViewingPatientRequests()
        {
            AppointmentRequests newAppointmentRequest = new AppointmentRequests();

            List<AppointmentRequests> appointmentlist = newAppointmentRequest.appointmentsRequestDeserialization();
            foreach (AppointmentRequests appointment in appointmentlist)
                { 
                    Console.WriteLine(appointment);
                }

            DateTime date1 = new DateTime();
            Console.Write("Unesite ime doktora: ");
            string doctor = Console.ReadLine();
            
            
            Console.Write("Unesite datum i vrijeme: ");
            string date = Console.ReadLine();
            Console.WriteLine(date);
            
            foreach (AppointmentRequests appointment in appointmentlist)
            {
                if(appointment.NewAppointment.TimeOfAppointment == date && appointment.NewAppointment.Doctor == doctor)
                {

                    Console.WriteLine(appointment);
                    MenageRequestes(appointment);
                } 
            }
        }

        private void MenageRequestes(AppointmentRequests appointmentRequest)
        {
           
            Appointment newAppointment = new Appointment(appointmentRequest.NewAppointment.TimeOfAppointment, appointmentRequest.NewAppointment.Doctor, appointmentRequest.NewAppointment.Patient);
            Appointment oldAppointment = new Appointment(appointmentRequest.OldAppointment.TimeOfAppointment,appointmentRequest.OldAppointment.Doctor,appointmentRequest.OldAppointment.Patient);
            
            if (appointmentRequest.TypeOfChange == typeOfChange.Delete)
            {
                oldAppointment.deletingAppointment(oldAppointment);
                appointmentRequest.DeletingAppointmentRequest();
            }
            if (appointmentRequest.TypeOfChange == typeOfChange.Update)
            {
                oldAppointment.deletingAppointment(oldAppointment);
                newAppointment.serializeAppointment();
                appointmentRequest.DeletingAppointmentRequest();
            }
            
        }
        //----------------------------------------------------------------------------
    }
}
