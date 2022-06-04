using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
    internal class EquipmentRequest
    {


        private Equipment equipment;
        private string executionDate;


        public EquipmentRequest(Equipment equipment, string executionDate)
        {

            this.equipment = equipment;
            this.executionDate = executionDate;


        }
        public EquipmentRequest()
        {



        }



        public string ExecutionDate { get => executionDate; set => executionDate = value; }
        public Equipment Equipment { get => equipment; set => equipment = value; }

        public void EquipmentRequestSerialization()
        {
            string file = "../../../Data/EquipmentRequest.json";
            string json = JsonSerializer.Serialize(this) + "\n";
            File.AppendAllText(file, json);
        }
        public List<EquipmentRequest> EquipmentRequestDeserialization()
        {
            string fileName = "../../../Data/EquipmentRequest.json";
            string PatientFileData = "";
            PatientFileData = File.ReadAllText(fileName);
            string[] Patients = PatientFileData.Split('\n');
            List<EquipmentRequest> PatientsList = new List<EquipmentRequest>();
            foreach (String s in Patients)
            {
                if (s != "")
                {
                    EquipmentRequest? patient = JsonSerializer.Deserialize<EquipmentRequest>(s);
                    if (patient != null)
                        PatientsList.Add(patient);

                }
            }

            return PatientsList;
        }

        public void DeleteFromEquipmentRequest(string name)
        {
            string fileName = "../../../Data/EquipmentRequest.json";
            List<EquipmentRequest> equipmentRequestList = EquipmentRequestDeserialization();
            string json = "";
            foreach (EquipmentRequest equipmentRequest in equipmentRequestList)
            {
                if (equipmentRequest.Equipment.Name != name)
                {
                    json += System.Text.Json.JsonSerializer.Serialize(equipmentRequest) + "\n";
                }
            }
            File.WriteAllText(fileName, json);
        }

    }
}
