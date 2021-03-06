
using HealthCare;
using HealthCare.Doctor;
using HealthCare.Patient;
using HealthCare.Secretary;



 Manager manager = new Manager();

 manager.Load();

 List<Doctor> doctors = Doctor.Deserialize();

 List<Patient> patients = Patient.patientDeserialization();

 List<SecretaryService> secretaries = SecretaryService.Deserialize();
 SecretaryController secretaryController = new SecretaryController();


 string userResponse;

 while (true)
 {
     do
     {
         Console.WriteLine();
         Console.WriteLine("1.Uloguj se");
         Console.WriteLine("2.Isključi aplikaciju");
         Console.WriteLine("Izaberi opciju: ");
         userResponse = Console.ReadLine();

     } while (userResponse != "1" && userResponse != "2");
     

     if (userResponse == "2")
         break;



     Console.WriteLine();

     Console.Write("Unesi korisnčko ime:");
     string username = Console.ReadLine();

     Console.Write("Unesi šifru:");
     string password = Console.ReadLine();


     if(manager.Username == username && manager.Password == password)
     {
        
         manager.DoctorMenu();
     }



     foreach (Doctor doctor in doctors)
     {
         if (doctor.Username == username && doctor.Password == password)
         {
            SecretaryService.SendNotificationToDoctor(username);
            doctor.DoctorMenu(manager);
            break;
         }
     }

     foreach (SecretaryService secretary in secretaries)
     {
         if (secretary.Username == username && secretary.Password == password)
         {
            secretaryController.WriteManu(manager);
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



 manager.Save();

 Doctor.Serialize(doctors);

 Patient.Serialize(patients);

 SecretaryJDBC.Serialize(secretaries);

 Console.WriteLine("Aplikacija ugašena.");
 