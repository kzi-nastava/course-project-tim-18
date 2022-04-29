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

        public BlockedPatients(BlockedType blockedType, Patient patient)
        {
            this.blockedType = blockedType;
            this.patient = patient;
        }

        public BlockedPatients()
        {
        }
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
        public override string ToString()
        {
            return BlockedType + "," + patient.username + "," + patient.password;
        }

        public  List<BlockedPatients> BlockedPatientsDeserialization()
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

        public void SerializeBlockedPatient()
        {
            string fileName = "../../../Data/BlockedPatients.json";
            List<BlockedPatients> blockedPatients = BlockedPatientsDeserialization();
            string json = "";
            foreach (BlockedPatients blockedPatient in blockedPatients)
            {
                json += JsonSerializer.Serialize(blockedPatient) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }

        public void DeleteFromBlockedPatients(string username, string fileName)
        {
            List<BlockedPatients> blockedPatients = BlockedPatientsDeserialization();
            string json = "";
            foreach (BlockedPatients blockedPatient in blockedPatients)
            {
                if (blockedPatient.Patient.username != username)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(blockedPatient) + "\n";
                }
            }
            File.WriteAllText(fileName, json);
        }
    }
}
