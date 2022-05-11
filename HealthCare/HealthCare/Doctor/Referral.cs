using System.Text.Json;

namespace HealthCare.Doctor;

public class Referral
{
    private string doctor;
    private string pacient;

    public string Doctor {
        get => doctor;
        set => doctor = value;
    }

    public string Pacient
    {
        get => pacient;
        set => pacient = value;
    }
    public Referral(){}
    public Referral(string doctor, string pacient)
    {
        this.doctor = doctor;
        this.pacient = pacient;
    }
    private static void serialize(List<Referral> referrals)
    {
        File.WriteAllText("../../../Data/Referrals.json", JsonSerializer.Serialize(referrals));
    }

    private static List<Referral> deserialize()
    {
        string filepath = "../../../Data/Referrals.json";
        string jsonText = File.ReadAllText(filepath);
        List<Referral> referrals = JsonSerializer.Deserialize<List<Referral>>(jsonText);
        return referrals;
    }

    public static void addReferral(Referral r)
    {
        List<Referral> deserializeedReferrals = deserialize();
        deserializeedReferrals.Add(r);
        serialize(deserializeedReferrals);
    }

}