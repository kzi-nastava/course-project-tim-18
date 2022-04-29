<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Patient;
=======
﻿
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
>>>>>>> Doctor

namespace HealthCare.Doctor
{
    class Doctor : User
    {
<<<<<<< HEAD
        public string Name;
        public string Surname;
        private List<Appointment> doctorAppointmentList;
=======
        private string name;
        private string surname;
        private string? id;
        private List<Appointment>? appointments;
>>>>>>> Doctor

        public Doctor() {
            username = "";
            password = "";
<<<<<<< HEAD
            doctorAppointmentList = null;
        }

        public Doctor(string username, string password, List<Appointment> doctorAppointmentList)
        {
            this.username = username;
            this.password = password;
            this.doctorAppointmentList = doctorAppointmentList;
=======
            appointments = new List<Appointment>();
            id = "";
            name = "";
            surname  = "";
        }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }

        public List<Appointment> Appointments { get => appointments; set => appointments = value; }
        

        [JsonConstructor]
        public Doctor(string username, string password,string name, string surname, string id, List<Appointment> appointments)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.id = id;
            this.appointments = appointments;
        }
    
        public override string ToString()
        {
            return String.Format("Doctor( Name: {0}, Surname: {1}, ID: {2}, Username: {3}, Password: {4}, Appointments: [{5}])", name, surname, id, username, password, String.Join("; ",appointments));
        }
        public void AddAppointment(Appointment appointment)
        {
            this.appointments.Add(appointment);
        }
        public bool CreateAppointment()
        {
            Console.Write("Unesite  ime i prezime pacijenta:  ");
            string patientFullName = Console.ReadLine();
            if (patientFullName == "") // TODO or check if username exists
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            Console.Write("Unesite datum i vreme pregleda pregleda(format = dd/MM/yyyy HH:mm):  ");
            string period = Console.ReadLine();
            if (period == "")
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            if (checkIfAvailable(DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null))) // TODO regex for datetime and check if doctor is available
            {
                Console.WriteLine("Doktor nije dostupan za dat termin.");
                return false;
            }
            Console.WriteLine("Izaberite tip termina:");
            Console.WriteLine("1. Operacija");
            Console.WriteLine("2. Pregled");
            string type = Console.ReadLine();
            if (type != "1" || type != "2")
            {
                Console.WriteLine("Neodgovarajuc unos");
                return false;
            }
            // TODO patient = new Patient(); 
            this.appointments.Add(new Appointment(this.username, patientFullName, DateTime.ParseExact(period, "dd/MM/yyyy HH:mm", null), (AppointmentType)Int32.Parse(type)-1));
            return true;
        }

        private bool checkIfAvailable(DateTime date)
        {
            foreach (var a in appointments)
            {
                if ((a.DateTime - date).Minutes > 0  && (a.DateTime - date).Minutes < 15)
                {
                    return false;
                }
            }
            return true;
        }
        public void readAppointments()
        {
            foreach (Appointment a in appointments)
            {
                Console.WriteLine(a);
            }
        }

        public void Serialize()
        {
            
        }
        public void deleteAppointment(int index)
        {
            appointments.RemoveAt(index);
        }

        public void CheckSchedule(DateTime? chosenDate)
        {
            if (!chosenDate.HasValue)
            {
                chosenDate = DateTime.Today;
            }
            
            for (int i = 0;i < appointments.Count;i++)
            {
                if ((appointments[i].DateTime - chosenDate) < TimeSpan.FromDays(4) && appointments[i].DateTime > chosenDate)
                {
                    Console.WriteLine(i);
                    Console.Write("Datum pregleda/operacije: ");
                    Console.WriteLine(appointments[i].DateTime);
                    Console.WriteLine("Zdravstveni karton pacijenta: ");
                    Console.WriteLine(appointments[i].Patient); // TODO add patient info from class
                }
            }
            
>>>>>>> Doctor
        }
    }
    
}
