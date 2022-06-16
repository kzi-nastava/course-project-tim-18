using System.Text.Json;

namespace HealthCare.Doctor.PerformAppointment;

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

}