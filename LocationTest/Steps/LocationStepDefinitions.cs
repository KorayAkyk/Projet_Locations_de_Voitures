using TechTalk.SpecFlow;
using FluentAssertions;
using LocationLibrary;
using LocationTest.Fake;

namespace LocationTest.Steps
{
    [Binding]
    public sealed class LocationStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;
        private string? _prenom;
        private string? _nom;
        private string? _mdp;
        private string? _lastErrorMessage;
        private Location _target;
        private FakeDataLayer _fakeDataLayer;
        private DateTime _dateDebut;
        private DateTime _dateFin;
        private int _kilometreEstimation;
        private int _kilometreRealisee;
        private double _prix;
        private Voiture? _voiture;
        private Client? _client;
        private Reservation? _reservation;
        private List<Reservation>? _reservations;
        private List<Voiture>? _voitures;
        private string? _voitureChoisie;
        
        public LocationStepDefinition(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _fakeDataLayer = new FakeDataLayer();
            _target = new Location(_fakeDataLayer);
        }
        
        #region Given

        [Given(@"Les clients enregistrés")]
        public void GivenLesClientsDejaExistants(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                TableRow row = table.Rows[index];
                _fakeDataLayer.Clients.Add(new Client(row["Prénom"], row["Nom"], row["Mot de passe"],
                    row["Date de naissance"], row["Date obtention permis"], row["Numéro permis"]));
            }
        }

        [Given(@"Le nom de l'utilisateur est ""(.*)"" ""(.*)""")]
        public void GivenMonNomEst(string? prenom, string? nom)
        {
            _prenom = prenom;
            _nom = nom;
        }

        [Given(@"Le mot de passe de l'utilisateur est ""(.*)""")]
        public void GivenMonMotDePasseEst(string? motDePasse) => _mdp = motDePasse;

        [Given(@"Les utilisateurs sont")]
        public void GivenLesClientsSont(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                TableRow row = table.Rows[index];
                if (_fakeDataLayer != null)
                    _fakeDataLayer.Clients.Add(new Client(row["Prénom"], row["Nom"], row["Mot de passe"],
                        row["Date de naissance"], row["Date obtention permis"], row["Numéro permis"]));
            }
        }

        [Given(@"Les voitures disponibles sont")]
        public void GivenLesVehiculesSont(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                TableRow row = table.Rows[index];
                _fakeDataLayer.Voitures.Add(new Voiture(row["Marque"], row["Modèle"],
                    row["Plaque d'immatriculation"], row["Couleur"], row["Prix"], row["Prix au Km"],
                    row["Chevaux Fiscaux"]));
            }
        }

        [Given(@"Les réservations sont")]
        public void GivenLesReservationsSont(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                TableRow row = table.Rows[index];
                _lastErrorMessage +=
                    _target.ConfigUtilisateur(row["Prénom Utilisateur"], row["Nom Utilisateur"]);
                _lastErrorMessage += _target.ConfigVoiture(row["Modèle Vehicule"]);

                if (string.IsNullOrEmpty(_lastErrorMessage))
                {
                    Client client = _target.Client;
                    Voiture voiture = _target.Voiture;

                    _fakeDataLayer.Reservations.Add(new Reservation(client, voiture,
                        DateTime.Parse(row["Date de début"]), DateTime.Parse(row["Date de fin"]),
                        int.Parse(row["Kilomètres estimés"]), int.Parse(row["Kilomètres réalisés"])));
                }
            }
        }

        [Given(@"Connexion de l'utilisateur valide")]
        public void GivenLutilisateurSeConnecteAvecUnCompteValide()
        {
            _client = new Client("Valentin", "CROSSET", "9684894984", "2001-08-01", "2018-09-01", "888AD154");
            _fakeDataLayer.Clients.Add(_client);
        }

        [Given(@"L'âge de l'utilisateur est entre 21 et 25 ans")]
        public void GivenLutilisateurSeConnecteAvecUnCompteValideEntre21Et25Ans()
        {
            _client = new Client("Valentin", "CROSSET", "9684894984", "2000-08-05", "2018-01-08", "781AA111");
            _fakeDataLayer.Clients.Add(_client);
        }
        
        [Given(@"L'utilisateur veut louer une voiture du '(.*)' au '(.*)'")]
        public void GivenLutilisateurSouhaiteUnVehiculeDuAu(string startDate, string endDate)
        {
            _dateDebut = DateTime.Parse(startDate);
            _dateFin = DateTime.Parse(endDate);
        }

        [Given(@"L'utilisateur a choisie la voiture '(.*)'")]
        public void GivenLutilisateurSelectionneLeVehicule(string model)
        {
            if (_lastErrorMessage != null) _lastErrorMessage += _target.ConfigVoiture(model);
            _voiture = _target.Voiture;
        }

        [Given(@"L'utilisateur veut faire environ (.*) kms")]
        public void GivenLutilisateurCompteEffectuerKms(int nbKms)
        {
            _kilometreEstimation = nbKms;
        }

        [Given(@"L'utilisateur a fait réellement (.*) kms")]
        public void GivenLutilisateurRendLaVoitureAvecKms(int nbKms)
        {
            _reservation = _target.Reservation;
            _target.KilometreVraimentFait(_reservation, nbKms);
            _kilometreRealisee = nbKms;
        }


        [Given(@"Les voitures existants sont")]
        public void GivenLesVehiculesExistantsSont(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                TableRow row = table.Rows[index];
                _fakeDataLayer?.Voitures.Add(new Voiture(row["Marque"], row["Modèle"],
                    row["Plaque d'immatriculation"], row["Couleur"], row["Prix"], row["Prix au Km"],
                    row["Chevaux Fiscaux"]));
            }
        }
        [Given(@"Le modèle de la voiture est ""(.*)""")]
        public void GivenLeModeleeSelectionneEst(string? selectedVehicle) => _voitureChoisie = selectedVehicle;
        
        #endregion
        
        #region When

        [When(@"L'utilisateur se connecte")]
        public void WhenJEssaieDeMeConnecterAMonCompte() => _lastErrorMessage = _target.Connexion(_prenom, _nom, _mdp);

        [When(@"L'utilisateur consulte ses informations")]
        public void WhenJeSouhaiteConnaitreMesInformations()
        {
            if (!string.IsNullOrEmpty(_lastErrorMessage)) return;
            _lastErrorMessage = _target.VerifUtilisateur(_prenom, _nom, _mdp);
        }

        [When(@"Création de l'utilisateur")]
        public void WhenUnUtilisateurEstCree()
        {
            _prenom = "Valentin";
            _nom = "CROSSET";
            _mdp = "9684894984";
            _fakeDataLayer.Clients.Add(new Client(_prenom, _nom, _mdp, "2000-08-05","2018-04-26", "781-AA-111"));
        }

        [When(@"L'utilisateur consulte les réservations disponibles")]
        public void WhenLutilisateurVeutConnaitreLesReservations()
        {
            if (_target != null)
            {
                _lastErrorMessage = _target.VerifReservation(_dateDebut, _dateFin);
                if (_target != null) _reservations = _target.Reservations;
            }
        }

        [When(@"L'utilisateur consulte les véhicules disponibles")]
        public void WhenLutilisateurVeutConnaitreLesVehiculesDisponibles()
        {
            if (_target != null)
            {
                _lastErrorMessage = _target.VerifVoitureDispo(_dateDebut, _dateFin);
                _voitures = _target.VehiculesDisponibles;
            }
        }

        [When(@"L'utilisateur souhaite faire une réservation")]
        public void WhenLutilisateurSouhaiteReserver()
        {
            _lastErrorMessage = _target.CreationReservation(_client, _voiture, _dateDebut,
                _dateFin, _kilometreEstimation);
        }

        [When(@"L'utilisateur consulte le prix")]
        public void WhenLutilisateurVeutConnaitreLePrix()
        {
            _lastErrorMessage = _target.CalculPrix(_voiture, _kilometreEstimation);
            _prix = _target.PrixCalcule;
        }

        [When(@"L'utilisateur consulte le prix final")]
        public void WhenLutilisateurVeutConnaitreLePrixFinal()
        {
            _target.Remboursement(_reservation, _kilometreRealisee);
            _prix = _target.PrixCalcule;
        }

        [When(@"Ajout d'une voiture")]
        public void WhenUnVehiculeEstCree()
        {
            _voitureChoisie = "RS6";
            _fakeDataLayer.Voitures.Add(new Voiture("Audi", this._voitureChoisie, "AD-789-DR", "Noir",
                "605", "21", "441"));
        }

        [When(@"La voiture est validée")]
        public void WhenLeVéhiculeEstVerifie() => _lastErrorMessage = _target.VerifInfoVoiture(_voitureChoisie);

        #endregion
        
        #region Then

        [Then(@"La connexion de l'utilisateur a été refusé")]
        public void ThenLaConnexionEstRefusee() => _target.ConnexionUtilisateur.Should().BeFalse();

        [Then(@"Le profil de l'utilisateur est complet et valide")]
        public void ThenMonProfilEstComplet() => _target.UtilisateurIncomplet.Should().BeFalse();

        [Then(@"Le profil de l'utilisateur est incomplet")]
        public void ThenMonProfilEstIncomplet() => _target.UtilisateurIncomplet.Should().BeTrue();

        [Then(@"Le message d'erreur est le suivant : ""(.*)""")]
        public void ThenLeMessageDerreurEst(string errorMessage) => _lastErrorMessage.Should().Be(errorMessage);

        [Then(@"La connexion a été validée")]
        public void ThenLaConnexionEstEtablie() => _target.ConnexionUtilisateur.Should().BeTrue();

        [Then(@"Les reservations entre ces dates sont les suivantes :")]
        public void ThenLesReservationsEntreCesDatesSont(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                var row = table.Rows[index];
                _target.ConfigUtilisateur(row["Prénom Utilisateur"], row["Nom Utilisateur"]);
                Client client;
                client = this._target.Client;

                _target.ConfigVoiture(row["Modèle Vehicule"]);
                Voiture voiture;
                voiture = this._target.Voiture;

                _target.ConfigReservation(client, voiture, DateTime.Parse(row["Date de début"]),
                    DateTime.Parse(row["Date de fin"]));
                Reservation reservation;
                reservation = this._target.Reservation;

                row["Prénom Utilisateur"].Should().Be(reservation.Client.Firstname);
                row["Nom Utilisateur"].Should().Be(reservation.Client.Lastname);
                row["Modèle Vehicule"].Should().Be(reservation.Voiture.Modele);


                DateTime.Parse(row["Date de début"]).Should().Be(reservation.DateDebut);
                DateTime.Parse(row["Date de fin"]).Should().Be(reservation.DateFin);
            }
        }

        [Then(@"Les voitures disponibles sont les suivantes")]
        public void ThenLesVehiculesDisponiblesSont(Table table)
        {
            for (var index = 0; index < table.Rows.Count; index++)
            {
                TableRow row = table.Rows[index];
                this._target.ConfigVoiture(row["Modèle"]);
                Voiture v = this._target.Voiture;

                row["Marque"].Should().Be(v.Marque);
                row["Modèle"].Should().Be(v.Modele);
                row["Plaque d'immatriculation"].Should().Be(v.PlaqueVoiture);
                row["Couleur"].Should().Be(v.Couleur);
                double.Parse(row["Prix"]).Should().Be(v.Prix);
                double.Parse(row["Prix au Km"]).Should().Be(v.PrixParKilometre);
                int.Parse(row["Chevaux Fiscaux"]).Should().Be(v.ChevauxFiscaux);
            }
        }

        [Then(@"La réservation de l'utilisateur a été validée")]
        public void ThenLaReservationEstValidee() => _target.ReservationFaite.Should().Be(true);

        [Then(@"La réservation de l'utilisateur a été refusée")]
        public void ThenLaReservationEstRefusee() => _target.ReservationFaite.Should().Be(false);

        [Then(@"Le message d'erreur est le suivant : '(.*)'")]
        public void ThenLeMessageEst(string error) => _lastErrorMessage.Should().Be(error);

        [Then(@"L'utilisateur a payé (.*) euros en plus")]
        [Then(@"(.*) euros ont été remboursé à l'utilisateur")]
        [Then(@"L'utilisateur a payé (.*) euros")]
        public void ThenLeClientPaieEuros(int price) => Math.Abs(this._prix).Should().Be(price);

        [Then(@"des informations de la voiture sont manquantes")]
        public void ThenLesDonneesDeLaVoitureSontManquantes() => _target.VéhiculeIncomplet.Should().BeTrue();

        [Then(@"La voiture n'existe pas")]
        public void ThenLeVehiculeNexistePas() => _target.VéhiculesExistants.Should().BeFalse();

        [Then(@"Les informations de la voiture sont incomplètes")]
        public void ThenLesDonneesDuVehiculeSontCompletes() => _target.VéhiculeIncomplet.Should().BeFalse();

        [Then(@"L'erreur est la suivante ""(.*)""")]
        public void ThenLErreurEst(string error) => _lastErrorMessage.Should().Be(error);

        #endregion
        
    }
}
