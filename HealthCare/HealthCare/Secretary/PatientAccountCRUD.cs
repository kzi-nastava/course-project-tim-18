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
    public class PatientAccountCRUD
    {
        public void MedicalRecord(MedicalRecord account)
        {
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("                   ZDRAVSTVENI KARTON                 ");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(account);
            Console.WriteLine("-------------------------------------------------------------");
        }
        public MedicalRecord CreateInput()
        {
            Console.Write("\nUnesite ime pacijenta: ");
            string name = Console.ReadLine();

            Console.Write("Unesite prezime pacijenta: ");
            string lastname = Console.ReadLine();

            Console.Write("Unesite id pacijenta: ");
            string id = Console.ReadLine();

            Console.Write("Unesite adresu pacijenta: ");
            string address = Console.ReadLine();

            Console.Write("Unesite email pacijenta: ");
            string email = Console.ReadLine();

            Console.Write("Unesite korisnicko ime pacijenta: ");
            string username = Console.ReadLine();

            Console.Write("Unesite lozinku pacijenta: ");
            string password = Console.ReadLine();

            MedicalRecord account = new MedicalRecord(name, lastname, address, username, password, email, id);
            return account;

        }
        public void JsonWriteFile(MedicalRecord account)
        {
            string fileName = "Data.json";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(account);
            Console.WriteLine(jsonString);
            File.AppendAllText(fileName, jsonString);
        }
        public void JsonReadFile(string id)
        {
            //string jsonString = String.Empty;
            //jsonString = File.ReadAllText("Data.json");
            //PatientAccount account = JsonConvert.DeserializeObject<PatientAccount>(jsonString);
            //Console.WriteLine(jsonString);

            /*string fileName = "../../../Data/Appointments.json";
            string appointmentFileData = "";
            appointmentFileData = File.ReadAllText(fileName);
            string[] appointments = appointmentFileData.Split('\n');
            List<Appointment> appointmentsList = new List<Appointment>();
            foreach (String s in appointments)
            {
                if (s != "")
                {
                    Appointment? appointment = JsonSerializer.Deserialize<Appointment>(s);
                    if (appointment != null)
                        appointmentsList.Add(appointment);

                }
            }
            return appointmentsList;*/

            StreamReader r = new StreamReader("Data.json");
            string jsonString = r.ReadToEnd();
            MedicalRecord m = JsonConvert.DeserializeObject<MedicalRecord>(jsonString);

            if (m.Id == id) { Console.WriteLine(m); }
            else { Console.WriteLine("Neispravan id!"); }
           
        }
        public void CreatePatient()
        {
            Console.WriteLine("\n-------------------------KREIRANJE NALOGA-----------------------------");

            MedicalRecord account = CreateInput();
            MedicalRecord(account);
            JsonWriteFile(account);
        }
        public void ReadPatient()
        {
            Console.Write("\nUnesite id pacijenta: ");
            string id = Console.ReadLine();

            Console.WriteLine("-------------------------PREGLED NALOGA-----------------------------");

            JsonReadFile(id);

        }
        public void UpdatePatient()
        {
            Console.WriteLine("-------------------------IZMJENA NALOGA-----------------------------");
        }
        public void DeletePatient()
        {
            Console.WriteLine("-------------------------BRISANJE NALOGA-----------------------------");
          
        }

       
    }
}
