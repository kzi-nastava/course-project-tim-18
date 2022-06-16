

using HealthCare.Doctor;

namespace HealthCare.Secretary
{
    public class MedicalRecord
    {
        private string name;
        private string lastname;
        private string address;
        private string username;
        private string password;
        private string email;
        private string height;
        private string weight;
        private string bloodType;
        private string doktor;
        private List<Doctor.PrescribeMedication.Allergy> allergies;


        public MedicalRecord(string name, string lastname, string address, string username, string password, string email, string height, string weight, string bloodType)
        {
            this.name = name;
            this.lastname = lastname;
            this.address = address;
            this.username = username;
            this.password = password;
            this.email = email;
            this.height = height;
            this.weight = weight;
            this.bloodType = bloodType;

        }
        public MedicalRecord(string name, string lastname, string address, string username, string password, string email, string height, string weight, string bloodType,string doktor)
        {
            this.name = name;
            this.lastname = lastname;
            this.address = address;
            this.username = username;
            this.password = password;
            this.email = email;
            this.height = height;
            this.weight = weight;
            this.bloodType = bloodType;
            this.doktor = doktor;

        }
        
        public MedicalRecord()
        {

        }

        public string Name 
        {
            get { return name; }
            set { name = value; }
        }
        public string Lastname 
        {
            get { return lastname; }
            set { lastname = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }

        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }

        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Height
        {
            get { return height; }
            set { height = value; }
        }
        
        public string Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public string BloodType
        {
            get { return bloodType; }
            set { bloodType = value; }
        }

        public string Doktor
        {
            get { return doktor; }
            set { doktor = value; }
        }

        public List<Doctor.PrescribeMedication.Allergy> Allergies
        {
            get { return allergies; }
            set { allergies = value; }
        }

        public override string ToString()
        {
            return "Ime: " + Name + "\nPrezime: " + Lastname + "\nAdresa: " + Address + "\nKorisnicko ime:" + Username + "\nLozinka: " + Password + "\nEmail: " + Email + "\nVisina: " + Height + "\nTezina: " + Weight+ "\nKrvna grupa: "+ BloodType +"\n";
        }

        
        public void ViewMedicalRecord(MedicalRecord record)
        {
            string[] title = { "Ime", "Prezime", "Adresa","Korisnicko ime","Lozinka","Email","Visina","Tezina","Krvna grupa" };
            string[] population = { record.Name, record.Lastname, record.Address ,record.Username,record.Password,record.Email,record.Height,record.Weight,record.bloodType};
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
    }
}
