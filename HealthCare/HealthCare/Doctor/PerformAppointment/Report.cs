using System.Text.Json;
using HealthCare.Patient;
using HealthCare.Secretary;

namespace HealthCare.Doctor.PerformAppointment;

public class Report
{
    private Patient.CreatingAppointment.Appointment appointment;
    private string description;
    private Secretary.MedicalRecord patientMedicalRecord;

    public Patient.CreatingAppointment.Appointment Appointment
    {
        get => appointment;
        set => appointment = value;
    }

    public MedicalRecord MedicalRecord
    {
        get => patientMedicalRecord;
        set => patientMedicalRecord = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }
    public Report(){}

    public Report(Patient.CreatingAppointment.Appointment appointment, string description, MedicalRecord medicalRecord)
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
        string filepath = "../../../Data/Reports.json";
        string jsonText = File.ReadAllText(filepath);
        List<Report> reports = JsonSerializer.Deserialize<List<Report>>(jsonText);
        return reports;
    }

    public static List<Report> deserializeForPatient(string username)
    {
        string filepath = "../../../Data/Reports.json";
        string jsonText = File.ReadAllText(filepath);
        List<Report> reports = JsonSerializer.Deserialize<List<Report>>(jsonText);
        List<Report> patientsReports =new  List<Report>();
        foreach(Report report in reports)
        {
            if (report.appointment.Patient == username)
            {
                patientsReports.Add(report);
            }
        }
        return patientsReports;
    }
    
    public static void addReport(Report r)
    {
        List<Report> deserializeedReports = deserialize();
        deserializeedReports.Add(r);
        serialize(deserializeedReports);
    }
    
    public static void MergeReport(ref List<Report> reports , int l, int m, int r)
            {
                int n1 = m - l + 1;
                int n2 = r - m;
        
                List<Report> L = new List<Report>( new Report[n1]);
                List<Report> R = new List<Report>( new Report[n2]);
                int i, j;
      
                for (i = 0; i < n1; ++i)
                    L[i] = reports[l + i];
                for (j = 0; j < n2; ++j)
                    R[j] = reports[m + 1 + j];
      
                i = 0;
                j = 0;
      
                int k = l;
                while (i < n1 && j < n2) {
                    if (DateTime.Compare(Patient.CreatingAppointment.Appointment.stringToDateTime(L[i].Appointment.TimeOfAppointment), Patient.CreatingAppointment.Appointment.stringToDateTime(R[j].Appointment.TimeOfAppointment)) >= 0) 
                    {
                        reports[k] = L[i];
                        i++;
                    }
                    else {
                        reports[k] = R[j];
                        j++;
                    }
                    k++;
                }
      
                while (i < n1) {
                    reports[k] = L[i];
                    i++;
                    k++;
                }
          
                while (j < n2) {
                    reports[k] = R[j];
                    j++;
                    k++;
                }
            }
            
                    
    
            public static void SortReport(ref List<Report> reports, int l, int r)
            {
                if (l < r) {
                    int m = l+ (r-l)/2;
        
                    SortReport(ref(reports), l, m);
                    SortReport(ref(reports), m + 1, r);
                
                    MergeReport(ref(reports), l, m, r);
                }
            }
}