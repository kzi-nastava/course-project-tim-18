
using System.Text.Json.Serialization;




namespace HealthCare
{
    public class RenovationRequest
    {
        private string roomName;
        private DateTime renovationStart;
        private DateTime renovationEnd;

        [JsonConstructor]
        public RenovationRequest(string roomName, DateTime renovationStart, DateTime renovationEnd)
        {
            this.roomName = roomName;
            this.renovationStart = renovationStart;
            this.renovationEnd = renovationEnd;
        }

        public string RoomName { get => roomName; set => roomName = value; }
        public DateTime RenovationStart { get => renovationStart; set => renovationStart = value; }
        public DateTime RenovationEnd { get => renovationEnd; set => renovationEnd = value; }
    }
}
