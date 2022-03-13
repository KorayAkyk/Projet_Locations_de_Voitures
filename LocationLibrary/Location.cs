using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace LocationLibrary
{
    public class Location
    {
        private IDataLayer _dataLayer;

        public bool ConnexionUtilisateur { get; private set; }
        public bool UtilisateurIncomplet { get; private set; }
        public Voiture Voiture { get; private set; }
        public List<Voiture> VehiculesDisponibles { get; private set; }
        public bool VéhiculesExistants { get; private set; }
        public bool VéhiculeIncomplet { get; private set; }
        public bool ReservationFaite { get; private set; }
        public Client Client { get; private set; }
        public Reservation Reservation { get; private set; }
        public double PrixCalcule { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        
        public Location(IDataLayer dataLayer)
        {
            this._dataLayer = dataLayer;
        }
        #region User
        public string ConfigUtilisateur(string prenom, string nom)
        {
            string erreur;
            erreur = Empty;
            Client = this._dataLayer.Clients.SingleOrDefault(_ => _.Firstname == prenom && _.Lastname == nom);
            if (IsNullOrEmpty(erreur) && Client != null) return erreur;
            erreur = "L'utilisateur '" + prenom + "' '" + nom + "' est inconnu";
            return erreur;

        }
        public string Connexion(string prenom, string nom, string mdp)
        {
            var client = this._dataLayer.Clients.SingleOrDefault(_ => _.Firstname == prenom && _.Lastname == nom);
            if (client == null)
            {
                ConnexionUtilisateur = false;
                return "Le nom d'utilisateur saisie est invalide";
            }

            if (client.Password == mdp)
            {
                ConnexionUtilisateur = true;
            }
            else
            {
                ConnexionUtilisateur = false;
                return "Le mot de passe saisie est invalide";
            }

            return "";
        }

        public string VerifUtilisateur(string prenom, string nom, string mdp)
        {
            var strError = Empty;
            var client = this._dataLayer.Clients.SingleOrDefault(_ => _.Firstname == prenom && _.Lastname == nom);

            var erreur = VerifAge(client);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";

            erreur = VerifDatePermis(client);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";

            erreur = VerifNumeroPermis(client);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";

            if (strError.Length > 4)
                strError = strError.Substring(0, strError.Length - 4);

            if (IsNullOrEmpty(strError))
                UtilisateurIncomplet = false;

            return strError;
        }

        public string VerifAge(Client client)
        {
            var erreur = Empty;
            if (client.DateNaissance.HasValue) return erreur;
            erreur = "La date de naissance n'existe pas";
            this.UtilisateurIncomplet = true;
            return erreur;
        }

        private string VerifDatePermis(Client client)
        {
            string erreur = Empty;
            if (!client.DatePermis.HasValue)
            {
                erreur = "La date d'obtention du permis de conduire n'existe pas";
                this.UtilisateurIncomplet = true;
            } return erreur;
        }
        public string VerifNumeroPermis(Client client)
        {
            string erreur = Empty;
            if (IsNullOrEmpty(client.NumeroPermis))
            {
                erreur = "Le numéro du permis de conduire n'existe pas";
                this.UtilisateurIncomplet = true;
            } return erreur;
        }
        #endregion

        #region Voiture
        public string ConfigVoiture(string modele)
        {
            var erreur = Empty;
            var dataLayer = this._dataLayer;
            if (dataLayer != null) Voiture = dataLayer.Voitures.SingleOrDefault(_ => _.Modele == modele);
            if (!IsNullOrEmpty(erreur))
            {
                erreur = "Vehicule inconnu";
            } return erreur;
        }
        public string VerifInfoVoiture(string vehicleModel)
        {
            string strError = Empty;
            Voiture voiture = _dataLayer.Voitures.SingleOrDefault(_ => _.Modele == vehicleModel);

            if (voiture == null)
            {
                VéhiculesExistants = false;
                return "La voiture '" + vehicleModel + "' n'existe pas";
            }
            var erreur = VerifPlaqueVoiture(voiture);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";
            erreur = VerifCouleurVoiture(voiture);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";
            erreur = VerifMarqueVoiture(voiture);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";
            erreur = VerifPrixVoiture(voiture);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";
            erreur = VerifKilometre(voiture);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";
            erreur = VerifChevauxVoitures(voiture);
            if (!IsNullOrEmpty(erreur))
                strError += erreur + " et ";
            if (strError.Length > 4)
                strError = strError.Substring(0, strError.Length - 4);
            if (IsNullOrEmpty(strError))
                VéhiculeIncomplet = false;
            return strError;
        }
        private string VerifPlaqueVoiture(Voiture voiture)
        {
            string erreur;
            erreur = Empty;

            if (IsNullOrEmpty(voiture.PlaqueVoiture))
            {
                erreur = "La voiture '" + voiture.Modele + "' n'a pas de plaque d'immatriculation valide";
                VéhiculeIncomplet = true;
            }
            else
            {
                if (this._dataLayer.Voitures.Where(_ => _.PlaqueVoiture == voiture.PlaqueVoiture).ToList().Count > 1)
                {
                    erreur = "La plaque d'immatriculation de la voiture '" + voiture.PlaqueVoiture +
                             "' n'est pas unique";
                    VéhiculeIncomplet = true;
                }
            } return erreur;
        }
        private string VerifCouleurVoiture(Voiture voiture)
        {
            string erreur;
            erreur = Empty;
            if (!IsNullOrEmpty(voiture.Couleur)) return erreur;
            erreur = "La voiture '" + voiture.Modele + "' n'a pas de couleur valide";
            VéhiculeIncomplet = true;
            return erreur;
        }

        private string VerifMarqueVoiture(Voiture voiture)
        {
            string erreur;
            erreur = Empty;
            if (!IsNullOrEmpty(voiture.Marque)) return erreur;
            erreur = "La voiture '" + voiture.Modele + "' n'a pas de marque valide";
            VéhiculeIncomplet = true;
            return erreur;
        }

        private string VerifPrixVoiture(Voiture voiture)
        {
            string erreur;
            erreur = Empty;

            if (voiture.Prix != null) return erreur;
            erreur = "La voiture '" + voiture.Modele + "' n'a pas de prix valide";
            VéhiculeIncomplet = true;

            return erreur;
        }

        private string VerifKilometre(Voiture voiture)
        {
            string erreur;
            erreur = Empty;
            if (voiture.PrixParKilometre != null) return erreur;
            erreur = "Le prix au kilometre n'a pas été renseigné pour la voiture '" + voiture.Modele + "'";
            VéhiculeIncomplet = true;
            return erreur;
        }

        private string VerifChevauxVoitures(Voiture voiture)
        {
            string erreur;
            erreur = Empty;
            if (voiture.ChevauxFiscaux != null) return erreur;
            erreur = "Les chevaux fiscaux n'ont pas été saisies pour la voiture '" + voiture.Modele + "'";
            VéhiculeIncomplet = true;
            return erreur;
        }

        public string VerifVoitureDispo(DateTime dateDebutLoc, DateTime dateFinLoc)
        {
            string erreur;
            erreur = Empty;
            Reservations = _dataLayer.Reservations.Where(_ => (_.DateDebut >= dateDebutLoc && _.DateFin <= dateFinLoc) || (_.DateDebut <= dateDebutLoc && _.DateFin >= dateDebutLoc) || (_.DateDebut <= dateFinLoc && _.DateFin >= dateFinLoc)).ToList();
            List<Voiture> voitures;
            voitures = _dataLayer.Voitures.Where(v => !Reservations.Exists(_ => _.Voiture.Modele == v.Modele)).ToList();
            VehiculesDisponibles = voitures;
            return erreur;
        }
        #endregion

        #region Reservation
        public string ConfigReservation(Client client, Voiture voiture, DateTime dateDebutLoc, DateTime dateFinLoc)
        {
            string erreur;
            erreur = Empty;
            Reservation = this._dataLayer.Reservations.SingleOrDefault(_ =>
                _.Client == client && _.Voiture == voiture && _.DateDebut == dateDebutLoc && _.DateFin == dateFinLoc);
            if (Reservation != null) return erreur;
            erreur = "Reservation inconnue";
            return erreur;
        }
        public string VerifReservation(DateTime dateDebutLoc, DateTime dateFinLoc)
        {
            string message;
            message = Empty;
            Reservations = _dataLayer.Reservations.Where(_ =>
                (_.DateDebut >= dateDebutLoc && _.DateFin <= dateFinLoc) ||
                (_.DateDebut <= dateDebutLoc && _.DateFin >= dateDebutLoc) ||
                (_.DateDebut <= dateFinLoc && _.DateFin >= dateFinLoc)).ToList();
            if (Reservations.Count != 0) return message;
            message = "Il n'y a aucune reservation entre ces dates";
            return message;
        }

        public string CreationReservation(Client client, Voiture voiture, DateTime startDate, DateTime endDate, int kilometreEstimation)
        {
            string erreur;
            erreur = Empty;
            var agelimiteChevaux21 = 8;
            var agelimiteChevaux25 = 13;

            if (client.DateNaissance > DateTime.Now.AddYears(-18))
                erreur = "L'utilisateur '" + client.Firstname + " " + client.Lastname + "' a moins de 18 ans. ";
            else
            {
                if (client.DateNaissance > DateTime.Now.AddYears(-21) && voiture.ChevauxFiscaux > agelimiteChevaux21)
                    erreur += "L'utilisateur '" + client.Firstname + " " + client.Lastname +
                              "' ne peut pas louer un véhicule de plus de " + agelimiteChevaux21 + " chevaux fiscaux. ";
                else
                {
                    if (client.DateNaissance < DateTime.Now.AddYears(-21) &&
                        client.DateNaissance > DateTime.Now.AddYears(-25) &&
                        voiture.ChevauxFiscaux > agelimiteChevaux25)
                        erreur += "L'utilisateur '" + client.Firstname + " " + client.Lastname +
                                  "' ne peut pas louer un véhicule de plus de " + agelimiteChevaux21 +
                                  " chevaux fiscaux.";
                }
            }
            if (!IsNullOrEmpty(erreur))
                ReservationFaite = false;
            else
            {
                Reservation = new Reservation(client, voiture, startDate, endDate, kilometreEstimation);
                ReservationFaite = true;
            } return erreur;
        }

        public string CalculPrix(Voiture v, int kilometre)
        {
            string erreur;
            erreur = Empty;
            switch (v.Prix)
            {
                case null:
                    erreur = "Le prix du véhicule n'est pas renseigné. ";
                    break;
            }
            if (v.PrixParKilometre == null)
                erreur += "Le prix au km du véhicule n'est pas renseigné. ";

            if (!IsNullOrEmpty(erreur)) return erreur;
            // ReSharper disable once PossibleInvalidOperationException
            PrixCalcule = (double)(v.Prix + v.PrixParKilometre * kilometre);
            return erreur;
        }

        public void Remboursement(Reservation r, int kilometreFait)
        {
            var erreur = Empty;
            if (r.KilometreEstimation == null)
                erreur = "Les kms estimés ne sont pas renseignés. ";
            else
            {
                if (r.KilometreRealisee == null)
                    erreur += "Les kms effectués ne sont pas renseignés. ";
                else
                {
                    if (r.Voiture.PrixParKilometre == null)
                        erreur += "Le prix au km n'est pas renseigné. ";
                }
            }
            if (!IsNullOrEmpty(erreur)) return;
            // ReSharper disable once PossibleInvalidOperationException
            PrixCalcule = (double)((kilometreFait - r.KilometreEstimation) * r.Voiture.PrixParKilometre);
        }
        public void KilometreVraimentFait(Reservation res, int nombresKilometres)
        {
            _dataLayer.Reservations.Remove(res);
            if (res == null) return;
            res.KilometreRealisee = nombresKilometres;
            _dataLayer.Reservations.Add(res);
            if (_dataLayer != null && _dataLayer.Reservations.Count(_ => _.Voiture == res.Voiture && _.Client == res.Client && _.DateDebut == res.DateDebut &&
                                                                         _.DateFin == res.DateFin &&
                                                                         _.KilometreRealisee == nombresKilometres) ==
                // ReSharper disable once RedundantJumpStatement
                1) return;
        }
        #endregion
    }
}
