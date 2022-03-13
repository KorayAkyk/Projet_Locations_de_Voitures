using LocationLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationTest.Fake
{
    class FakeDataLayer : IDataLayer
    {
        public List<Client?> Clients { get; set; }
        public List<Voiture> Voitures { get; }
        public List<Reservation> Reservations { get; set; }
        public List<Voiture>? Voiture { get; }

        public FakeDataLayer()
        {
            this.Clients = new List<Client?>();
            this.Voitures = new List<Voiture>();
            this.Reservations = new List<Reservation>();
        }
    }
}
