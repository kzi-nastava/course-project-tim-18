using System;
namespace HealthCare
{
    public class Manager : User
    {
        private Hospital hospital;

        public Manager(string username, string password, Hospital hospital)
        {
            this.username = username;
            this.password = password;
            this.hospital = hospital;
        }
    }
}
