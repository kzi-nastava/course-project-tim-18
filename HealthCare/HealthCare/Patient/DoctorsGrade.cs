namespace HealthCare.Patient;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using HealthCare.Doctor;
public class DoctorsGrade
{
    private string comment;
    private int wouldYouSuggest;
    private int howGoodDoctorWas;
    private string doctor;
    
    public int HowGoodDoctorWas
    {
        get => howGoodDoctorWas;
        set => howGoodDoctorWas = value;
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
    
    public string Doctor
    {
        get => doctor;
        set => doctor = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public DoctorsGrade(string comment, int wouldYouSuggest, int howGoodDoctorWas, string doctor)
    {
        this.howGoodDoctorWas = howGoodDoctorWas;
        this.wouldYouSuggest = wouldYouSuggest;
        this.comment = comment;
        this.doctor = doctor;
    }
    
    public static List<DoctorsGrade> DeserializeDoctorsGrade()
    {
        string fileName = "../../../Data/DoctorsGrade.json";
        string DoctorsGradeFileData = "";
        DoctorsGradeFileData = File.ReadAllText(fileName);
        string[] DoctorsGrades = DoctorsGradeFileData.Split('\n');
        List<DoctorsGrade> DoctorsGradeList = new List<DoctorsGrade>();
        foreach (String s in DoctorsGrades)
        {
            if (s != "")
            {
                DoctorsGrade? doctorsGrade = JsonSerializer.Deserialize<DoctorsGrade>(s);
                if (doctorsGrade != null)
                    DoctorsGradeList.Add(doctorsGrade);

            }
        }
        return DoctorsGradeList;
    }

    public void SerializeDoctorsGrade()
    {
        string fileName = "../../../Data/DoctorsGrade.json";
        List<DoctorsGrade> doctorsGrades = DeserializeDoctorsGrade();
        string json = "";
        foreach (DoctorsGrade doctorsGrade in doctorsGrades)
        {
            json += JsonSerializer.Serialize(doctorsGrade) + "\n";
        }
        json += JsonSerializer.Serialize(this) + "\n"; ;
        File.WriteAllText(fileName, json);
    }
    
}