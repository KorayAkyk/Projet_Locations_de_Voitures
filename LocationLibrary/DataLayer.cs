using System.Collections.Generic;

namespace LocationLibrary
{
    internal class DataLayer : IDataLayer
    {
        public List<Client> Clients { get; private set; }
        public List<Voiture> Voitures { get; private set; }
        
        public List<Reservation> Reservations { get; private set; }
        public List<Voiture> Voiture { get; }

        public DataLayer(List<Voiture> voitures, List<Voiture> voiture)
        {
            Voitures = voitures;
            Voiture = voiture;
            this.Clients = new List<Client>();
            this.Voitures = new List<Voiture>();
            this.Reservations = new List<Reservation>();
        }
    }
}