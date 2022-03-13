using System;
using System.Collections.Generic;
using System.Text;

namespace LocationLibrary
{
    public interface IDataLayer
    {
        List<Client> Clients { get; }
        List<Voiture> Voitures { get; }
        List<Reservation> Reservations { get; }
    }
}
