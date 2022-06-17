
using System.Globalization;
using System.Text.Json;

namespace HealthCare.Patient.CreatingAppointment
{
    class AntiTrolCounter
    {
        private string patientUsername;
        private string timeOfChange;
        private typeOfChange typeOfChange;

        public string PatientUsername
        {
            get => patientUsername;
            set => patientUsername = value ?? throw new ArgumentNullException(nameof(value));
        }

        public typeOfChange TypeOfChange
        {
            get => typeOfChange;
            set => typeOfChange = value;
        }

        public string TimeOfChange
        {
            get => timeOfChange;
            set => timeOfChange = value ?? throw new ArgumentNullException(nameof(value));
        }

        public AntiTrolCounter()
        {
            patientUsername = "";
            timeOfChange = "";
            typeOfChange = 0;
        }

        public AntiTrolCounter(string patientUsername,string timeOfChange, typeOfChange typeOfChange)
        {
            this.typeOfChange = typeOfChange;
            this.timeOfChange = timeOfChange;
            this.patientUsername = patientUsername;
        }

        public static List<AntiTrolCounter> antiTrolDeserialization()
        {
            string fileName = "../../../Data/AntiTrol.json";
            string antiTrolFileData = "";
            antiTrolFileData = File.ReadAllText(fileName);
            string[] antiTrolString = antiTrolFileData.Split('\n');
            List<AntiTrolCounter> antiTrolList = new List<AntiTrolCounter>();
            foreach (String s in antiTrolString)
            {
                if (s != "")
                {
                    AntiTrolCounter? antiTrol = JsonSerializer.Deserialize<AntiTrolCounter>(s);
                    if (antiTrol != null)
                        antiTrolList.Add(antiTrol);

                }
            }
            return antiTrolList;
        }

        public void serializeAntiTrol()
        {
            string fileName = "../../../Data/AntiTrol.json";
            List<AntiTrolCounter> listOfAntiTrol = antiTrolDeserialization();
            string json = "";
            foreach (AntiTrolCounter AntiTrol in listOfAntiTrol)
            {
                json += JsonSerializer.Serialize(AntiTrol) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }

        public bool validation()
        {
            bool value;
            if(this.typeOfChange == typeOfChange.Delete)
            {
                value = deleteValidation();
            }
            if (this.typeOfChange == typeOfChange.Update)
            {
                value = updateValidation();
            }
            else
            {
                value = createValidation();
            }
            return value;
        }

        public bool deleteValidation()
        {
            List<AntiTrolCounter> antiTrolList = antiTrolDeserialization();
            DateTime now = DateTime.Now;
            int counter = 0;
            foreach(AntiTrolCounter antiTrol in antiTrolList)
            {
                DateTime changeDate = Appointment.stringToDateTime(antiTrol.TimeOfChange);
                TimeSpan timeDifference = now.Subtract(changeDate);
                if (timeDifference.TotalDays < 31 && this.PatientUsername == antiTrol.PatientUsername && antiTrol.typeOfChange == typeOfChange.Delete)
                    counter++;
            }
            if(counter > 5)
                return false;
            serializeAntiTrol();
            return true;
        }

        public bool updateValidation()
        {
            List<AntiTrolCounter> antiTrolList = antiTrolDeserialization();
            DateTime now = DateTime.Now;
            int counter = 0;
            foreach (AntiTrolCounter antiTrol in antiTrolList)
            {
                DateTime changeDate = Appointment.stringToDateTime(antiTrol.TimeOfChange);
                TimeSpan timeDifference = now.Subtract(changeDate);
                if (timeDifference.TotalDays < 31 && this.PatientUsername == antiTrol.PatientUsername && antiTrol.typeOfChange == typeOfChange.Update)
                    counter++;
            }
            if (counter > 5)
                return false;
            serializeAntiTrol();
            return true;
        }

        public bool createValidation()
        {
            List<AntiTrolCounter> antiTrolList = antiTrolDeserialization();
            DateTime now = DateTime.Now;
            int counter = 0;
            foreach (AntiTrolCounter antiTrol in antiTrolList)
            {
                DateTime changeDate = Appointment.stringToDateTime(antiTrol.TimeOfChange);
                TimeSpan timeDifference = now.Subtract(changeDate);
                if (timeDifference.TotalDays < 31 && this.PatientUsername == antiTrol.PatientUsername && antiTrol.typeOfChange == typeOfChange.Create)
                    counter++;
            }
            if (counter > 7)
                return false;
            serializeAntiTrol();
            return true;
        }
    }
}
