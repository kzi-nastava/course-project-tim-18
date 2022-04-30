
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



/*

bool showMenu = true;
    while (showMenu)
    {
       showMenu = menu.WriteManu();
    }




Console.WriteLine("aasa");*/