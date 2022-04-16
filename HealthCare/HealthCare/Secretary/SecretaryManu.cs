using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Secretary
{
    public class SecretaryManu
    {
        PatientAccountCRUD crud = new PatientAccountCRUD();
        public void PrintMainManu()
        {
            Console.WriteLine("1. Manipulisanje nalogom pacijenta");
            Console.WriteLine("2. Blokiranje pacijenata");
            Console.WriteLine("3. Pregled zahtjeva");
            Console.WriteLine("4. Exit");
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
                    crud.CreatePatient();
                    return true;
                case "2":
                    crud.ReadPatient();
                    return true;
                case "3":
                    crud.UpdatePatient();
                    return true;
                case "4":
                    crud.DeletePatient();
                    return true;
                case "5":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
            }
        }
        public bool WriteManu()
        {
            PrintMainManu();
            switch (Console.ReadLine())
            {
                case "1":
                    CheckInput();
                    return true;
                case "2":
                    return true;
                case "3":
                    return true;
                case "4":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
                    
            }

        }
    }
}
