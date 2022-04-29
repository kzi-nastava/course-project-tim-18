using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
    public class BlockedPatients
    {
        private BlockedType blockedType;
        private Patient patient;

        public BlockedType BlockedType
        {
            get { return blockedType; }
            set { blockedType = value; }
        }
        public Patient Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        public BlockedPatients(BlockedType blockedType, Patient patient)
        {
            this.blockedType = blockedType;
            this.patient = patient;
        }

        public static List<BlockedPatients> blockedPatientsDeserialization()
        {
            string fileName = "../../../Data/BlockedPatients.json";
            string BlockedPatientsFileData = "";
            BlockedPatientsFileData = File.ReadAllText(fileName);
            string[] blockedPatients = BlockedPatientsFileData.Split('\n');
            List<BlockedPatients> blockedPatientsList = new List<BlockedPatients>();
            foreach (String s in blockedPatients)
            {
                if (s != "")
                {
                    BlockedPatients? blockedPatient = JsonSerializer.Deserialize<BlockedPatients>(s);
                    if (blockedPatient != null)
                        blockedPatientsList.Add(blockedPatient);

                }
            }
            return blockedPatientsList;
        }

        public void serializeBlockedPatient()
        {
            string fileName = "../../../Data/BlockedPatients.json";
            List<BlockedPatients> blockedPatients = blockedPatientsDeserialization();
            string json = "";
            foreach (BlockedPatients blockedPatient in blockedPatients)
            {
                json += JsonSerializer.Serialize(blockedPatient) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }

    }
}
