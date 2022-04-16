using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void CreatePatient()
        {
            Console.WriteLine("\nTO DO: Create patient");

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

            PatientAccount account = new PatientAccount(name, lastname, address, username, password,email);
            MedicalRecord(account);

        }
        public void ReadPatient()
        {
            Console.WriteLine("TO DO: Read patient");
        }
        public void UpdatePatient()
        {
            Console.WriteLine("TO DO: Update patient");
        }
        public void DeletePatient()
        {
            Console.WriteLine("TO DO: Delete patient");
        }

       
    }
}
