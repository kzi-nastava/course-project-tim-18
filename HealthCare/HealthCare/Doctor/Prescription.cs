using System.Data.SqlTypes;

namespace HealthCare.Doctor;

public class Prescription
{
    public List<Medication> medications;
    private List<Allergy> allAllergyTriggers;
    public Prescription(List<Medication> medications)
    {
        this.medications = medications;
        this.allAllergyTriggers = new List<Allergy>();
        loadAllergies(this.medications);
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

    private void loadAllergies(List<Medication> medications)
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
}