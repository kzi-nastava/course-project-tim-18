using System;
namespace HealthCare
{
    public class User
    {
        protected string username;
        protected string password;

        public string Username
        {
            get => username;
            set => username = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Password
        {
            get => password;
            set => password = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
