
using HealthCare.Patient;

namespace HealthCare
{
    public class SurveyHandling
    {

        static public string GetOption()
        {
            string userResponse = "";

            while (userResponse != "1" && userResponse != "2")
            {
                Console.WriteLine("1. Pregled anketa za bolnicu");
                Console.WriteLine("2. Pregled anketa za doktora");
                Console.WriteLine("Unesi opciju: ");
                userResponse = Console.ReadLine();
            }
            return userResponse;
        }

        public static void ViewSurveyResults()
        {

            string option = GetOption();

            if (option == "1")
                PrintSurveyHospital(HospitalGrade.DeserializeHospitalGrade());
            else
                PrintSurveyDoctor(DoctorsGrade.DeserializeDoctorsGrade());

        }

        static void GetCounterAndAverages(List<HospitalGrade> hospitalGrades, List<int[]> counters, List<double> averages)
        {

            Console.WriteLine();
            Console.WriteLine("KOMENTARI");
            Console.WriteLine("--------------------------------------------------");

            int i = 1;

            foreach (HospitalGrade hospitalGrade in hospitalGrades)
            {
                Console.WriteLine(i.ToString() + ". Komentar - " + hospitalGrade.Comment);
                i++;

                averages[0] += hospitalGrade.HowCleanItIs;
                counters[0][hospitalGrade.HowCleanItIs - 1]++;

                averages[1] += hospitalGrade.WouldYouSuggest;
                counters[1][hospitalGrade.WouldYouSuggest - 1]++;

                averages[2] += hospitalGrade.HowSatisfiedAreYou;
                counters[2][hospitalGrade.HowSatisfiedAreYou - 1]++;

                averages[3] += hospitalGrade.HowGoodHospitalIs;
                counters[3][hospitalGrade.HowGoodHospitalIs - 1]++;

            }

            for (int j = 0; j < 4; j++)
                averages[j] /= hospitalGrades.Count;
        }


        static void PrintGradesHospital(List<int[]> counters, List<double> averages)
        {
            Console.WriteLine("OCENE");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Prosecna ocene");
            Console.WriteLine("Da li bi preporucili: " + averages[0]);
            Console.WriteLine("Koliko Vam se svidja bolnica: " + averages[1]);
            Console.WriteLine("Koliko je cista bolnica: " + averages[2]);
            Console.WriteLine("Koliko ste zadovoljni: " + averages[3]);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Ocena " + (i + 1));
                Console.WriteLine("Da li bi preporucili: " + counters[0][i]);
                Console.WriteLine("Koliko Vam se svidja bolnica: " + counters[1][i]);
                Console.WriteLine("Koliko je cista bolnica: " + counters[2][i]);
                Console.WriteLine("Koliko ste zadovoljni: " + counters[3][i]);
                Console.WriteLine();
            }
        }

        static void PrintSurveyHospital(List<HospitalGrade> hospitalGrades)
        {

            List<int[]> counters = new List<int[]> { new int[5], new int[5], new int[5], new int[5] };
            List<double> averages = new List<double> { 0, 0, 0, 0 };

            GetCounterAndAverages(hospitalGrades, counters, averages);

            PrintGradesHospital(counters, averages);
        }


        static void FillCounter(List<DoctorsGrade> doctorsGrades, Dictionary<string, List<DoctorsGrade>> counter)
        {
            foreach (DoctorsGrade doctorsGrade in doctorsGrades)
            {
                if (counter.ContainsKey(doctorsGrade.Doctor))
                {
                    counter[doctorsGrade.Doctor].Add(doctorsGrade);
                }
                else
                {
                    counter[doctorsGrade.Doctor] = new List<DoctorsGrade> { doctorsGrade };
                }
            }

        }
        static void PrintDoctorsGrade(List<DoctorsGrade> doctorsGrades, Dictionary<string, List<DoctorsGrade>> counter, Dictionary<string, double> sortedDoctors)
        {


            foreach (var item in counter)
            {
                Console.WriteLine();
                Console.WriteLine("Doktor - " + item.Key);
                List<DoctorsGrade> grades = item.Value;

                double averageWouldYouSuggest = 0;
                int[] countWouldYouSuggest = new int[5];

                double averageHowGoodIsDoctor = 0;
                int[] countHowGoodIsDoctor = new int[5];


                int i = 1;
                Console.WriteLine("------------------------------------------------");

                foreach (DoctorsGrade doctorsGrade in grades)
                {
                    Console.WriteLine(i.ToString() + ". Komentar - " + doctorsGrade.Comment);
                    i++;


                    averageWouldYouSuggest += doctorsGrade.WouldYouSuggest;
                    countWouldYouSuggest[doctorsGrade.WouldYouSuggest - 1]++;

                    averageHowGoodIsDoctor += doctorsGrade.HowGoodDoctorWas;
                    countHowGoodIsDoctor[doctorsGrade.HowGoodDoctorWas - 1]++;

                }

                averageWouldYouSuggest /= grades.Count;
                averageHowGoodIsDoctor /= grades.Count;

                Console.WriteLine();
                Console.WriteLine("Prosecna ocene");
                Console.WriteLine("Da li bi preporucili: " + averageWouldYouSuggest);
                Console.WriteLine("Koliko ste zadovoljni doktorom: " + averageHowGoodIsDoctor);
                Console.WriteLine();

                sortedDoctors[item.Key] = averageHowGoodIsDoctor;

                for (i = 0; i < 5; i++)
                {
                    Console.WriteLine("Ocena " + (i + 1));
                    Console.WriteLine("Da li bi preporucili: " + countWouldYouSuggest[i]);
                    Console.WriteLine("Koliko ste zadovoljni doktorom: " + countHowGoodIsDoctor[i]);
                    Console.WriteLine();
                }

                Console.WriteLine("------------------------------------------------");
                Console.WriteLine();


            }
        }

        public static void PrintDoctors(Dictionary<string, double> sortedDoctors)
        {

            int i = 0;
            foreach (var item in sortedDoctors)
            {
                if (i >= 2)
                    break;

                Console.WriteLine((i + 1).ToString() + ". " + item.Key + " - " + item.Value);

                i++;
            }
        }


        static void PrintSurveyDoctor(List<DoctorsGrade> doctorsGrades)
        {
            string option;


            Dictionary<string, List<DoctorsGrade>> counter = new Dictionary<string, List<DoctorsGrade>>();

            FillCounter(doctorsGrades, counter);

            Dictionary<string, double> sortedDoctors = new Dictionary<string, double>();

            PrintDoctorsGrade(doctorsGrades, counter, sortedDoctors);

            Console.WriteLine("Da li hoces ispis tri najbolja ocenjena lekara? (da/ne)");
            option = Console.ReadLine();

            if (option == "da")
            {
                sortedDoctors = sortedDoctors.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                PrintDoctors(sortedDoctors);

            }

            Console.WriteLine("Da li hoces ispis tri najgore ocenjena lekara? (da/ne)");
            option = Console.ReadLine();

            if (option == "da")
            {
                sortedDoctors = sortedDoctors.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                PrintDoctors(sortedDoctors);

            }



        }





    }
}
