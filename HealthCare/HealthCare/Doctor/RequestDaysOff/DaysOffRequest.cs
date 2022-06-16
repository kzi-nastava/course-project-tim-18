using System.Text.Json;

namespace HealthCare.Doctor;

public class DaysOffRequest
{
    private DateTime vacationStart;
    private DateTime vacationEnd;
    private bool isUrgent;
    private string requestMessage;
    private RequestState state;
    private string doctorName;
    
    public DateTime VacationStart {
        get => vacationStart;
        set => vacationStart = value;
    }

    public string DoctorName
    {
        get => doctorName;
        set => doctorName = value;
    }
    public DateTime VacationEnd {
        get => vacationEnd;
        set => vacationEnd = value;
    }
    public bool IsUrgent
    {
        get => isUrgent;
        set => isUrgent = value;
    }
    public string RequestMessage
    {
        get => requestMessage;
        set => requestMessage = value;
    }
    public RequestState State
    {
        get => state;
        set => state = value;
    }

    public DaysOffRequest(DateTime vacationStart, DateTime vacationEnd, bool isUrgent, string requestMessage, RequestState state, string doctorName)
    {
        this.vacationStart = vacationStart;
        this.vacationEnd = vacationEnd;
        this.isUrgent = isUrgent;
        this.requestMessage = requestMessage;
        this.state = state;
        this.doctorName = doctorName;
    }

    public static void AddRequest(DaysOffRequest request)
    {
        List<DaysOffRequest> deserializedRequests = Deserialize();
        deserializedRequests.Add(request);
        Serialize(deserializedRequests);
    }
    public static void Serialize(List<DaysOffRequest> requests)
    {
        File.WriteAllText("../../../Data/DaysOffRequests.json", JsonSerializer.Serialize(requests));
    }

    public static List<DaysOffRequest> Deserialize()
    {
        string filepath = "../../../Data/DaysOffRequests.json";
        string jsonText = File.ReadAllText(filepath);
        List<DaysOffRequest> deserializedRequests = JsonSerializer.Deserialize<List<DaysOffRequest>>(jsonText);
        return deserializedRequests;
    }

    public override string ToString()
    {
        return String.Format("Zahtev( pocetak: {0}, kraj: {1}, hitno: {2}, poruka: {3}, stanje: {4}, doktor: {5})", vacationStart, vacationEnd, isUrgent, requestMessage, state, doctorName);
    }
}