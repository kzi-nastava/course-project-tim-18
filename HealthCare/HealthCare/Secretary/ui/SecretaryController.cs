using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HealthCare.Secretary

{
    internal class SecretaryController
    {
        SecretaryService secretaryService = new SecretaryService();
        public void PrintHeader(string title)
        {
            Console.WriteLine("\n-------------------------" + title + "-----------------------------");
        }

        public void PrintMainManu()
        {
            Console.WriteLine("-------------OPCIJE---------------");
            Console.WriteLine("1  Manipulisanje nalogom");
            Console.WriteLine("2  Blokiraj naloga");
            Console.WriteLine("3  Odblokiraj naloga");
            Console.WriteLine("4  Pregled zahtjeva");
            Console.WriteLine("5  Zakazivanje pregleda");
            Console.WriteLine("6  Nabavka dinamicke opreme");
            Console.WriteLine("7  Rasporedjivanje dinamicke opreme");
            Console.WriteLine("8  Pregled zahtjeva za slobodne dane");
            Console.WriteLine("9  Exit");

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

                    secretaryService.CreatePatientAccount();
                    return true;
                case "2":
                    PrintHeader("PREGLEDAJ NALOG");
                    secretaryService.ReadPatientAccount();
                    return true;
                case "3":
                    PrintHeader("IZMIJENI NALOG");
                    secretaryService.UpdatePatientAccount();
                    return true;
                case "4":
                    PrintHeader("OBRISI NALOG");
                    secretaryService.DeletePatientAccount();
                    return true;
                case "5":
                    return false;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
            }
        }

        public bool WriteManu(Manager manager)
        {
            PrintMainManu();
            switch (Console.ReadLine())
            {
                case "1":
                    CheckInput();
                    return true;
                case "2":
                    secretaryService.ReadPatientAccount();
                    return true;
                case "3":
                    secretaryService.UnblockingPatientsAccount();
                    return true;
                case "4":
                    secretaryService.ViewingPatientRequests();
                    return true;
                case "5":
                    secretaryService.MakingAnAppointment();
                    return true;
                case "6":
                    manager.Load();
                    manager.DynamicEquipmentRequests();
                    manager.Save();
                    return true;
                case "7":
                    manager.Load();
                    manager.DynamicEquipmentDistribution();
                    manager.Save();
                    return true;
                case "8":
                    secretaryService.ViewDayOffRequests();
                    return true;
                default:
                    Console.WriteLine("\nPogresan unos, pokusajte ponovo!\n");
                    return true;
            }

        }
    }
}
