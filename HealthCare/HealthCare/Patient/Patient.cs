
using System.Globalization;
using System.Text.Json;
using HealthCare.Doctor;
using HealthCare.Patient.Grading;
using HealthCare.Secretary;

namespace HealthCare.Patient
{
    public class Patient : User
    {
        private MedicalRecord medicalRecord;
        private int hoursForNotification;

        public int HoursForNotification
        {
            get => hoursForNotification;
            set => hoursForNotification = value;
        }
        public MedicalRecord MedicalRecord
        {
            get => medicalRecord;
            set => medicalRecord = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Patient(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public Patient()
        {
        }

        public Patient(string username, string password, MedicalRecord medicalRecord)
        {
            this.username = username;
            this.password = password;
            this.medicalRecord = medicalRecord;
        }

        public static List<Patient> patientDeserialization()
        {
            string fileName = "../../../Data/Patients.json";
            string PatientFileData = "";
            PatientFileData = File.ReadAllText(fileName);
            string[] Patients = PatientFileData.Split('\n');
            List<Patient> PatientsList = new List<Patient>();
            foreach (String s in Patients)
            {
                if (s != "")
                {
                    Patient? patient = JsonSerializer.Deserialize<Patient>(s);
                    if (patient != null)
                        PatientsList.Add(patient);

                }
            }

            return PatientsList;
        }

        public void blockingPatient()
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient> patients = patientDeserialization();
            string json = "";
            foreach (Patient patient in patients)
            {
                if (patient.username != this.username && patient.password != this.password)
                    json += JsonSerializer.Serialize(patient) + "\n";
            }

            File.WriteAllText(fileName, json);
            CreatingAppointment.BlockedPatients blocked = new CreatingAppointment.BlockedPatients(CreatingAppointment.BlockedType.Patient, this);
            blocked.serializeBlockedPatient();
        }

        public void serializePatient()
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient> Patients = patientDeserialization();
            string json = "";
            foreach (Patient Patient in Patients)
            {
                json += JsonSerializer.Serialize(Patient) + "\n";
            }

            json += JsonSerializer.Serialize(this) + "\n";
            ;
            File.WriteAllText(fileName, json);
        }

        static public void Serialize(List<Patient> Patients)
        {
            string fileName = "../../../Data/Patients.json";
            string json = "";
            foreach (Patient Patient in Patients)
            {
                json += JsonSerializer.Serialize(Patient) + "\n";
            }

            File.WriteAllText(fileName, json);
        }

        public List<CreatingAppointment.Appointment> DoctorAppointmentsRecommendation(string doctor, string timeOfStartString,
            string timeOfFinishString, string dateOfFinishString)
        {
            List<CreatingAppointment.Appointment> recommendedAppointments = new List<CreatingAppointment.Appointment>();
            string[] dateAsArray = dateOfFinishString.Split("/");
            int day = int.Parse(dateAsArray[0]) + 1;
            int month = int.Parse(dateAsArray[1]);
            int year = int.Parse(dateAsArray[2]);
            string[] timeAsArray = timeOfStartString.Split(":");
            int hourStart = int.Parse(timeAsArray[0]);
            int minuteStart = int.Parse(timeAsArray[1]);
            string[] timeAsArrayEnd = timeOfFinishString.Split(":");
            //DateTime dateForRecommendation = new DateTime(year, month, day, hourStart, minuteStart, 0);
            DateTime dateForRecommendation = DateTime.Today;
            int hourFinish = int.Parse(timeAsArrayEnd[0]);
            int minuteFinish = int.Parse(timeAsArrayEnd[1]);
            DateTime dateForChecking = new DateTime(year, month, day, hourFinish, minuteFinish, 0);
            TimeSpan changeStart = new TimeSpan(hourStart, minuteStart, 0);
            TimeSpan ChangeEnd = new TimeSpan(hourFinish, minuteFinish, 0);
            TimeSpan timeNeededForExamption = new TimeSpan(0, 15, 0);
            int i = 1;
            TimeSpan difference = dateForRecommendation.Subtract(dateForChecking);
            while (difference.Days != 0)
            {
                DateTime timeBeingChecked = dateForRecommendation.AddDays(i);
                timeBeingChecked = timeBeingChecked + changeStart;
                TimeSpan minutes = new TimeSpan(0, 0, 0);
                while (changeStart + minutes < ChangeEnd)
                {
                    timeBeingChecked = timeBeingChecked + timeNeededForExamption;
                    minutes = minutes + timeNeededForExamption;
                    string stringTimeBeingChecked = timeBeingChecked.ToString("dd/MM/yyyy HH:mm");
                    if (CreatingAppointment.Appointment.isAppointmentValid(stringTimeBeingChecked, doctor) == true)
                    {
                        CreatingAppointment.Appointment appointment = new CreatingAppointment.Appointment(stringTimeBeingChecked, doctor, this.username,
                            HealthCare.Doctor.PerformAppointment.AppointmentType.Examination);
                        Console.WriteLine(appointment);
                        recommendedAppointments.Add(appointment);
                        return recommendedAppointments;
                    }
                }

                i += 1;
                difference = timeBeingChecked.Subtract(dateForChecking);
            }

            List<Doctor.Doctor> doctors = Doctor.Doctor.Deserialize();
            foreach (Doctor.Doctor d in doctors)
            {
                while (difference.Days != 0)
                {
                    DateTime timeBeingChecked = dateForRecommendation.AddDays(i);
                    timeBeingChecked = timeBeingChecked + changeStart;
                    TimeSpan minutes = new TimeSpan(0, 0, 0);
                    while (changeStart + minutes < ChangeEnd)
                    {
                        timeBeingChecked = timeBeingChecked + timeNeededForExamption;
                        minutes = minutes + timeNeededForExamption;
                        string stringTimeBeingChecked = timeBeingChecked.ToString("dd/MM/yyyy HH:mm");
                        if (CreatingAppointment.Appointment.isAppointmentValid(stringTimeBeingChecked, d.Username) == true)
                        {
                            CreatingAppointment.Appointment appointment = new CreatingAppointment.Appointment(stringTimeBeingChecked, d.Username, this.username,
                                HealthCare.Doctor.PerformAppointment.AppointmentType.Examination);
                            recommendedAppointments.Add(appointment);
                            if (recommendedAppointments.Count == 3)
                            {
                                Console.WriteLine("Pronadjeni termini su najpriblizniji unetim kriterijumima:");
                                foreach (CreatingAppointment.Appointment a in recommendedAppointments)
                                {
                                    Console.WriteLine(a);
                                }

                                return recommendedAppointments;
                            }
                        }
                    }

                    i += 1;
                    difference = timeBeingChecked.Subtract(dateForChecking);
                }
            }

            Console.WriteLine("Pronadjeni termini su najpriblizniji unetim kriterijumima:");
            foreach (CreatingAppointment.Appointment a in recommendedAppointments)
            {
                Console.WriteLine(a);
            }

            return recommendedAppointments;
        }

        public List<CreatingAppointment.Appointment> DateTimeAppointmentsRecommendation(string doctor, string timeOfStartString,
            string timeOfFinishString, string dateOfFinishString)
        {
            List<CreatingAppointment.Appointment> recommendedAppointments = new List<CreatingAppointment.Appointment>();
            string[] dateAsArray = dateOfFinishString.Split("/");
            int day = int.Parse(dateAsArray[0]) + 1;
            int month = int.Parse(dateAsArray[1]);
            int year = int.Parse(dateAsArray[2]);
            string[] timeAsArray = timeOfStartString.Split(":");
            int hourStart = int.Parse(timeAsArray[0]);
            int minuteStart = int.Parse(timeAsArray[1]);
            string[] timeAsArrayEnd = timeOfFinishString.Split(":");
            //DateTime dateForRecommendation = new DateTime(year, month, day, hourStart, minuteStart, 0);
            DateTime dateForRecommendation = DateTime.Today;
            int hourFinish = int.Parse(timeAsArrayEnd[0]);
            int minuteFinish = int.Parse(timeAsArrayEnd[1]);
            DateTime dateForChecking = new DateTime(year, month, day, hourFinish, minuteFinish, 0);
            TimeSpan changeStart = new TimeSpan(hourStart, minuteStart, 0);
            TimeSpan ChangeEnd = new TimeSpan(hourFinish, minuteFinish, 0);
            TimeSpan timeNeededForExamption = new TimeSpan(0, 15, 0);
            int i = 1;
            List<Doctor.Doctor> doctors = Doctor.Doctor.Deserialize();
            TimeSpan difference = dateForRecommendation.Subtract(dateForChecking);
            while (difference.Days != 0)
            {
                DateTime timeBeingChecked = dateForRecommendation.AddDays(i);
                timeBeingChecked = timeBeingChecked + changeStart;
                TimeSpan minutes = new TimeSpan(0, 0, 0);
                while (changeStart + minutes < ChangeEnd)
                {
                    timeBeingChecked = timeBeingChecked + timeNeededForExamption;
                    minutes = minutes + timeNeededForExamption;
                    string stringTimeBeingChecked = timeBeingChecked.ToString("dd/MM/yyyy HH:mm");
                    if (CreatingAppointment.Appointment.isAppointmentValid(stringTimeBeingChecked, doctor))
                    {
                        CreatingAppointment.Appointment appointment = new CreatingAppointment.Appointment(stringTimeBeingChecked, doctor, this.username,
                            HealthCare.Doctor.PerformAppointment.AppointmentType.Examination);
                        recommendedAppointments.Add(appointment);
                        return recommendedAppointments;
                    }
                    else
                    {
                        foreach (Doctor.Doctor d in doctors)
                        {
                            if (CreatingAppointment.Appointment.isAppointmentValid(stringTimeBeingChecked, d.Username))
                            {
                                CreatingAppointment.Appointment appointment = new CreatingAppointment.Appointment(stringTimeBeingChecked, d.Username,
                                    this.username,
                                    HealthCare.Doctor.PerformAppointment.AppointmentType.Examination);
                                recommendedAppointments.Add(appointment);

                            }
                        }

                        return recommendedAppointments;
                    }
                }

                i += 1;
                difference = timeBeingChecked.Subtract(dateForChecking);
            }

            return recommendedAppointments;
        }


        public void AppointmentRecommendation()
        {
            Console.WriteLine("Unesite ime doktora kod koga zelite da zakazete tretman:");
            string doctor = Console.ReadLine();
            Console.WriteLine("Unesite vreme od kada mozete da idete na tretman:(U formatu HH:MM)");
            string timeOfStart = Console.ReadLine();
            Console.WriteLine("Unesite vreme do kada mozete da idete na tretman:(U formatu HH:MM)");
            string timeOfFinish = Console.ReadLine();
            Console.WriteLine("Unesite datum do kada najkasnije mozete da idete na tretman:(U formatu dd/mm/yyyy)");
            string dateOfFinish = Console.ReadLine();
            Console.WriteLine("Unesite broj ispred opcije po kojoj zelite preporuku:\n1Doktor\n2Vreme");
            string option = Console.ReadLine();
            if (option == "1")
            {
                List<CreatingAppointment.Appointment> recommendedAppointments =
                    DoctorAppointmentsRecommendation(doctor, timeOfStart, timeOfFinish, dateOfFinish);
                Console.WriteLine(
                    "Unesite redni broj ispred appointmenta koji zelite da zakazete ukoliko ne zelite ni jedan unesite bilo sta drugo");
                string appointmentOptionS = Console.ReadLine();
                int appointmentOption = int.Parse(appointmentOptionS);
                int i = 0;
                foreach (CreatingAppointment.Appointment appointment in recommendedAppointments)
                {
                    i += 1;
                    if (i == appointmentOption)
                    {
                        appointment.RoomId = Doctor.Doctor.DoctorsRoom(appointment.Doctor);
                        appointment.serializeAppointment();
                    }
                }
            }

            if (option == "2")
            {
                List<CreatingAppointment.Appointment> recommendedAppointments =
                    DoctorAppointmentsRecommendation(doctor, timeOfStart, timeOfFinish, dateOfFinish);
                Console.WriteLine(
                    "Unesite redni broj ispred appointmenta koji zelite da zakazete ukoliko ne zelite ni jedan unesite bilo sta drugo");
                string appointmentOptionS = Console.ReadLine();
                int appointmentOption = int.Parse(appointmentOptionS);
                int i = 0;
                foreach (CreatingAppointment.Appointment appointment in recommendedAppointments)
                {
                    i += 1;
                    if (i == appointmentOption)
                    {
                        appointment.RoomId = Doctor.Doctor.DoctorsRoom(appointment.Doctor);
                        appointment.serializeAppointment();
                    }
                }
            }

        }

        public void WritingAppointment(string doctor)
        {
            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            CreatingAppointment.AntiTrolCounter counter = new CreatingAppointment.AntiTrolCounter(this.username, dateInString, typeOfChange.Create);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                //Console.WriteLine("Unesite ime doktora kod koga zelite tretman: ");
                //string doctor = Console.ReadLine();
                Console.WriteLine("Unesite vreme tretmana koji zelite da zakazete:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                if (CreatingAppointment.Appointment.isAppointmentValid(timeOfAppointment, doctor) == true)
                {
                    CreatingAppointment.Appointment appointment = new CreatingAppointment.Appointment(timeOfAppointment, doctor, this.username,
                        HealthCare.Doctor.PerformAppointment.AppointmentType.Examination, Doctor.Doctor.DoctorsRoom(doctor));
                    //Doctor.Doctor.addAppointmentForDoctor(appointment, doctor);
                    appointment.serializeAppointment();
                }
            }
            else
            {
                Console.WriteLine("Prevelik broj zakazivanja novih tretmana vas nalog ce sada biti blokiran: ");
            }
        }

        public void MakingAppointment()
        {
            string doctor = Console.ReadLine();
            Console.WriteLine("Unesite vreme tretmana koji zelite da zakazete:(u formatu DD/MM/YYYY hh:mm ");
            WritingAppointment(doctor);
        }

        public void changingAppointment()
        {

            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            CreatingAppointment.AntiTrolCounter counter = new CreatingAppointment.AntiTrolCounter(this.username, dateInString, typeOfChange.Update);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol)
            {
                CreatingAppointment.Appointment.printingAppointment();
                Console.WriteLine("Unesite vreme tretmana koji zelite da izmenite:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                Console.WriteLine("Unesite ime doktora kod koga zelite da izmenite tretman: ");
                string doctor = Console.ReadLine();
                List<CreatingAppointment.Appointment> appointments = CreatingAppointment.Appointment.appointmentsDeserialization();
                bool validationOfNewAppointment = false;
                CreatingAppointment.Appointment oldAppointment = new CreatingAppointment.Appointment();
                int indexOfRequest = 0;
                for (int i = 0; i < appointments.Count; i++)
                {
                    if (appointments[i].TimeOfAppointment == timeOfAppointment && appointments[i].Doctor == doctor)
                    {
                        indexOfRequest = i;
                        Console.WriteLine(
                            "Unesite broj ispred opcije:\n1 Promena doktora\n2 Promena vremena termina:\nSve drugo za kraj");
                        string option = Console.ReadLine();
                        if (option == "1")
                        {
                            Console.WriteLine("Unesite ime doktora kod koga zelite novi termin: ");
                            string newDoctor = Console.ReadLine();
                            oldAppointment = appointments[i];
                            appointments[i].Doctor = newDoctor;
                            appointments[i].RoomId = Doctor.Doctor.DoctorsRoom(newDoctor);
                            if (CreatingAppointment.Appointment.isAppointmentValid(appointments[i].TimeOfAppointment,
                                    appointments[i].Doctor))
                                validationOfNewAppointment = true;
                        }

                        if (option == "2")
                        {
                            Console.WriteLine("Unesite vreme novog termina: ");
                            string newTimeOfAppointment = Console.ReadLine();
                            oldAppointment = appointments[i];
                            appointments[i].TimeOfAppointment = newTimeOfAppointment;
                            if (CreatingAppointment.Appointment.isAppointmentValid(appointments[i].TimeOfAppointment,
                                    appointments[i].Doctor))
                            {
                                validationOfNewAppointment = true;
                                timeOfAppointment = newTimeOfAppointment;
                            }
                        }
                    }
                }

                DateTime timeChecked = CreatingAppointment.Appointment.stringToDateTime(timeOfAppointment);
                TimeSpan timeDifference = timeChecked.Subtract(DateTime.Now);
                if (validationOfNewAppointment)
                    if (timeDifference.TotalDays > 2)
                        CreatingAppointment.Appointment.serializingListOfAppointments(appointments);
                    else
                    {
                        CreatingAppointment.AppointmentRequests appointmentRequest = new CreatingAppointment.AppointmentRequests(oldAppointment,
                            appointments[indexOfRequest], typeOfChange.Update);
                        appointmentRequest.serializeAppointmentRequest();
                    }
            }
            else
            {
                this.blockingPatient();
                Console.WriteLine("Prevelik broj promena vas nalog je sada blokiran: ");
            }
        }

        public void deletingAppointment()
        {
            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            CreatingAppointment.AntiTrolCounter counter = new CreatingAppointment.AntiTrolCounter(this.username, dateInString, typeOfChange.Update);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Console.WriteLine("Unesite ime doktora kod koga zelite da izbrisete tretman: ");
                string doctor = Console.ReadLine();
                Console.WriteLine("Unesite vreme tretmana koji zelite da izbrisete:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                CreatingAppointment.Appointment appointment = new CreatingAppointment.Appointment(timeOfAppointment, doctor, this.username);
                DateTime timeChecked = CreatingAppointment.Appointment.stringToDateTime(timeOfAppointment);
                TimeSpan timeDifference = timeChecked.Subtract(DateTime.Now);
                if (timeDifference.TotalDays > 2)
                    appointment.deletingAppointment();
                else
                {
                    CreatingAppointment.AppointmentRequests appointmentRequest =
                        new CreatingAppointment.AppointmentRequests(appointment, appointment, typeOfChange.Delete);
                    appointmentRequest.serializeAppointmentRequest();
                }

            }
            else
            {
                this.blockingPatient();
                this.DeleteFromPatients(this.Username);
                Console.WriteLine("Prevelik broj brisanja vas nalog je sada blokiran: ");
            }
        }

        public void DeleteFromPatients(string username)
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient> blockedPatients = patientDeserialization();
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

        public void ReportsOverview()
        {
            List<Doctor.PerformAppointment.Report> reports = Doctor.PerformAppointment.Report.deserializeForPatient(this.Username);
            Console.WriteLine(
                "Unesite po kojem kriterijumu zelite pregled:\n1 Sortirani po datumu\n2 Pregledi odredjenog doktora\n3 Pregledi po specijalnosti doktora");
            string option = Console.ReadLine();
            if (option == "1")
            {
                Console.WriteLine("Tretmani su sortirani po datumu pregleda: ");
                Doctor.PerformAppointment.Report.SortReport(ref reports, 0, reports.Count - 1);
                foreach (Doctor.PerformAppointment.Report report in reports)
                {
                    Console.WriteLine("Karton: " + report.MedicalRecord + " termin: " + report.Appointment +
                                      "\n opis: " + report.Description);
                }
            }

            if (option == "2")
            {
                Console.WriteLine("Unesite ime doktora za koga zelite da vidite tretmane:");
                string doctor = Console.ReadLine();
                foreach (Doctor.PerformAppointment.Report report in reports)
                {
                    if (report.Appointment.Doctor == doctor)
                    {
                        Console.WriteLine("Karton: " + report.MedicalRecord + " termin: " + report.Appointment +
                                          "\n opis: " + report.Description);
                    }
                }
            }

            if (option == "3")
            {
                Console.WriteLine("Unesite redni broj specijalnosti za koju zelite da vidite tretmane:\n" +
                                  "Pedijatan, ginekolog, dermatolog, kardiolog, endokrinolog, gastroentrolog, neurolog, onkolog, radiolog, urinolog");
                string specialization = Console.ReadLine();
                int valueSpecialization = int.Parse(specialization) + 1;
                foreach (Doctor.PerformAppointment.Report report in reports)
                {
                    DoctorSpecialization doctorsSpecialization =
                        Doctor.Doctor.DoctorsSpecialization(report.Appointment.Doctor);
                    if (doctorsSpecialization.Equals(valueSpecialization))
                    {
                        Console.WriteLine("Karton: " + report.MedicalRecord + " termin: " + report.Appointment +
                                          "\n opis: " + report.Description);
                    }
                }
            }
        }
        
        public void DoctorOverview()
        {
            List<DoctorsGrade> doctorsGrades = DoctorsGrade.DeserializeDoctorsGrade();
            Console.WriteLine(
                "Unesite kriterijum po kome zelite pretragu doktora:\n1 Po imenu\n2 Po prezimenu\n3 Po specijalnosti\n 4 Svi doktori");
            string criteria = Console.ReadLine();
            List<Doctor.Doctor> doctorsMatchingCriteria = Doctor.Doctor.DoctorMatchingCriteria(criteria);
            Console.WriteLine("Ukoliko zelite da sortirate doktore po prosecnoj oceni unesite 1:");
            string sort = Console.ReadLine();
            if (sort == "1")
            {
                Doctor.Doctor.SortDoctor(ref doctorsMatchingCriteria,0,doctorsMatchingCriteria.Count - 1);
                //sortiranje doktora po oceni    
            }

            int i = 1;
            foreach (Doctor.Doctor doctor in doctorsMatchingCriteria)
            {
                Console.WriteLine(i + " : " +doctor);
                i++;
            }
            Console.WriteLine("Ukoliko zelite da zakazete termin kod nekog doktora unesite redni broj ispred njegovog imena u suprotnom unesite ne: ");
            string doctorsIndexString = Console.ReadLine();
            if (doctorsIndexString != "ne")
            {
                int doctorsIndex = Int32.Parse(doctorsIndexString) - 1;
                WritingAppointment(doctorsMatchingCriteria[doctorsIndex].Username);
            }
            
        }

        public void ChangeOfNotificationTime()
        {
            Console.WriteLine("Unesite koliko sati pre pijenja leka treba da vam stigne notifikacija:");
            string hoursForNotificationString = Console.ReadLine();
            this.hoursForNotification = Int32.Parse(hoursForNotificationString); 
        }
        
        public void Notification_system()
        {
            List<Doctor.PrescribeMedication.Prescription> prescriptions = Doctor.PrescribeMedication.Prescription.DeserializePrescription();
            foreach(Doctor.PrescribeMedication.Prescription prescription in prescriptions)
            {
                if (this.Username == prescription.Patient)
                {
                    foreach(Doctor.PrescribeMedication.Medication medication in prescription.Medications)
                    {
                        if(medication.WhenToConsume == Doctor.PrescribeMedication.TimeForMedicine.AfterTheMeal)
                        {
                            DateTime time = DateTime.Now;
                            TimeSpan hoursBeforeTaking = new TimeSpan( this.hoursForNotification , 0 , 0 );
                            if ((time.Add(hoursBeforeTaking).Hour > 8 && 8 > time.Subtract(hoursBeforeTaking).Hour) || (time.Add(hoursBeforeTaking).Hour > 13 && 13 > time.Subtract(hoursBeforeTaking).Hour) || (time.Add(hoursBeforeTaking).Hour > 18 &&  18> time.Subtract(hoursBeforeTaking).Hour))
                            {
                                Console.WriteLine("Ne zaboravite da uzmete lek nakon obroka!");
                            }
                        }
                        if (medication.WhenToConsume == Doctor.PrescribeMedication.TimeForMedicine.DuringTheMeal)
                        {
                            DateTime time = DateTime.Now;
                            TimeSpan hoursBeforeTaking = new TimeSpan(this.hoursForNotification, 0, 0);
                            if ((time.Add(hoursBeforeTaking).Hour > 8 && 8 > time.Subtract(hoursBeforeTaking).Hour) || (time.Add(hoursBeforeTaking).Hour > 13 && 13 > time.Subtract(hoursBeforeTaking).Hour) || (time.Add(hoursBeforeTaking).Hour > 18 && 18 > time.Subtract(hoursBeforeTaking).Hour))
                            {
                                Console.WriteLine("Ne zaboravite da uzmete lek u toku obroka!");
                            }
                        }
                        if (medication.WhenToConsume == Doctor.PrescribeMedication.TimeForMedicine.BeforeTheMeal)
                        {
                            DateTime time = DateTime.Now;
                            TimeSpan hoursBeforeTaking = new TimeSpan(this.hoursForNotification, 0, 0);
                            if ((time.Add(hoursBeforeTaking).Hour > 8 && 8 > time.Subtract(hoursBeforeTaking).Hour) || (time.Add(hoursBeforeTaking).Hour > 13 && 13 > time.Subtract(hoursBeforeTaking).Hour) || (time.Add(hoursBeforeTaking).Hour > 18 && 18 > time.Subtract(hoursBeforeTaking).Hour))
                            {
                                Console.WriteLine("Ne zaboravite da uzmete lek pre obroka!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("");
                        }
                    }
                }

            }

        }

        public void GradingHospital()
        {
            Console.WriteLine("Sve osim komentara ocenjujete brojevima od 1 do 5:");
            Console.WriteLine("Ocenite kvalitet bolnice:");
            string howGoodHospitalWas = Console.ReadLine();
            Console.WriteLine("Koliko ste zadovoljni uslugom bolnice:");
            string howSatisfiedAreYou = Console.ReadLine(); 
            Console.WriteLine("Ocenite koliko je bolnica cista:");
            string howCleanItIs = Console.ReadLine();
            Console.WriteLine("Da li bi ste preporucili prijatelju usluge bolnice:");
            string wouldYouSuggest = Console.ReadLine();
            Console.WriteLine("Unesite vas komentar o bolnici:");
            string comment = Console.ReadLine();
            HospitalGrade grade = new HospitalGrade(comment,Int32.Parse(wouldYouSuggest), Int32.Parse(howGoodHospitalWas), Int32.Parse(howSatisfiedAreYou), Int32.Parse(howCleanItIs));
            grade.SerializeDoctorsGrade();
        }

        public bool canUserGradeDoctor(string doctor)
        {
            List<CreatingAppointment.Appointment> usersAppointments = CreatingAppointment.Appointment.UsersAppointments(this.Username);
            foreach (CreatingAppointment.Appointment appointment in usersAppointments)
            {
                DateTime dateForCheck = CreatingAppointment.Appointment.stringToDateTime(appointment.TimeOfAppointment);
                DateTime now = DateTime.Now;
                if (dateForCheck.CompareTo(now) < 0 && doctor == appointment.Doctor)
                    return true;
            }
            return false;
        }

        public void GradingDoctor()
        {
            Console.WriteLine("Mozete oceniti doktore samo kod kojih ste vec imali tretman\nSve osim komentara ocenjujete brojevima od 1 do 5:");
            Console.WriteLine("Unesite ime doktora:");
            CreatingAppointment.Appointment.printingAppointmentForUser(this.Username);
            string doctor = Console.ReadLine();
            bool validation = canUserGradeDoctor(doctor);
            if (validation)
            {
                Console.WriteLine("Koliko ste zadovoljni uslugom doktora:");
                string howGoodDoctorWas = Console.ReadLine();
                Console.WriteLine("Da li bi ste preporucili prijatelju usluge doktora:");
                string wouldYouSuggest = Console.ReadLine();
                Console.WriteLine("Unesite vas komentar o doktoru:");
                string comment = Console.ReadLine();
                DoctorsGrade grade = new DoctorsGrade(comment, Int32.Parse(wouldYouSuggest), Int32.Parse(howGoodDoctorWas), doctor);
                grade.SerializeDoctorsGrade();
            }
            else
            {
                Console.WriteLine("Ne mozete oceniti doktora kod koga niste ni imali tretman!");
            }
        }

        public void patientMenu()
        {
            string option;
            this.Notification_system();
            while (true)
            {

                Console.WriteLine("Izaberite opciju koju zelite da izaberete:\n1 Zakazivanje termina\n2 Izmena termina\n3 Brisanje termina\n4 Prikaz termina\n5 Pomoc pri zakazivanju termina\n6 Pregled izvestaja\n7 Pregled doktora\n8 Birajte broj sati koliko pre jela zelite da dobijete notifikaciju o uzimanju leka\n9 Ocenjivanje bolnice\n10 Ocenjivanje doktora\n11 Izalazak iz menua");
                option = Console.ReadLine();

                if (option == "11")
                    break;

                if (option == "1")
                    this.MakingAppointment();
                else if (option == "2")
                    this.changingAppointment();
                else if (option == "3")
                    this.deletingAppointment();
                else if (option == "4")
                    CreatingAppointment.Appointment.printingAppointmentForUser(this.Username);
                else if (option == "5")
                    this.AppointmentRecommendation();
                else if (option == "6")
                    this.ReportsOverview();
                else if (option == "7")
                    this.DoctorOverview();
                else if (option == "8")
                    this.ChangeOfNotificationTime();
                else if (option == "9")
                    this.GradingHospital();
                else if (option == "10")
                    this.GradingDoctor();
            }
        }
    }
}
