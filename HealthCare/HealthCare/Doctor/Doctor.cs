using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCare.Doctor
{
    class Doctor : User
    {
        private string name;
        private string surname;
        private List<Patient.Appointment>? appointments;
        private string roomId;
        private DoctorSpecialization specialization;

        public Doctor() {
            username = "";
            password = "";
            appointments = new List<Patient.Appointment>();
            name = "";
            surname  = "";
            roomId = "";
            specialization = DoctorSpecialization.Pediatrician;
        }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }

        public string RoomId { get => roomId; set => roomId = value; }

        public List<Patient.Appointment> Appointments { get => appointments; set => appointments = value; }
        
        public DoctorSpecialization Specialization
        {
            get => specialization;
            set => specialization = value;
        }

        // [JsonConstructor]
        // public Doctor(string username, string password,string name, string surname,List<Patient.Appointment> appointments)
        // {
        //     this.username = username;
        //     this.password = password;
        //     this.name = name;
        //     this.surname = surname;
        //     this.appointments = appointments;
        // }

        [JsonConstructor]
        public Doctor(string username, string password, string name, string surname, List<Patient.Appointment> appointments, string roomId, DoctorSpecialization specialization)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.appointments = appointments;
            this.roomId = roomId;
            this.specialization = specialization;
        }

        public override string ToString()
        {
            return String.Format("Doctor( Name: {0}, Surname: {1}, Username: {2}, Password: {3}, Appointments: [{4}])", name, surname, username, password, String.Join("; ",appointments));
        }


        public static string DoctorsRoom(string username)
        {
            string roomName = "";
            List<Doctor> doctors = Deserialize();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Username == username)
                    roomName = doctor.RoomId;
            }
            return roomName;
        }

        public void AddAppointment(Patient.Appointment appointment)
        {
            this.appointments.Add(appointment);
        }
        public bool CreateAppointment()
        {
            Console.Write("Unesite korisnicko ime pacijenta:  ");
            string patient = Console.ReadLine();
            if (patient == "") 
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            Console.Write("Unesite datum i vreme pregleda pregleda(format = dd/MM/yyyy HH:mm):  ");
            string period = Console.ReadLine();
            DateTime dt;
            if (period == "" || !DateTime.TryParseExact(period, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            if (checkIfAvailable(DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null)))
            {
                Console.WriteLine("Doktor nije dostupan za dat termin.");
                return false;
            }
            Console.WriteLine("Izaberite tip termina:");
            Console.WriteLine("1. Operacija");
            Console.WriteLine("2. Pregled");
            string type = Console.ReadLine();
            if (type != "1" || type != "2")
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }

            if (type == "2")
            {
                this.appointments.Add(new Patient.Appointment(this.username, patient, period, (AppointmentType)Int32.Parse(type)-1, roomId));
            }
            else
            {
                Console.WriteLine("Unesite sobu za odrzavanje operacije: ");
                string operationRoom = Console.ReadLine();
                this.appointments.Add(new Patient.Appointment(this.username, patient, period, (AppointmentType)Int32.Parse(type)-1, operationRoom));
                
            }
            
            return true;
        }
        
        public void readAppointments()
        {
            foreach (Patient.Appointment a in appointments)
            {
                Console.WriteLine(a);
            }
        }
        public static void deleteAppointment(string patient, string doctor, string date)
        {
            List<Doctor> deserializedDoctors = Deserialize();
            foreach (Doctor d in deserializedDoctors)
            {
                if (d.username == doctor)
                {
                    foreach (Patient.Appointment a in d.appointments)
                    {
                        if (a.Patient == patient && a.TimeOfAppointment == date)
                        {
                            d.appointments.Remove(a);
                            Serialize(deserializedDoctors);
                            return;
                        }
                    }
                }
            }
            
        }

        private void updateAppointment(string change, string choice, int index)
        {
            if (choice == "date")
            {
                appointments[index].TimeOfAppointment = change;
            }

            if (choice == "patient")
            {
                appointments[index].Patient = change;
            }
        }
        private void printSchedule(DateTime? chosenDate)
        {
            if (!chosenDate.HasValue)
            {
                chosenDate = DateTime.Today;
            }
            
            for (int i = 0;i < appointments.Count;i++)
            {
                DateTime dt = DateTime.ParseExact(appointments[i].TimeOfAppointment, "dd/MM/yyyy HH:mm", null);
                if ((dt - chosenDate) < TimeSpan.FromDays(4) && dt > chosenDate)
                {
                    List<Patient.Patient> patients = Patient.Patient.patientDeserialization();
                    Patient.Patient patient = new Patient.Patient();
                    foreach (Patient.Patient p in patients)
                    {
                        if (appointments[i].Patient == p.Username)
                        {
                            patient = p;
                            break;
                        }
                    }
                    Console.WriteLine("Pregled " + i+1 + ". ");
                    Console.Write("Datum pregleda/operacije: ");
                    Console.WriteLine(dt);
                    Console.Write("Pacijent: ");
                    Console.WriteLine(appointments[i].Patient); 
                    Console.WriteLine("Zdravstveni karton pacijenta: ");
                    patient.MedicalRecord.ViewMedicalRecord(patient.MedicalRecord);
                }
            }
            
        }
        private bool checkIfAvailable(DateTime appointmentDate)
        {
            foreach (var a in appointments)
            {
                DateTime dt = DateTime.ParseExact(a.TimeOfAppointment, "dd/MM/yyyy HH:mm", null);
                if ((dt - appointmentDate).TotalMinutes < 15)
                {
                    return false;
                }
            }
            return true;
        }
        public static List<Doctor> Deserialize()
        {
            string path = "../../../Data/DoctorsData.json";
            string jsonText = File.ReadAllText(path);
            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(jsonText);
            return doctors;
        }


        public static void Serialize(List<Doctor> doctors)
        {
            File.WriteAllText("../../../Data/DoctorsData.json", JsonSerializer.Serialize(doctors));
        }

        
        private int? chooseAppointment()
        {
            for (int i = 0; i< appointments.Count; i++)
            {
                Console.WriteLine(i+1 + ". " + appointments[i]);
            }
            Console.WriteLine("Izaberite termin: ");
            string index = Console.ReadLine();
            int x = 0;
            
            if (!Int32.TryParse(index, out x))
            {
                Console.WriteLine("Neispravan unos.");
                return null;
            }
            if (x <= 0 || x > appointments.Count)
            {
                Console.WriteLine(x+ " nije ponudjena opcija. ");
                return null;
            }

            return x;
        }
        private void updateAppointmentMenu()
        {
            int? x = chooseAppointment();
            int index;
            if (!x.HasValue)
            {
                return;
            }
            else
            {
                index = x.Value-1;
            }
            
            Console.WriteLine("Izmena termina: ");
            Console.WriteLine("1. izmena datuma i vremena");
            Console.WriteLine("2. izmena pacijenta");
            Console.Write("Izaberite opciju za menjanje: ");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("Unesite novi datum i vreme termina( format dd/MM/yyyy HH:mm): ");
                string newDate = Console.ReadLine();
                DateTime dt;
                if (!DateTime.TryParseExact(newDate, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out dt))
                {
                    Console.WriteLine("Datum"+newDate+" nije validan.");
                    return;
                }
                if (!checkIfAvailable(dt))
                {
                    Console.WriteLine("Nemate slobodan termin za dato vreme.");
                    return;
                }
                updateAppointment(newDate, "date", index);
            }
            else if (choice == "2")
            {
                Console.WriteLine("Unesite korisnicko ime novog pacijenta: ");
                string newPatient = Console.ReadLine();
                if (newPatient == "")
                {
                    Console.WriteLine("Neispravan unos.");
                    return;
                }
                updateAppointment(newPatient, "patient", index);
            }
        }
        private void deleteAppointmentMenu()
        {
            Console.Write("Unesite korisnicko ime pacijenta: ");
            string chosenPatient = Console.ReadLine();
            Console.Write("Unesite datum i vreme pregleda/operacije za brisanje( format Hdd/MM/yyyy HH:mm): ");
            string chosenDate = Console.ReadLine();
            deleteAppointment(chosenPatient, this.username, chosenDate);
        }

        private Referral createRefferal(string? doctor, string pacient, DoctorSpecialization? specialization)
        {
            if (doctor != null)
            {
                return new Referral(doctor, pacient);
            }
            else
            {
                string referredDoctor = findDoctorInField(specialization.GetValueOrDefault()).Username;
                return new Referral(referredDoctor, pacient);
            }
        }

        private static Doctor findDoctorInField(DoctorSpecialization specialization)
        {
            List<Doctor> allDoctors = Doctor.Deserialize();
            List<Doctor> doctorsInField = new List<Doctor>();
            foreach (var d in allDoctors)
            {
                if (d.specialization == specialization)
                {
                    doctorsInField.Add(d);
                }
            }

            Random random = new Random();
            int index = random.Next(doctorsInField.Count);
            return doctorsInField[index];
        }
        private void referralMenu(Patient.Patient pacient)
        {
            Console.WriteLine("Izaberite opciju: ");
            Console.WriteLine("1. Uput ka doktoru opste prakse");
            Console.WriteLine("2. Uput ka doktoru specijaliste");
            string s = Console.ReadLine();
            if (s == "1")
            {
                Console.WriteLine("Unesite korisnicko ime zeljenog doktora: ");
                string referredDoctor = Console.ReadLine();
                if (referredDoctor != "")
                {
                    //TODO pacient.MedicalRecord.Referral = createRefferal(null, pacient.Username, (DoctorSpecialization)x) when referral is added to medicalrecord
                }
                else
                {
                    Console.WriteLine("Greska pri unosu!");
                    Console.WriteLine("Izlazak..");
                    return;
                }
            }
            else if (s == "2")
            {
                
                Console.WriteLine("Izaberite specijalistu za uput: ");
                Console.WriteLine("1. Ginekolog");
                Console.WriteLine("2. Dermatolog");
                Console.WriteLine("3. Kardiolog");
                Console.WriteLine("4. Endokrinolog ");
                Console.WriteLine("5. Gastroenterolog");
                Console.WriteLine("6. Neurolog");
                Console.WriteLine("7. Onkolog");
                Console.WriteLine("8. Radiolog");
                Console.WriteLine("9. Urolog");
                s = Console.ReadLine();
                int x = 0;
            
                if (!Int32.TryParse(s, out x))
                {
                    Console.WriteLine("Neispravan unos.");
                    return;
                }

                if (x < 1 || x > Enum.GetNames(typeof(DoctorSpecialization)).Length - 1)
                {
                    Console.WriteLine(s + " nije ponudjena opcija!");
                    return;
                }
                else
                {
                    //TODO pacient.MedicalRecord.Referral = createRefferal(null, pacient.Username, (DoctorSpecialization)x) when referral is added to medicalrecord
                }
            }
            else
            {
                Console.WriteLine(s + " nije ponunjena opcija");
                Console.WriteLine("Izlazak..");
                return;
            }
        }
        private void performAppointmentMenu()
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("Izvodjenje pregleda: ");
            int? x = chooseAppointment();
            if (!x.HasValue)
            {
                return;
            }
            int index = x.Value-1;
            Console.WriteLine(appointments[index]);
            List<Patient.Patient> deserializedPatients = Patient.Patient.patientDeserialization();
            Patient.Patient patient = new Patient.Patient();
            foreach (Patient.Patient p in deserializedPatients)
            {
                if (appointments[index].Patient == p.Username)
                {
                    patient = p;
                    break;
                }
            }
            Console.WriteLine("Zdravstveni karton pacijenta: ");
            patient.MedicalRecord.ViewMedicalRecord(patient.MedicalRecord);
            Console.WriteLine("Da li zelite da izmenite nesto iz zdravstvenog kartona(y/n): ");
            string s = Console.ReadLine();
            if (s == "y")
            {
                patient.MedicalRecord.CreateInput();
            }
            Console.WriteLine("Anamneza: ");
            string anamneza = Console.ReadLine();
            Report.addReport(new Report(appointments[index], anamneza, patient.MedicalRecord));
            if (this.Specialization == DoctorSpecialization.Pediatrician)
            {
                Console.WriteLine("Da li zelite da uputite pacijenta nekome? (y/n)");
                s = Console.ReadLine();
                if (s == "y")
                {
                    referralMenu(patient);
                    //TODO patient.serializePatient() when referral is added to medicalrecord
                }
            }
            Console.WriteLine("Da li zelite da izdate recept?(y/n)");
            s = Console.ReadLine();
            if (s == "y")
            {
                prescribeMedicineMenu(patient);
            }

        }

        public void prescribeMedicineMenu(Patient.Patient patient)
        {
            Prescription prescription = new Prescription();
            List<Medication> availableMedications = Medication.Deserialize();
            while (availableMedications.Count > 0)
            {
                for (int i = 0; i < availableMedications.Count; i++)
                {
                    Medication medication = availableMedications[i];
                    Console.Write(i + 1 + ". ");
                    Console.WriteLine(medication.Name);
                }

                Console.WriteLine("Izaberite lek za dodavanje receptu: ");
                string chosenMedicine = Console.ReadLine();
                int medicineIndex = Int32.Parse(chosenMedicine);
                if (medicineIndex < 1 || medicineIndex > availableMedications.Count)
                {
                    Console.WriteLine(chosenMedicine + " nije ponudjena opcija!");
                    return;
                }

                prescription.medications.Add(availableMedications[medicineIndex - 1]);
                availableMedications.RemoveAt(medicineIndex-1);
                Console.WriteLine("Dodavanje jos lekova?(y/n)");
                string moreMedicine = Console.ReadLine();
                if (moreMedicine == "n")
                {
                    break;
                }
            }
            prescription.LoadAllergies();
            if (!prescription.CheckPatientAllergies(patient))
            {
                Console.WriteLine("Pacijent je alergican na neki od datih lekova!");
                return;
            }
            Console.WriteLine("Recept za: " + patient.Username);
            prescription.PrintPrescription();
        }

        private void checkScheduleMenu()
        {
            Console.Write("Izaberite datum za prikaz( format dd/MM/yyyy HH:mm) ili 'danas' za danasnji dan: ");
            string chosenDate = Console.ReadLine();
            DateTime dt;
            if (chosenDate == "danas")
            {
                printSchedule(null);
                return;
            }
            if (chosenDate == "" || !DateTime.TryParseExact(chosenDate, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Neodgovarajuc unos");
                return;
            }
            printSchedule(dt);
        }
        public void DoctorMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenuWrite();
            }
        }
        private void MainMenuPrint()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("1. CRUD pregled/operaciju");
            Console.WriteLine("2. Prikaz rasporeda");
            Console.WriteLine("3. Izvodjenje pregleda/operacije");
            Console.WriteLine("4. Exit");
            Console.Write("Izaberite opciju: ");

        }
        private void CRUDMenuPrint()
        {
            Console.WriteLine("\n1. Kreiraj pregled/operaciju");
            Console.WriteLine("2. Prikaz pregleda/operacija");
            Console.WriteLine("3. Izmena pregleda/operacije");
            Console.WriteLine("4. Brisanje pregleda/operaciju");
            Console.WriteLine("5. Vratite se nazad");
            Console.Write("Izaberite opciju: ");
        }
        private void CRUDMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = CRUDMenuWrite();
            }
        }
        private bool CRUDMenuWrite()
        {
            CRUDMenuPrint();
            switch (Console.ReadLine())
            {
                case "1":
                    this.CreateAppointment();
                    return true;
                case "2":
                    this.readAppointments();
                    return true;
                case "3":
                    updateAppointmentMenu();
                    return true;
                case "4":
                    this.deleteAppointmentMenu();
                    return true;
                case "5":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos.\n");
                    return true;
            }
        }
        private bool MainMenuWrite()
        {
            MainMenuPrint();
            switch (Console.ReadLine())
            {
                case "1":
                    CRUDMenu();
                    return true;
                case "2":
                    checkScheduleMenu();
                    return true;
                case "3":
                    performAppointmentMenu();
                    return true;
                case "4":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos!\n");
                    return true;
                    
            }

        }
    }
    
}
