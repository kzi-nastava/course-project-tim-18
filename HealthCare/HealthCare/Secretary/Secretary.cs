using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Secretary
{
    public class Secretary:User
    {
        public Secretary(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
