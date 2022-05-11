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

    public Referral(string doctor, string pacient)
    {
        this.doctor = doctor;
        this.pacient = pacient;
    }

}