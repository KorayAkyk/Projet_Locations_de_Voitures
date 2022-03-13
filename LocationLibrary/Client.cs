using System;

namespace LocationLibrary
{
    public class Client
    {
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Password { get; private set; }
        public DateTime? DateNaissance { get; set; }
        public DateTime? DatePermis { get; set; }
        public string NumeroPermis { get; set; }

        public Client(string firstname, string lastname, string password, string datenaissance, string datepermis, string numeropermis)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Password = password;
            this.NumeroPermis = numeropermis;
            if(!string.IsNullOrEmpty(datenaissance))
                this.DateNaissance = DateTime.Parse(datenaissance);
            if(!string.IsNullOrEmpty(datepermis))
                this.DatePermis = DateTime.Parse(datepermis);
        }
    }
}