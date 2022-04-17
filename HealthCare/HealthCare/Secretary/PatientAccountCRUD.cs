using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.Secretary
{
    public class PatientAccountCRUD
    {
        public void MedicalRecord(PatientAccount account)
        {
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("                   ZDRAVSTVENI KARTON                 ");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(account);
            Console.WriteLine("-------------------------------------------------------------");
        }
        public PatientAccount CreateInput()
        {
            Console.Write("\nUnesite ime pacijenta: ");
            string name = Console.ReadLine();

            Console.Write("Unesite prezime pacijenta: ");
            string lastname = Console.ReadLine();

            Console.Write("Unesite adresu pacijenta: ");
            string address = Console.ReadLine();

            Console.Write("Unesite email pacijenta: ");
            string email = Console.ReadLine();

            Console.Write("Unesite korisnicko ime pacijenta: ");
            string username = Console.ReadLine();

            Console.Write("Unesite lozinku pacijenta: ");
            string password = Console.ReadLine();

            PatientAccount account = new PatientAccount(name, lastname, address, username, password, email);
            return account;

        }
        public void JsonWriteFile(PatientAccount account)
        {
            string fileName = "Data.json";
            string jsonString = System.Text.Json.JsonSerializer.Serialize(account);
            Console.WriteLine(jsonString);
            File.AppendAllText(fileName, jsonString);
        }
        public void JsonReadFile(string name)
        {
            string jsonString = String.Empty;
            jsonString = File.ReadAllText("Data.json");
            PatientAccount account = JsonConvert.DeserializeObject<PatientAccount>(jsonString);
            Console.WriteLine(account);
        }
        public void CreatePatient()
        {
            Console.WriteLine("\n-------------------------KREIRANJE NALOGA-----------------------------");

            PatientAccount account = CreateInput();
            MedicalRecord(account);
            JsonWriteFile(account);
        }
        public void ReadPatient()
        {
            Console.Write("\nUnesite ime pacijenta: ");
            string name = Console.ReadLine();

            Console.WriteLine("-------------------------PREGLED NALOGA-----------------------------");

            JsonReadFile(name);

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
