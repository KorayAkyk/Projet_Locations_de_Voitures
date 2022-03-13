using System;

namespace LocationLibrary
{
    public class Voiture
    {
        public string Marque { get; private set; }
        public string Modele { get; private set; }
        public string PlaqueVoiture { get; private set; }
        public string Couleur { get; set; }
        public double? Prix { get; set; }
        public double? PrixParKilometre { get; set; }
        public int? ChevauxFiscaux { get; set; }

        public Voiture(string marque, string modele, string plaquevoiture, string couleur, string prix, string prixparkilometre, string chevauxciscaux)
        {
            Marque = marque;
            Modele = modele;
            PlaqueVoiture = plaquevoiture;
            Couleur = couleur;
            if (!string.IsNullOrEmpty(prix))
                Prix = double.Parse(prix);
            if (!string.IsNullOrEmpty(prixparkilometre))
                PrixParKilometre = double.Parse(prixparkilometre);
            if (string.IsNullOrEmpty(chevauxciscaux)) return;
            ChevauxFiscaux = int.Parse(chevauxciscaux);
        }
    }
}