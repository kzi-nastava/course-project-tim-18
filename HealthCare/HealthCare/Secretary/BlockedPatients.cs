using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare
{
    public class BlockedPatients
    {
        private BlockedType blockedType;
        private Patient patient;

        public BlockedPatients(BlockedType blockedType, Patient patient)
        {
            this.blockedType = blockedType;
            this.patient = patient;
        }

        public BlockedType BlockedType 
        { 
            get { return blockedType; } 
            set { blockedType = value; }
        }
        public Patient Patient
        {
            get { return patient; }
            set { patient = value; }
        }
        public override string ToString()
        {
            return BlockedType + "," + patient.username + "," + patient.password;
        }
    }
}
