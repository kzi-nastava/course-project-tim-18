
using HealthCare.Doctor.PrescribeMedication;

namespace HealthCare
{
    public class MedicationHandling
    {
        private static string GetMedicationName()
        {
            Console.Write("Unesi naziv leka - ");
            return Console.ReadLine();
        }


        private static int GetTimesOfDay()
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


        private static TimeForMedicine GetTimeForMedicine()
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





        public static void SuggestMedication()
        {

            string medicationName = GetMedicationName();

            int timesADay = GetTimesOfDay();


            TimeForMedicine timeForMedicine = GetTimeForMedicine();

            Allergy allergy = GetAllergy();


            List<string> ingredients = GetIngredients();


            Medication.addMedicationSuggestion(new Medication(medicationName, timesADay, timeForMedicine, new List<Allergy>() { allergy }, ingredients, ""));

        }

    }
}
