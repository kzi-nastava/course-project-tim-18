using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
    class AppointmentRequests
    {
        Appointment newAppointment;
        Appointment oldAppointment;
        typeOfChange typeOfChange;

        public Appointment NewAppointment
        {
            get => newAppointment;
            set => newAppointment = value;
        }

        public Appointment OldAppointment
        {
            get => oldAppointment;
            set => oldAppointment = value;
        }

        public typeOfChange TypeOfChange
        {
            get => typeOfChange;
            set => typeOfChange = value;
        }

        public AppointmentRequests(Appointment oldAppointment, Appointment newAppointment, typeOfChange typeOfChange)
        {
            this.typeOfChange = typeOfChange;
            this.newAppointment = newAppointment;
            this.oldAppointment = oldAppointment;
        }

        public AppointmentRequests()

        {

        }

        public override string ToString()
        {
            return "\nVrijeme: "+ NewAppointment.TimeOfAppointment + "\nDoktor: "  + NewAppointment.Doctor +"\nPacijent: "+ NewAppointment.Patient +"\nTip zahtjeva: "+ TypeOfChange;
        }

        public List<AppointmentRequests> appointmentsRequestDeserialization()
        {
            string fileName = "../../../Data/AppointmentsRequest.json";
            string appointmentRequestFileData = "";
            appointmentRequestFileData = File.ReadAllText(fileName);
            string[] appointmentsRequest = appointmentRequestFileData.Split('\n');
            List<AppointmentRequests> appointmentsRequestList = new List<AppointmentRequests>();
            foreach (String s in appointmentsRequest)
            {
                if (s != "")
                {
                    AppointmentRequests? appointmentRequest = JsonSerializer.Deserialize<AppointmentRequests>(s);
                    if (appointmentRequest != null)
                        appointmentsRequestList.Add(appointmentRequest);

                }
            }
            return appointmentsRequestList;
        }

        public void serializeAppointmentRequest()
        {
            string fileName = "../../../Data/AppointmentsRequest.json";
            List<AppointmentRequests> appointmentsRequest = appointmentsRequestDeserialization();
            string json = "";
            foreach (AppointmentRequests appointmentRequest in appointmentsRequest)
            {
                json += JsonSerializer.Serialize(appointmentRequest) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }

        public void DeletingAppointmentRequest()
        {

            string fileName = "../../../Data/AppointmentsRequest.json";
            List<AppointmentRequests> appointmentRequestList = appointmentsRequestDeserialization();
            string json = "";
            foreach (AppointmentRequests appointmentRequest in appointmentRequestList)
            {
                if (appointmentRequest.NewAppointment.Doctor != this.NewAppointment.Doctor && appointmentRequest.NewAppointment.TimeOfAppointment != this.NewAppointment.TimeOfAppointment)
                    json += JsonSerializer.Serialize(appointmentRequest) + "\n";
            }
            File.WriteAllText(fileName, json);
        }
    }
}