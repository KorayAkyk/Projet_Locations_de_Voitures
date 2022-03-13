Feature: Location_Reservation

Background:     
    Given Les voitures disponibles sont
    | Marque  | Modèle | Plaque d'immatriculation | Couleur | Prix  | Prix au Km | Chevaux Fiscaux |
    | Porsche | 911    | MM-911-BG                | Blanche | 200   | 0.9        | 29              |
    | Bmw     | M4     | BM-489-WW                | Noir    | 200   | 0.5        | 34              |
    | Bugatti | Chiron | BB-888-CC                | Bleu    | 200   | 0.9        | 213             |
    | Nissan  | GTR    | NG-999-TR                | Gris    | 150.0 | 0.8        | 47              |
    | Peugeot | 206    | OP-531-PM                | Gris    | 10    | 0.1        | 4               |
    | Ford    | Mondeo | FM-455-MF                | Gris    | 200   | 0.2        | 8               |

    And Les utilisateurs sont
    | Prénom  | Nom     | Mot de passe | Date de naissance | Date obtention permis | Numéro permis |
    | Koray   | AKYUREK | 123456789    | 1999-05-05        | 2018-11-11            | 1968DC45      |
    | Nicolas | ROSSAT  | 987654321    | 1980-01-01        | 2017-08-25            | 9415AD14      |
    And Les réservations sont
    | Prénom Utilisateur | Nom Utilisateur | Modèle Vehicule | Date de début | Date de fin | Kilomètres estimés | Kilomètres réalisés |
    | Koray              | AKYUREK         |  911            | 2022-02-12    | 2022-02-15  | 200                | 150                 |
    | Nicolas            | ROSSAT          |  911            | 2022-02-16    | 2022-02-17  | 800                | 900                 |
    | Nicolas            | ROSSAT          |  Mondeo         | 2022-02-11    | 2022-02-11  | 800                | 400                 |
    | Koray              | AKYUREK         |  GTR            | 2022-02-14    | 2022-02-16  | 200                | 150                 |
    | Koray              | AKYUREK         |  911            | 2022-02-09    | 2022-02-11  | 400                | 500                 |

Scenario: Réservation d'une voiture avec plus de 8 chevaux fiscaux avec un utilisateur entre 21 et 25 ans
	Given L'âge de l'utilisateur est entre 21 et 25 ans
	And L'utilisateur veut louer une voiture du '2022-02-10' au '2022-02-16'
	When L'utilisateur consulte les véhicules disponibles
	Then Les voitures disponibles sont les suivantes
	  | Marque  | Modèle | Plaque d'immatriculation | Couleur | Prix | Prix au Km | Chevaux Fiscaux |
	  | Bugatti | Chiron | BB-888-CC                | Bleu    | 200  | 0.9        | 213             |
	  | Peugeot | 206    | OP-531-PM                | Gris    | 10   | 0.1        | 4               |
	  | Bmw     | M4     | BM-489-WW                | Noir    | 200  | 0.5        | 34              |

	Given L'utilisateur a choisie la voiture 'Chiron'
	And L'utilisateur veut faire environ 300 kms
	When L'utilisateur souhaite faire une réservation
	Then La réservation de l'utilisateur a été refusée
	And Le message d'erreur est le suivant : 'L'utilisateur 'Valentin CROSSET' ne peut pas louer un véhicule de plus de 8 chevaux fiscaux.'

Scenario: Réservation d'une voiture disponible avec un utilisateur valide
	Given L'âge de l'utilisateur est entre 21 et 25 ans
	And L'utilisateur veut louer une voiture du '2022-02-10' au '2022-02-16'
	When L'utilisateur consulte les véhicules disponibles
	Then Les voitures disponibles sont les suivantes
	  | Marque  | Modèle | Plaque d'immatriculation | Couleur | Prix | Prix au Km | Chevaux Fiscaux |
	  | Bugatti | Chiron | BB-888-CC                | Bleu    | 200  | 0.9        | 213             |
	  | Peugeot | 206    | OP-531-PM                | Gris    | 10   | 0.1        | 4               |
	  | Bmw     | M4     | BM-489-WW                | Noir    | 200  | 0.5        | 34              | 

	Given L'utilisateur a choisie la voiture '206'
	And L'utilisateur veut faire environ 1500 kms
	When L'utilisateur souhaite faire une réservation
	Then La réservation de l'utilisateur a été validée
	When L'utilisateur consulte le prix
	Then L'utilisateur a payé 160 euros
	Given L'utilisateur a fait réellement 1000 kms
	When L'utilisateur consulte le prix final
	Then 50 euros ont été remboursé à l'utilisateur
	
Scenario: Réservation : Information sur les dates de réservations
	Given Connexion de l'utilisateur valide
	And L'utilisateur veut louer une voiture du '2022-02-10' au '2022-02-16'
	When L'utilisateur consulte les réservations disponibles
	Then Les reservations entre ces dates sont les suivantes : 
	  | Prénom Utilisateur | Nom Utilisateur | Modèle Vehicule | Date de début | Date de fin |
	  | Koray              | AKYUREK         |  GTR            | 2022-02-14    | 2022-02-16  |
	  | Koray              | AKYUREK         |  911            | 2022-02-09    | 2022-02-11  |
	  | Nicolas            | ROSSAT          |  911            | 2022-02-16    | 2022-02-17  |
	  | Nicolas            | ROSSAT          |  Mondeo         | 2022-02-11    | 2022-02-11  |
	  | Koray              | AKYUREK         |  911            | 2022-02-12    | 2022-02-15  |
	


