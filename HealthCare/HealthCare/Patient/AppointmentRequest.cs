using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare
{
    class AppointmentRequest
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

        public AppointmentRequest(Appointment oldAppointment, Appointment newAppointment, typeOfChange typeOfChange)
        {
            this.typeOfChange = typeOfChange;
            this.newAppointment = newAppointment;
            this.oldAppointment = oldAppointment;
        }

        public static List<AppointmentRequest> appointmentsRequestDeserialization()
        {
            string fileName = "../../../Data/AppointmentsRequest.json";
            string appointmentRequestFileData = "";
            appointmentRequestFileData = File.ReadAllText(fileName);
            string[] appointmentsRequest = appointmentRequestFileData.Split('\n');
            List<AppointmentRequest> appointmentsRequestList = new List<AppointmentRequest>();
            foreach (String s in appointmentsRequest)
            {
                if (s != "")
                {
                    AppointmentRequest? appointmentRequest = JsonSerializer.Deserialize<AppointmentRequest>(s);
                    if (appointmentRequest != null)
                        appointmentsRequestList.Add(appointmentRequest);

                }
            }
            return appointmentsRequestList;
        }

        public void serializeAppointmentRequest()
        {
            string fileName = "../../../Data/AppointmentsRequest.json";
            List<AppointmentRequest> appointmentsRequest = appointmentsRequestDeserialization();
            string json = "";
            foreach (AppointmentRequest appointmentRequest in appointmentsRequest)
            {
                json += JsonSerializer.Serialize(appointmentRequest) + "\n";
            }
            json += JsonSerializer.Serialize(this) + "\n"; ;
            File.WriteAllText(fileName, json);
        }



    }
}
