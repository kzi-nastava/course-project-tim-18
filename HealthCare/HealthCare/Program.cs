using System.Text.Json;
using System.Xml;
using System;

namespace HealthCare
{


    class Program
    {
        public static DateTime stringToDateTime(string date)
        {
            string[] splitToDateAndTime = date.Split(" ");
            string[] dateAsArray = splitToDateAndTime[0].Split('/');
            string[] timeAsArray = splitToDateAndTime[1].Split(':');
            int day = int.Parse(dateAsArray[0]);
            int month = int.Parse(dateAsArray[1]);
            int year = int.Parse(dateAsArray[2]);
            int hour = int.Parse(timeAsArray[0]);
            int minute = int.Parse(timeAsArray[1]);
            DateTime dateTime = new DateTime(year, month, day);
            dateTime = dateTime.AddHours(hour);
            dateTime = dateTime.AddMinutes(minute);
            //Console.WriteLine(dateTime.ToString("dd/MM/yyyy HH:mm"));
            return dateTime;
        }

        static void Main(string[] args)
        {
            
            Patient patient = new Patient("markop38", "markooo");
            patient.deletingAppointment();
            Appointment.printingAppointment();
            

        }
    }
}
