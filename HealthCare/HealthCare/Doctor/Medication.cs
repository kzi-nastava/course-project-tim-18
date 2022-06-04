using System.Text.Json;

namespace HealthCare.Doctor;

public class Medication
{
    private string name;
    private int timesADay;
    private TimeForMedicine whenToConsume;
    private List<Allergy> allergyTriggers;
    private List<string> ingredients;


    public string Name
    {
        get => name;
        set => name = value;
    }

    public int TimesADay
    {
        get => timesADay;
        set => timesADay = value;
    }

    public TimeForMedicine WhenToConsume
    {
        get => whenToConsume;
        set => whenToConsume = value;
    }

    public List<Allergy> AllergyTriggers
    {
        get => allergyTriggers;
        set => allergyTriggers = value;
    }
    public List<string> Ingredients { get => ingredients; set => ingredients = value; }

    public Medication(string name, int timesADay, TimeForMedicine whenToConsume, List<Allergy> allergyTriggers, List<string> ingredients)
    {
        this.name = name;
        this.timesADay = timesADay;
        this.whenToConsume = whenToConsume;
        this.allergyTriggers = allergyTriggers;
        this.ingredients = ingredients;
    }
    private static void serialize(List<Medication> medications)
    {
        File.WriteAllText("../../../Data/Medication.json", JsonSerializer.Serialize(medications));
    }

    public static List<Medication> Deserialize()
    {
        string filepath = "../../../Data/Medication.json";
        string jsonText = File.ReadAllText(filepath);
        List<Medication> deserializedNedications = JsonSerializer.Deserialize<List<Medication>>(jsonText);
        return deserializedNedications;
    }


    public static void SerializeSuggestions(List<Medication> medications)
    {
        File.WriteAllText("../../../Data/MedicationSuggestions.json", JsonSerializer.Serialize(medications));
    }

    public static List<Medication> DeserializeSuggestions()
    {
        string filepath = "../../../Data/MedicationSuggestions.json";
        string jsonText = File.ReadAllText(filepath);
        List<Medication> deserializedNedications = JsonSerializer.Deserialize<List<Medication>>(jsonText);
        return deserializedNedications;
    }




    public static void addMedication(Medication r)
    {
        List<Medication> deserializedMedications = Deserialize();
        deserializedMedications.Add(r);
        serialize(deserializedMedications);
    }


    public static void addMedicationSuggestion(Medication r)
    {
        List<Medication> deserializedMedications = DeserializeSuggestions();
        deserializedMedications.Add(r);
        SerializeSuggestions(deserializedMedications);
    }

}