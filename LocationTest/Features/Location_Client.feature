Feature: Location
	
Background: 
	Given Les clients enregistrés
	  | Prénom  | Nom     | Mot de passe | Date de naissance | Date obtention permis | Numéro permis |
	  | Koray   | AKYUREK | 123456789    |                   |                       |               |
	  | Nicolas | ROSSAT  | 987654321    | 1999-01-01        |                       |	             |


Scenario: Connexion de l'utilisateur - Nom d'utilisateur non valide
    Given Le nom de l'utilisateur est "Koraille" "AKYUREEK"
    And Le mot de passe de l'utilisateur est "123456789"
    When L'utilisateur se connecte
    Then La connexion de l'utilisateur a été refusé
    And Le message d'erreur est le suivant : "Le nom d'utilisateur saisie est invalide"

Scenario: Connexion de l'utilisateur - Nom d'utilisateur valide
    Given Le nom de l'utilisateur est "Koray" "AKYUREK"
    And Le mot de passe de l'utilisateur est "123456789"
    When L'utilisateur se connecte
    Then La connexion a été validée

Scenario: Connexion de l'utilisateur - Nom d'utilisateur valide mais mot de passe non valide
    Given Le nom de l'utilisateur est "Koray" "AKYUREK"
    And Le mot de passe de l'utilisateur est "968496841"
    When L'utilisateur se connecte
    Then La connexion de l'utilisateur a été refusé
    And Le message d'erreur est le suivant : "Le mot de passe saisie est invalide"

Scenario: Informations de l'utilisateur - Informations manquantes
    Given Le nom de l'utilisateur est "Koray" "AKYUREK"
    And Le mot de passe de l'utilisateur est "123456789"
    When L'utilisateur se connecte
    And L'utilisateur consulte ses informations
    Then Le profil de l'utilisateur est incomplet
    And Le message d'erreur est le suivant : "La date de naissance n'existe pas et La date d'obtention du permis de conduire n'existe pas et Le numéro du permis de conduire n'existe pas"

Scenario: Creation de l'utilisateur
    When Création de l'utilisateur
    And L'utilisateur consulte ses informations
    Then Le profil de l'utilisateur est complet et valide

    