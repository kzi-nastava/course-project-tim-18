using System;
namespace HealthCare
{
    public class Manager : User
    {
        private Hospital hospital;

        public Manager(string username, string password, Hospital hospital)
        {
            this.username = username;
            this.Password = password;
            this.hospital = hospital;
        }
        
        public Hospital Hospital
        {
            get => hospital;
            set => hospital = value ?? throw new ArgumentNullException(nameof(value));
        }



   
    }
}
