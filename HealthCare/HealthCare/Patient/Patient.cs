﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
    public class Patient : User
    {

        public Patient(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public static List<Patient> patientDeserialization()
        {
            string fileName = "../../../Data/Patients.json";
            string PatientFileData = "";
            PatientFileData = File.ReadAllText(fileName);
            string[] Patients = PatientFileData.Split('\n');
            List<Patient> PatientsList = new List<Patient>();
            foreach (String s in Patients)
            {
                if (s != "")
                {
                    Patient? Patient = JsonSerializer.Deserialize<Patient>(s);
                    if (Patient != null)
                        PatientsList.Add(Patient);

                }
            }
            return PatientsList;
        }

        public void blockingPatient()
        {
            string fileName = "../../../Data/Appointments.json";
            List<Patient> patients = patientDeserialization();
            string json = "";
            foreach (Patient patient in patients)
            {
                if (patient.Username != this.Username && patient.Password != this.Password)
                    json += JsonSerializer.Serialize(patient) + "\n";
            }
            File.WriteAllText(fileName, json);
            BlockedPatients blocked = new BlockedPatients(BlockedType.Patient, this);
            blocked.serializeBlockedPatient();
        }

        public void serializePatient()
        {
            string fileName = "../../../Data/Patients.json";
            List<Patient> Patients = patientDeserialization();
            string json = "";
            foreach (Patient Patient in Patients)
            {
                json += JsonSerializer.Serialize(Patient) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }

        public void makingAppointment()
        {
            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            AntiTrolCounter counter = new AntiTrolCounter(this.Username, dateInString, typeOfChange.Create);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Console.WriteLine("Unesite ime doktora kod koga zelite tretman: ");
                string doctor = Console.ReadLine();
                Console.WriteLine("Unesite vreme tretmana koji zelite da zakazete:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                if (Appointment.isAppointmentValid(timeOfAppointment, doctor) == true)
                {
                    Appointment appointment = new Appointment(timeOfAppointment, doctor, this.Username);
                    appointment.serializeAppointment();
                }
            }
            else
            {
                Console.WriteLine("Prevelik broj zakazivanja novih tretmana vas nalog ce sada biti blokiran: ");
            }
        }

        public void changingAppointment()
        {

            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            AntiTrolCounter counter = new AntiTrolCounter(this.Username, dateInString, typeOfChange.Update);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Appointment.printingAppointment();
                Console.WriteLine("Unesite vreme tretmana koji zelite da izmenite:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                Console.WriteLine("Unesite ime doktora kod koga zelite da izmenite tretman: ");
                string doctor = Console.ReadLine();
                List<Appointment> appointments = Appointment.appointmentsDeserialization();
                bool validationOfNewAppointment = false;
                int? indexOfRequest = 0;
                for (int i = 0; i < appointments.Count; i++)
                {
                    if (appointments[i].TimeOfAppointment == timeOfAppointment && appointments[i].Doctor == doctor)
                    {
                        indexOfRequest = i;
                        Console.WriteLine("Unesite broj ispred opcije:\n1 Promena doktora\n2 Promena vremena termina:\nSve drugo za kraj");
                        string option = Console.ReadLine();
                        if (option == "1")
                        {
                            Console.WriteLine("Unesite ime doktora kod koga zelite novi termin: ");
                            string newDoctor = Console.ReadLine();
                            appointments[i].Doctor = newDoctor;
                            if (Appointment.isAppointmentValid(appointments[i].TimeOfAppointment, appointments[i].Doctor))
                                validationOfNewAppointment = true;
                        }
                        if (option == "2")
                        {
                            Console.WriteLine("Unesite vreme novog termina: ");
                            string newTimeOfAppointment = Console.ReadLine();
                            appointments[i].TimeOfAppointment = newTimeOfAppointment;
                            if (Appointment.isAppointmentValid(appointments[i].TimeOfAppointment, appointments[i].Doctor))
                                validationOfNewAppointment = true;
                        }
                    }
                }
                DateTime timeChecked = Program.stringToDateTime(timeOfAppointment);
                TimeSpan timeDifference = timeChecked.Subtract(DateTime.Now);
                if (validationOfNewAppointment == true)
                    if (timeDifference.TotalDays > 2)
                        Appointment.serializingListOfAppointments(appointments);
                    else
                    {
                        AppointmentRequest appointmentRequest = new AppointmentRequest(appointments[indexOfRequest], typeOfChange.Update);
                        appointmentRequest.serializeAppointmentRequest();
                    }
                Appointment.serializingListOfAppointments(appointments);
            }
            else
            {
                this.blockingPatient();
                Console.WriteLine("Prevelik broj promena vas nalog je sada blokiran: ");
            }
        }

        public void deletingAppointment()
        {
            DateTime now = DateTime.Now;
            string dateInString = now.ToString("dd/MM/yyyy HH:mm");
            AntiTrolCounter counter = new AntiTrolCounter(this.Username, dateInString, typeOfChange.Update);
            bool validationOfAntiTrol = counter.validation();
            if (validationOfAntiTrol == true)
            {
                Console.WriteLine("Unesite ime doktora kod koga zelite da izbrisete tretman: ");
                string doctor = Console.ReadLine();
                Console.WriteLine("Unesite vreme tretmana koji zelite da izbrisete:(u formatu DD/MM/YYYY hh:mm ");
                string timeOfAppointment = Console.ReadLine();
                Appointment appointment = new Appointment(timeOfAppointment, doctor, this.Username);
                DateTime timeChecked = Program.stringToDateTime(timeOfAppointment);
                TimeSpan timeDifference = timeChecked.Subtract(DateTime.Now);
                if(timeDifference.TotalDays > 2)
                    appointment.deletingAppointment();
                else
                {
                    AppointmentRequest appointmentRequest = new AppointmentRequest(appointment, typeOfChange.Delete);
                    appointmentRequest.serializeAppointmentRequest();
                }

            }
            else
            {
                this.blockingPatient();
                Console.WriteLine("Prevelik broj brisanja vas nalog je sada blokiran: ");
            }
        }
    }
}
