using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthCare.Patient;
using HealthCare.Patient.Grading;
using HealthCare.Secretary;

namespace HealthCare.Doctor
{
    class Doctor : User
    {
        private string name;
        private string surname;
        private List<Patient.CreatingAppointment.Appointment>? appointments;
        private string roomId;
        private DoctorSpecialization specialization;

        public Doctor() {
            username = "";
            password = "";
            appointments = new List<Patient.CreatingAppointment.Appointment>();
            name = "";
            surname  = "";
            roomId = "";
            specialization = DoctorSpecialization.Pediatrician;
        }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }

        public string RoomId { get => roomId; set => roomId = value; }

        public List<Patient.CreatingAppointment.Appointment> Appointments { get => appointments; set => appointments = value; }
        
        public DoctorSpecialization Specialization
        {
            get => specialization;
            set => specialization = value;
        }
        [JsonConstructor]
        public Doctor(string username, string password, string name, string surname, List<Patient.CreatingAppointment.Appointment> appointments, string roomId, DoctorSpecialization specialization)
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
        SecretaryService secretaryService = new SecretaryService();
        public static DoctorSpecialization DoctorsSpecialization(string username)
        {
            DoctorSpecialization doctorSpecialization = 0;
            List<Doctor> doctors = Deserialize();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Username == username)
                    doctorSpecialization = doctor.Specialization;
            }
            return doctorSpecialization;
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

        public static List<Doctor> DoctorMatchingCriteria(string criteria)
        {
            List<Doctor> doctors = Doctor.Deserialize();
            List<Doctor> doctorsMatching = new List<Doctor>();
            if (criteria == "1")
            {
                Console.WriteLine("Unesite ime doktora koje zelite da pretrazite:");
                string doctorsName = Console.ReadLine();
                foreach (Doctor doctor in doctors)
                {
                    if (doctor.Name == doctorsName)
                        doctorsMatching.Add(doctor);
                }
            }

            if (criteria == "2")
            {
                Console.WriteLine("Unesite prezime doktora koje zelite da pretrazite:");
                string doctorsLastname = Console.ReadLine();
                foreach (Doctor doctor in doctors)
                {
                    if (doctor.Surname == doctorsLastname)
                        doctorsMatching.Add(doctor);
                }
            }

            if (criteria == "3")
            {
                Console.WriteLine("Unesite specijalizaciju doktora koje zelite da pretrazite:");
                string doctorsSpecializationString = Console.ReadLine();
                int doctorsSpecialization = Int32.Parse(doctorsSpecializationString);
                foreach (Doctor doctor in doctors)
                {
                    if (doctor.Specialization.Equals(doctorsSpecialization))
                        doctorsMatching.Add(doctor);
                }
            }

            if (criteria == "4")
            {
                doctorsMatching = doctors;
            }
            return doctorsMatching;
        }
        
        private bool CreateAppointment()
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
                this.appointments.Add(new Patient.CreatingAppointment.Appointment(this.username, patient, period, (PerformAppointment.AppointmentType)Int32.Parse(type)-1, roomId));
            }
            else
            {
                Console.WriteLine("Unesite sobu za odrzavanje operacije: ");
                string operationRoom = Console.ReadLine();
                this.appointments.Add(new Patient.CreatingAppointment.Appointment(this.username, patient, period, (PerformAppointment.AppointmentType)Int32.Parse(type)-1, operationRoom));
                
            }
            
            return true;
        }
        
        private void readAppointments()
        {
            foreach (Patient.CreatingAppointment.Appointment a in appointments)
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
                    foreach (Patient.CreatingAppointment.Appointment a in d.appointments)
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
                    secretaryService.ViewMedicalRecord(patient.MedicalRecord);
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

        private PerformAppointment.Referral createRefferal(string? doctor, string pacient, DoctorSpecialization? specialization)
        {
            if (doctor != null)
            {
                return new PerformAppointment.Referral(doctor, pacient);
            }
            else
            {
                string referredDoctor = findDoctorInField(specialization.GetValueOrDefault()).Username;
                return new PerformAppointment.Referral(referredDoctor, pacient);
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
                    // pacient.MedicalRecord.Referral = createRefferal(null, pacient.Username, (DoctorSpecialization)x) when referral is added to medicalrecord
                }
            }
            else
            {
                Console.WriteLine(s + " nije ponunjena opcija");
                Console.WriteLine("Izlazak..");
                return;
            }
        }
        private void performAppointmentMenu(Manager manager)
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
            secretaryService.ViewMedicalRecord(patient.MedicalRecord);
            Console.WriteLine("Da li zelite da izmenite nesto iz zdravstvenog kartona(y/n): ");
            string s = Console.ReadLine();
            if (s == "y")
            {
                secretaryService.CreateInput();
            }
            Console.WriteLine("Anamneza: ");
            string anamneza = Console.ReadLine();
            PerformAppointment.Report.addReport(new PerformAppointment.Report(appointments[index], anamneza, patient.MedicalRecord));
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

            string roomId = this.appointments[index].RoomId;
            this.manageEquipment(manager, roomId);
            this.appointments.RemoveAt(index);
        }

        private void manageEquipment(Manager manager, string roomId)
        {
            Room appointmentRoom = manager.GetRoom(roomId);
            List<Equipment> equipments = appointmentRoom.EquipmentList;
            Console.WriteLine("==============================");
            Console.WriteLine("Azuriranje opreme: ");
            while (equipments.Count > 0)
            {
                for (int i = 0; i < equipments.Count; i ++)
                {
                    var equipment = equipments[i];
                    Console.WriteLine("");
                    Console.WriteLine(i + 1 + ".");
                    Console.WriteLine("Oprema: " + equipment.Name);
                    Console.WriteLine("Tip opreme: " + equipment.EquipmentType);
                    Console.WriteLine("Kolicina: " + equipment.Amount);
                    Console.WriteLine("");
                }
                Console.WriteLine("Izaberite opremu za azuriranje: ");
                string s = Console.ReadLine();
                int index = Int32.Parse(s)-1;
                if (s == "" || index >= equipments.Count || index < 0)
                {
                    Console.WriteLine("Pogresan unos!");
                    return;
                }
                Console.WriteLine("Izabrana oprema: ");
                var chosenEquipment = equipments[index];
                Console.WriteLine("Oprema: " + chosenEquipment.Name);
                Console.WriteLine("Tip opreme: " + chosenEquipment.EquipmentType);
                Console.WriteLine("Kolicina: " + chosenEquipment.Amount);
                Console.WriteLine("Unesite kolicinu opreme koja je potrosena: ");
                s = Console.ReadLine();
                int amountUsed = Int32.Parse(s);
                if (amountUsed > chosenEquipment.Amount)
                {
                    Console.WriteLine("Greska uneta kolicina je veca od ukupne kolicine dostupne opreme!");
                    return;
                }
                appointmentRoom.EquipmentList[index].Amount -= amountUsed;
                Console.WriteLine("Nastaviti azuriranje opreme? (y/n)");
                s = Console.ReadLine();
                if (s == "n")
                {
                    return;
                }
            }

        }

        public static void addAppointmentForDoctor(Patient.CreatingAppointment.Appointment appointment, string doctorusername)
        {
            List<Doctor> doctors = Deserialize();
            foreach (var doctor in doctors)
            {
                if (doctorusername == doctor.Username)
                {
                    doctor.appointments.Add(appointment);
                    return;
                }
            }
        }
        private void prescribeMedicineMenu(Patient.Patient patient)
        {
            PrescribeMedication.Prescription prescription = new PrescribeMedication.Prescription();
            List<PrescribeMedication.Medication> availableMedications = PrescribeMedication.Medication.Deserialize();
            while (availableMedications.Count > 0)
            {
                for (int i = 0; i < availableMedications.Count; i++)
                {
                    PrescribeMedication.Medication medication = availableMedications[i];
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
        public void DoctorMenu(Manager manager)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenuWrite(manager);
            }
        }
        

        private void manageMedicationMenu()
        {
            List<PrescribeMedication.Medication> suggestions = PrescribeMedication.Medication.DeserializeSuggestions();
            if (suggestions.Count == 0)
            {
                Console.WriteLine("Trenutno nema sugestija lekova za pregled");
                return;
            }
            while (suggestions.Count > 0)
            {
                for (int i = 0; i < suggestions.Count; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine(i+1 +".");
                    Console.WriteLine("Sugestija za " + suggestions[i]);
                }
                Console.WriteLine("Izaberite sugestiju leka: ");
                string s = Console.ReadLine();
                var index = Int32.Parse(s)-1;
                if (index < 0 || index >= suggestions.Count)
                {
                    Console.WriteLine("Pogresan unos!");
                    return;
                }
                Console.WriteLine("Izabran " + suggestions[index]);
                Console.WriteLine("Da li prihvatate lek?(y/n)");
                Console.WriteLine("Izaberite opciju: ");
                s = Console.ReadLine();
                if (s == "y")
                {
                    PrescribeMedication.Medication.addMedication(suggestions[index]);
                    suggestions.RemoveAt(index);
                }else if (s == "n")
                {
                    Console.Write("Unesite razlog za odbijanje: ");
                    string reasonForDenial = Console.ReadLine();
                    suggestions[index].DoctorNote = reasonForDenial;
                    PrescribeMedication.Medication.AddDeniedSuggestion(suggestions[index]);
                    suggestions.RemoveAt(index);
                }
                else
                {
                    Console.WriteLine("Greska pri unosu");
                }
                Console.WriteLine("Nastaviti pregled sugestija?(y/n)");
                s = Console.ReadLine();
                if (s == "y")
                {
                    continue;
                }else if (s == "n")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Greska pri unosu");
                    return;
                }
            }
            PrescribeMedication.Medication.SerializeSuggestions(suggestions);
                

            
        }

        private bool checkAppointmentsInSpan(DateTime start, DateTime end)
        {
            foreach (var appointment in appointments)
            {
                DateTime currentAppointmentDate =
                    DateTime.ParseExact(appointment.TimeOfAppointment, "dd/MM/yyyy HH:mm", null);
                if (currentAppointmentDate > start && currentAppointmentDate < end)
                {
                    return false;
                }
            }

            return true;
        }

        private void checkNotifications()
        {
            List<DaysOffRequest> deserizaliedRequests = DaysOffRequest.Deserialize();
            List<DaysOffRequest> requestsToSerialize = DaysOffRequest.Deserialize();
            
            for (int i = 0; i < deserizaliedRequests.Count; i++)
            {
                DaysOffRequest deserizaliedRequest = deserizaliedRequests[i];
                if (deserizaliedRequest.DoctorName == username &&
                    deserizaliedRequest.State != RequestState.AwaitingDecision)
                {
                    if (deserizaliedRequest.State == RequestState.Accepted)
                    {
                        Console.WriteLine("Prihvacen vam je " + deserizaliedRequest + " za slobodne dane!");
                    }

                    if (deserizaliedRequest.State == RequestState.Denied)
                    {
                        Console.WriteLine("Odbijen vam je " + deserizaliedRequest + " za slobodne dane!");
                    }

                    requestsToSerialize.RemoveAt(i);
                }
            }
            DaysOffRequest.Serialize(requestsToSerialize);
        }
        private bool urgentInput()
        {
            Console.WriteLine("Da li je zahtev hitan(y/n)?");
            Console.WriteLine("(hitni zahtevi mogu trajati najduze 5 dana)");
            string urgentInput = Console.ReadLine();
            bool urgent = urgentInput == "y";
            return urgent;
        }

        private DateTime dateInput()
        {
            Console.WriteLine("Unesite datum za pocetak slobodnih dana( mora biti bar 2 dana unapred)(format dd/MM/yyyy)");
            string beginningDateInput = Console.ReadLine();
            if ((DateTime.ParseExact(beginningDateInput, "dd/MM/yyyy", null) - DateTime.Today).TotalDays < 2 || DateTime.ParseExact(beginningDateInput, "dd/MM/yyyy", null) < DateTime.Today)
            {
                Console.WriteLine("Zahtev mora biti bar 2 dana ubuduće");
            }
            DateTime beginningDate = DateTime.ParseExact(beginningDateInput, "dd/MM/yyyy", null);
            return beginningDate;
        }
        private DaysOffRequest daysOffRequestMenu()
        {
            bool urgent = urgentInput();
            DateTime beginningDate = dateInput();
            Console.WriteLine("Unesite datum za kraj slobodnih dana( mora biti bar 2 dana unapred)(format dd/MM/yyyy)");
            string endDateInput = Console.ReadLine();
            DateTime endDate = DateTime.ParseExact(endDateInput, "dd/MM/yyyy", null);
            if (urgent && (endDate - beginningDate).TotalDays > 5)
            {
                Console.WriteLine("Hitni zahtevi ne mogu biti duzi od 5 dana");
                return null;
            }
            Console.WriteLine("Unesite razlog zbog kog trazite slobodan dan: ");
            string reason = Console.ReadLine();
            if (checkAppointmentsInSpan(beginningDate, endDate))
            {
                return new DaysOffRequest(beginningDate, endDate, urgent, reason, RequestState.AwaitingDecision, username);
            }
            Console.WriteLine("Zauzeti ste u tom periodu!");
            return null;
        }

        private void previewRequests()
        {
            List<DaysOffRequest> deserializedRequests = DaysOffRequest.Deserialize();
            foreach (var deserializedRequest in deserializedRequests)
            {
                if (username == deserializedRequest.DoctorName)
                {
                    Console.WriteLine("============================");
                    Console.WriteLine(deserializedRequest);
                    Console.WriteLine("============================");
                }
            }
        }
        private void requestDaysOff()
        {
            Console.WriteLine("==============================");
            Console.WriteLine("1. Pregled zahteva");
            Console.WriteLine("2. Novi zahtev");
            string choice = Console.ReadLine();
           
            if (choice == "1")
            {
                previewRequests();
                return;
            }if (choice != "2")
            {
                Console.WriteLine(choice + " nije validna opcija");
                return;

            }
            DaysOffRequest request = daysOffRequestMenu();
            if (request == null)
            {
                return;
            }
            Console.WriteLine("Uspesno podnesen zahtev za period od " + request.VacationStart + " do " + request.VacationEnd);
            if (!request.IsUrgent)
            {
                DaysOffRequest.AddRequest(request);
            }
            Console.WriteLine("==============================");
        }
        private void MainMenuPrint()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("1. CRUD pregled/operaciju");
            Console.WriteLine("2. Prikaz rasporeda");
            Console.WriteLine("3. Izvodjenje pregleda/operacije");
            Console.WriteLine("4. Upravljanje lekovima");
            Console.WriteLine("5. Zahtev za slobodan dan");
            Console.WriteLine("6. Exit");
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
        private bool MainMenuWrite(Manager manager)
        {
            checkNotifications();
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
                    performAppointmentMenu(manager);
                    return true;
                case "4":
                    manageMedicationMenu();
                    return true;
                case "5":
                    requestDaysOff();
                    return true;
                case "6":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos!\n");
                    return true;
                    
            }

        }
        

        public float DoctorsAvgGrade()
        {
            List<DoctorsGrade> grades = DoctorsGrade.DeserializeDoctorsGrade();
            int counter = 0;
            int sum = 0;
            foreach (DoctorsGrade grade in grades)
            {
                if (grade.Doctor == this.Username)
                {
                    counter++;
                    sum += grade.HowGoodDoctorWas;
                }
                    
            }
            return sum/counter;
        }
        
        public static void MergeDoctor(ref List<Doctor> doctors , int l, int m, int r)
                {
                    int n1 = m - l + 1;
                    int n2 = r - m;
            
                    List<Doctor> L = new List<Doctor>( new Doctor[n1]);
                    List<Doctor> R = new List<Doctor>( new Doctor[n2]);
                    int i, j;
          
                    for (i = 0; i < n1; ++i)
                        L[i] = doctors[l + i];
                    for (j = 0; j < n2; ++j)
                        R[j] = doctors[m + 1 + j];
          
                    i = 0;
                    j = 0;
          
                    int k = l;
                    while (i < n1 && j < n2) {
                        if (L[i].DoctorsAvgGrade() <= R[i].DoctorsAvgGrade()) 
                        {
                            doctors[k] = L[i];
                            i++;
                        }
                        else {
                            doctors[k] = R[j];
                            j++;
                        }
                        k++;
                    }
          
                    while (i < n1) {
                        doctors[k] = L[i];
                        i++;
                        k++;
                    }
              
                    while (j < n2) {
                        doctors[k] = R[j];
                        j++;
                        k++;
                    }
                }
                
                        
        
        public static void SortDoctor(ref List<Doctor> doctors, int l, int r)
        {
            if (l < r) {
                int m = l+ (r-l)/2;
            
                SortDoctor(ref(doctors), l, m);
                SortDoctor(ref(doctors), m + 1, r);
                    
                MergeDoctor(ref(doctors), l, m, r);
            }
        }
    }
    
}
