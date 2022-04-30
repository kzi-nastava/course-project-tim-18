
using HealthCare;
using HealthCare.Doctor;
using HealthCare.Patient;
using HealthCare.Secretary;



Secretary s1 = new Secretary("milica", "milica123");
Secretary s2 = new Secretary("alex", "acke123");
Secretary s3 = new Secretary("nina", "nina123");
Secretary s4 = new Secretary("lesnina", "lesnina123");


List<Secretary> ls = new List<Secretary>() { s1,s2,s3,s4};


Secretary.Serialize(ls);

Manager m = new Manager();
m.Load();

List<Doctor> doctors = Doctor.Deserialize();

List<Patient> patients = Patient.patientDeserialization();

List<Secretary> secretaries = Secretary.Deserialize();

string userResponse;

while (true)
{

    Console.WriteLine("1.Uloguj se");
    Console.WriteLine("2.Isključi aplikaciju");
    Console.WriteLine("Izaberi opciju: ");

    userResponse = Console.ReadLine();

    if (userResponse == "2")
        break;

    Console.WriteLine("Unesi korisnčko ime:");
    string username = Console.ReadLine();

    Console.WriteLine("Unesi šifru:");
    string password = Console.ReadLine();

    foreach (Doctor doctor in doctors)
    {
        if (doctor.Username == username && doctor.Password == password)
        {
            doctor.DoctorMenu();
            break;
        }
    }

    foreach (Secretary secretary in secretaries)
    {
        if (secretary.Username == username && secretary.Password == password)
        {
            secretary.WriteManu();
            break;
        }
    }


    foreach (Patient patinet in patients)
    {
        if (patinet.Username == username && patinet.Password == password)
        {
            patinet.patientMenu();
            break;
        }
    }
}


