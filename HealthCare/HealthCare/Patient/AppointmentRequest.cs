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
        Appointment appointment;
        typeOfChange typeOfChange;

        public Appointment Appointment
        {
            get => Appointment;
            set => Appointment = value;
        }

        public typeOfChange TypeOfChange
        {
            get => typeOfChange;
            set => typeOfChange = value;
        }

        public AppointmentRequest(Appointment appointment, typeOfChange typeOfChange)
        {
            this.typeOfChange = typeOfChange;
            this.appointment = appointment;
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
