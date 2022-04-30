using System.Text.Json;
using HealthCare.Patient;
using HealthCare.Secretary;

namespace HealthCare.Doctor;

public class Report
{
    private Appointment appointment;
    private string description;
    private MedicalRecord medicalRecord;
    public Appointment Appointment
    {
        get => appointment;
        set => appointment = value;
    }

    public MedicalRecord MedicalRecord
    {
        get => medicalRecord;
        set => medicalRecord = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }
    public Report(){}

    public Report(Appointment appointment, string description, MedicalRecord medicalRecord)
    {
        Appointment = appointment;
        Description = description;
        MedicalRecord = medicalRecord;
    }

    private static void serialize(List<Report> reports)
    {
        File.WriteAllText("../../../Data/Reports.json", JsonSerializer.Serialize(reports));
    }

    private static List<Report> deserialize()
    {
        string path = "../../../Data/Reports.json";
        string jsonText = File.ReadAllText(path);
        List<Report> reports = JsonSerializer.Deserialize<List<Report>>(jsonText);
        return reports;
    }

    public static void addReport(Report r)
    {
        List<Report> reports = deserialize();
        reports.Add(r);
        serialize(reports);
    }
}