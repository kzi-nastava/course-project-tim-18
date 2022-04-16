using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Secretary
{
    public class PatientAccountCRUD
    {
        private PatientAccount account;

        public void CreatePatient()
        {
            Console.WriteLine("TO DO: Create patient");
        }
        public void ReadPatient()
        {
            Console.WriteLine("TO DO: Read patient");
        }
        public void Update()
        {
            Console.WriteLine("TO DO: Update patient");
        }
        public void Delete()
        {
            Console.WriteLine("TO DO: Delete patient");
        }

        public PatientAccount Account
        {
            get { return account; }
            set { account = value; }
        }
    }
}
