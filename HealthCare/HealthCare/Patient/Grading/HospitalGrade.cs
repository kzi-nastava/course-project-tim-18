using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace HealthCare.Patient.Grading
{
    class HospitalGrade
    {
        private string comment;
        private int wouldYouSuggest;
        private int howCleanItIs;
        private int howSatisfiedAreYou;
        private int howGoodHospitalIs;

        public int HowCleanItIs
        {
            get => howCleanItIs;
            set => howCleanItIs = value;
        }

        public int HowSatisfiedAreYou
        {
            get => howSatisfiedAreYou;
            set => howSatisfiedAreYou = value;
        }

        public int HowGoodHospitalIs
        {
            get => howGoodHospitalIs;
            set => howGoodHospitalIs = value;
        }

        public int WouldYouSuggest
        {
            get => wouldYouSuggest;
            set => wouldYouSuggest = value;
        }

        public string Comment
        {
            get => comment;
            set => comment = value ?? throw new ArgumentNullException(nameof(value));
        }

        public HospitalGrade(string comment, int wouldYouSuggest, int howGoodHospitalIs, int howSatisfiedAreYou, int howCleanItIs)
        {
            this.howGoodHospitalIs = howGoodHospitalIs;
            this.wouldYouSuggest = wouldYouSuggest;
            this.comment = comment;
            this.howSatisfiedAreYou = howSatisfiedAreYou;
            this.howCleanItIs = howCleanItIs;
        }

        public static List<HospitalGrade> DeserializeHospitalGrade()
        {
            string fileName = "../../../Data/HospitalGrade.json";
            string HospitalGradeFileData = "";
            HospitalGradeFileData = File.ReadAllText(fileName);
            string[] HospitalGrades = HospitalGradeFileData.Split('\n');
            List<HospitalGrade> HospitalGradeList = new List<HospitalGrade>();
            foreach (string s in HospitalGrades)
            {
                if (s != "")
                {
                    HospitalGrade? hospitalGrade = JsonSerializer.Deserialize<HospitalGrade>(s);
                    if (hospitalGrade != null)
                        HospitalGradeList.Add(hospitalGrade);

                }
            }
            return HospitalGradeList;
        }

        public void SerializeDoctorsGrade()
        {
            string fileName = "../../../Data/HospitalGrade.json";
            List<HospitalGrade> hospitalGrades = DeserializeHospitalGrade();
            string json = "";
            foreach (HospitalGrade hospitalGrade in hospitalGrades)
            {
                json += JsonSerializer.Serialize(hospitalGrade) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }
    }
}
