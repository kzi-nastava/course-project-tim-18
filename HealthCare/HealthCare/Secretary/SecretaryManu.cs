using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare
{
    public class SecretaryManu
    {
        Secretary crud = new Secretary();
        public void PrintHeader(string title)
        {
            Console.WriteLine("\n-------------------------"+title+"-----------------------------");
        }
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
                    PrintHeader("KREIRAJ NALOG");
                    crud.CreatePatient();
                    return true;
                case "2":
                    PrintHeader("PREGLEDAJ NALOG");
                    crud.ReadPatient();
                    return true;
                case "3":
                    PrintHeader("IZMIJENI NALOG");
                    crud.UpdatePatient();
                    return true;
                case "4":
                    PrintHeader("OBRISI NALOG");
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
                    crud.UnblockingPatientsAccount();
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
