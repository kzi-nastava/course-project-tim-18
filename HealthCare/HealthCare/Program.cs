
using HealthCare;
using HealthCare.Doctor;
using HealthCare.Patient;
using HealthCare.Secretary;



// Manager manager = new Manager();
//
// manager.Load();
//
// List<Doctor> doctors = Doctor.Deserialize();
//
// List<Patient> patients = Patient.patientDeserialization();
//
// List<Secretary> secretaries = Secretary.Deserialize();
//
//
// string userResponse;
//
// while (true)
// {
//     do
//     {
//         Console.WriteLine();
//         Console.WriteLine("1.Uloguj se");
//         Console.WriteLine("2.Isključi aplikaciju");
//         Console.WriteLine("Izaberi opciju: ");
//         userResponse = Console.ReadLine();
//
//     } while (userResponse != "1" && userResponse != "2");
//     
//
//     if (userResponse == "2")
//         break;
//
//
//
//     Console.WriteLine();
//
//     Console.Write("Unesi korisnčko ime:");
//     string username = Console.ReadLine();
//
//     Console.Write("Unesi šifru:");
//     string password = Console.ReadLine();
//
//
//     if(manager.Username == username && manager.Password == password)
//     {
//         manager.DoctorMenu();
//     }
//
//
//
//     foreach (Doctor doctor in doctors)
//     {
//         if (doctor.Username == username && doctor.Password == password)
//         {
//             doctor.DoctorMenu();
//             break;
//         }
//     }
//
//     foreach (Secretary secretary in secretaries)
//     {
//         if (secretary.Username == username && secretary.Password == password)
//         {
//             secretary.WriteManu();
//             break;
//         }
//     }
//
//
//     foreach (Patient patinet in patients)
//     {
//         if (patinet.Username == username && patinet.Password == password)
//         {
//             patinet.patientMenu();
//             break;
//         }
//     }
// }
//
//
//
// manager.Save();
//
// Doctor.Serialize(doctors);
//
// Patient.Serialize(patients);
//
// Secretary.Serialize(secretaries);
//
// Console.WriteLine("Aplikacija ugašena.");
// List<Appointment> appointments = Appointment.appointmentsDeserialization();
List<Appointment> apps = new List<Appointment>();
apps.Add(new Appointment("29/04/2022 15:43", "1", "markop39", AppointmentType.Operation, "105"));
apps.Add(new Appointment("11/05/2022 16:30", "1", "markop39", AppointmentType.Examination, "A1"));
Doctor doc = new Doctor("1", "1", "1", "1", apps, "A1", DoctorSpecialization.Cardiologist);
List<Doctor> doctors = new List<Doctor>();


doctors.Add(doc);
Doctor.Serialize(doctors);

Console.Write(":D");
    
