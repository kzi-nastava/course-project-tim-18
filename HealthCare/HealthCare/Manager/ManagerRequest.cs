using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;



namespace HealthCare
{
    public class ManagerRequest
    {
        private Room oldRoom;
        private Room newRoom;
        private Equipment equipment;
        private int amount;
        private DateTime executionDate;
        private bool executed;

        [JsonConstructor]
        public ManagerRequest(Room oldRoom, Room newRoom, Equipment equipment, int amount, DateTime executionDate, bool executed)
        {
            this.oldRoom = oldRoom;
            this.newRoom = newRoom;
            this.equipment = equipment;
            this.amount = amount;
            this.executionDate = executionDate;
            this.executed = executed;
            
        }


        public int Amount { get => amount; set => amount = value; }
        public DateTime ExecutionDate { get => executionDate; set => executionDate = value; }
        public Room OldRoom { get => oldRoom; set => oldRoom = value; }
        public Room NewRoom { get => newRoom; set => newRoom = value; }
        public Equipment Equipment { get => equipment; set => equipment = value; }
        public bool Executed { get => executed; set => executed = value; }
    }
}
