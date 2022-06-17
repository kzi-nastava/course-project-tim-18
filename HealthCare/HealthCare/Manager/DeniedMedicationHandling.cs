using System.Text.Json;
using System.Text.Json.Serialization;
using HealthCare.Doctor;
using HealthCare.Doctor.PrescribeMedication;
using HealthCare.Patient;

namespace HealthCare
{
    public class DeniedMedicationHandling
    {
        static void PrintMedicationNames(List<Medication> declinedMedications)
        { 
            for (int i = 0; i < declinedMedications.Count; i++)
            {

                Console.WriteLine((i + 1).ToString() + ". " + declinedMedications[i].Name);

            }

        }
        static int GetIndex()
        {
            Console.Write("Unesi redni broj leka koji revidiraš  - ");

            int index;

            string indexString = Console.ReadLine();
            if (Int32.TryParse(indexString, out index) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return 0;
            }

            if (index <= 0)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return 0;
            }

            return index;

        }

        static int GetTimesADay()
        {
            Console.Write("Unesi koliko puta lek treba da se uzima dnevno  - ");
            int timesADay;
            string timesADayString = Console.ReadLine();
            if (Int32.TryParse(timesADayString, out timesADay) == false)
            {
                Console.WriteLine("Uneta neispravna vrednost.");
                return 1;
            }
            return timesADay;

        }


        static TimeForMedicine GetTimeForMedicine()
        {
            Console.WriteLine("1.Pre obroka");
            Console.WriteLine("2.Tokom obroka");
            Console.WriteLine("3.Posle obroka");
            Console.WriteLine("4.Nebitno");
            Console.WriteLine("Unesi vreme kad lek treba da se unese:");

            string userResponse = Console.ReadLine();
            TimeForMedicine timeForMedicine;

            if (userResponse == "1")
                timeForMedicine = TimeForMedicine.BeforeTheMeal;
            else if (userResponse == "2")
                timeForMedicine = TimeForMedicine.DuringTheMeal;
            else if (userResponse == "3")
                timeForMedicine = TimeForMedicine.AfterTheMeal;
            else
                timeForMedicine = TimeForMedicine.Irrelevant;
            
            return timeForMedicine;

        }



        private static Allergy GetAllergy()
        {

            Console.WriteLine("1.Penicilin");
            Console.WriteLine("2.Antibiotik");
            Console.WriteLine("3.Sulfonamid");
            Console.WriteLine("4.Antikonvulziv");
            Console.WriteLine("5.NSAIL");
            Console.WriteLine("Unesi redni broj sajstojka koji je sadržan u leku:");


            string userResponse = Console.ReadLine();
            Allergy allergy;


            if (userResponse == "1")
                allergy = Allergy.Penicilin;
            else if (userResponse == "2")
                allergy = Allergy.Antibiotic;
            else if (userResponse == "3")
                allergy = Allergy.Sulfonamides;
            else if (userResponse == "4")
                allergy = Allergy.Anticonvulsants;
            else
                allergy = Allergy.NSAIDs;

            return allergy;
        }


        private static List<string> GetIngredients()
        {


            List<string> ingredients = new List<string>();

            string userResponse;

            while (true)
            {

                Console.Write("Unesi ime sastojka(ako si završio unesi stop)  - ");

                userResponse = Console.ReadLine();

                if (userResponse == "stop")
                    break;

                ingredients.Add(userResponse);

            }

            return ingredients;
        }



        static public void SuggestDeclinedMedication()
        {
            List<Medication> declinedMedications = Medication.DeserializeDenials();

            PrintMedicationNames(declinedMedications);


            int index = GetIndex();


            int timesADay = GetTimesADay();

            string medicationName = declinedMedications[index - 1].Name;

            declinedMedications.Remove(declinedMedications[index]);

            Medication.SerializeDenials(declinedMedications);

            TimeForMedicine timeForMedicine = GetTimeForMedicine();




            Allergy allergy = GetAllergy();


            List<string> ingredients =  GetIngredients();






            Medication.addMedicationSuggestion(new Medication(medicationName, timesADay, timeForMedicine, new List<Allergy>() { allergy }, ingredients, ""));

        }

    }
}
