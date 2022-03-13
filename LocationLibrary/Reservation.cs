using System;

namespace LocationLibrary
{
    public class Reservation
    {
        public Client Client { get; private set; }
        public Voiture Voiture { get; private set; }
        public DateTime DateDebut { get; private set; }
        public DateTime DateFin { get; private set; }
        public int? KilometreEstimation { get; private set; }
        public int? KilometreRealisee { get; set; }

        public Reservation(Client client, Voiture voiture, DateTime datedebut, DateTime datefin, int kilometreEstimation=0, int? kilometreRealisee=null)
        {
            Client = client;
            Voiture = voiture;
            DateDebut = datedebut;
            DateFin = datefin;
            KilometreEstimation = kilometreEstimation;
            KilometreRealisee = kilometreRealisee;
        }
    }
}