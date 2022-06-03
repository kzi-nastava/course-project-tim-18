using System.Data.SqlTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
namespace HealthCare.Doctor;

public class Prescription
{
    public List<Medication> medications;
    private List<Allergy> allAllergyTriggers;
    private string patient;

    public List<Medication> Medications { get => medications; set => medications = value; }
    public List<Allergy> AllAllergyTriggers { get => allAllergyTriggers; set => allAllergyTriggers = value; }

    public string Patient
    {
        get => patient;
        set => patient = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Prescription()
    {
        this.patient = "";
        this.medications = new List<Medication>();
        this.allAllergyTriggers = new List<Allergy>();
    }
    public Prescription(List<Medication> medications)
    {
        this.medications = medications;
        this.allAllergyTriggers = new List<Allergy>();
        LoadAllergies();
        this.patient = "";
    }

    [JsonConstructor]
    public Prescription(List<Medication> medications, string patient)
    {
        this.medications = medications;
        this.allAllergyTriggers = new List<Allergy>();
        LoadAllergies();
        this.patient = patient;
    }
    
    public bool CheckPatientAllergies(Patient.Patient patient)
    {
        /*
         * Checks patient eligibility for a prescription based on his allergies
         * @params patient - patient whose medical record is checked
         * @return value - returns true if patient is eligible for the prescrition, returns false if he's not
         */
        foreach (var allergy in patient.MedicalRecord.Allergies)
        {
            if (this.allAllergyTriggers.Contains(allergy))
            {
                return false;
            }
        }

        return true;
    }

    public void LoadAllergies()
    {
        foreach (var medication in medications)
        {
            foreach (var allergyTrigger in medication.AllergyTriggers)
            {
                if (!allAllergyTriggers.Contains(allergyTrigger))
                {
                    allAllergyTriggers.Add(allergyTrigger);
                }
                
            }
        }
    }

    public void PrintPrescription()
    {
        for (int i = 0; i < medications.Count; i++)
        {
            Medication medication = medications[i];
            Console.WriteLine("==============================");
            Console.WriteLine(i+1+ ".");
            Console.WriteLine("Ime leka: " + medication.Name);
            Console.WriteLine("Koliko puta dnevno se pije: " +medication.TimesADay);
            string timeWhenToConsume;
            if (medication.WhenToConsume == TimeForMedicine.BeforeTheMeal)
            {
                timeWhenToConsume = "Pre jela";
            }else if (medication.WhenToConsume  == TimeForMedicine.DuringTheMeal)
            {
                timeWhenToConsume = "Tokom jela";
            }else if (medication.WhenToConsume == TimeForMedicine.AfterTheMeal)
            {
                timeWhenToConsume = "Posle jela";
            }
            else
            {
                timeWhenToConsume = "Nebitno";
            }
            Console.WriteLine("Kada se pije lek: " + timeWhenToConsume);
            Console.WriteLine("==============================");
        }
    }
    
    public static List<Prescription> DeserializeDoctorsGrade()
    {
        string fileName = "../../../Data/Prescription.json";
        string PrescriptionFileData = "";
        PrescriptionFileData = File.ReadAllText(fileName);
        string[] Prescriptions = PrescriptionFileData.Split('\n');
        List<Prescription> DoctorsGradeList = new List<Prescription>();
        foreach (String s in Prescriptions)
        {
            if (s != "")
            {
                Prescription? prescription = JsonSerializer.Deserialize<Prescription>(s);
                if (prescription != null)
                    DoctorsGradeList.Add(prescription);

            }
        }
        return DoctorsGradeList;
    }

    public void SerializePrescription()
    {
        string fileName = "../../../Data/Prescription.json";
        List<Prescription> prescriptions = DeserializeDoctorsGrade();
        //List<Prescription> prescriptions = new List<Prescription>();
        string json = "";
        foreach (Prescription prescription in prescriptions)
        {
            json += JsonSerializer.Serialize(prescription) + "\n";
        }
        json += JsonSerializer.Serialize(this) + "\n"; ;
        File.WriteAllText(fileName, json);
    }
    
}