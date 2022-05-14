using System.Text.Json;

namespace HealthCare.Doctor;

public class Medication
{
    private string name;
    private int timesADay;
    private TimeForMedicine whenToConsume;
    private List<Allergy> allergyTriggers;

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

    public Medication(string name, int timesADay, TimeForMedicine whenToConsume, List<Allergy> allergyTriggers)
    {
        this.name = name;
        this.timesADay = timesADay;
        this.whenToConsume = whenToConsume;
        this.allergyTriggers = allergyTriggers;
    }
    private static void serialize(List<Medication> reports)
    {
        File.WriteAllText("../../../Data/Medication.json", JsonSerializer.Serialize(reports));
    }

    private static List<Medication> deserialize()
    {
        string filepath = "../../../Data/Medication.json";
        string jsonText = File.ReadAllText(filepath);
        List<Medication> medications = JsonSerializer.Deserialize<List<Medication>>(jsonText);
        return medications;
    }

    public static void addReport(Medication r)
    {
        List<Medication> deserializedMedications = deserialize();
        deserializedMedications.Add(r);
        serialize(deserializedMedications);
    }

}