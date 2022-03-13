Feature: Location_Vehicle

Background: 
	Given Les voitures existants sont
	| Marque      | Modèle  | Plaque d'immatriculation | Couleur | Prix  | Prix au Km  | Chevaux Fiscaux |
	| Bugatti     | Chiron  | BB-888-CC                |         | 50.0  | 10          | 213             |
	| Porsche     | 911     |                          |         | 200   | 5           | 29              |
	| BMW         | M4      | FM-455-MF                |         | 50.0  | 7           | 34              |
	|             | GTR     | NG-999-TR                | Rouge   | 150.0 | 5           | 47              |
	| Lamborghini | Huracan | LA-789-HU                | Mat     |       |             |                 |	
	| Ford        | Mondeo  | FM-455-MF                |         | 10    | 2           | 8               |

Scenario: La voiture n'est pas reconnu
	Given Le modèle de la voiture est "Supra"
	When La voiture est validée
	Then La voiture n'existe pas
	And L'erreur est la suivante "La voiture 'Supra' n'existe pas"
	Scenario: Immatriculation non unique
		
Given Le modèle de la voiture est "Mondeo"
When La voiture est validée
Then des informations de la voiture sont manquantes
And L'erreur est la suivante "La plaque d'immatriculation de la voiture 'FM-455-MF' n'est pas unique et La voiture 'Mondeo' n'a pas de couleur valide"

Scenario: Vérification de la couleur de la voiture
Given Le modèle de la voiture est "Chiron"
When La voiture est validée
Then des informations de la voiture sont manquantes
And L'erreur est la suivante "La voiture 'Chiron' n'a pas de couleur valide"

Scenario: Vérification de la marque de la voiture
Given Le modèle de la voiture est "GTR"
When La voiture est validée
Then des informations de la voiture sont manquantes
And L'erreur est la suivante "La voiture 'GTR' n'a pas de marque valide"

Scenario: Vérification du prix, du prix au kilomètre et des chevaux fiscaux de la voiture
Given Le modèle de la voiture est "Huracan"
When La voiture est validée
Then des informations de la voiture sont manquantes
Then L'erreur est la suivante "La voiture 'Huracan' n'a pas de prix valide et Le prix au kilometre n'a pas été renseigné pour la voiture 'Huracan' et Les chevaux fiscaux n'ont pas été saisies pour la voiture 'Huracan'"

Scenario: Creation d'une voiture
When Ajout d'une voiture
And La voiture est validée
Then Les informations de la voiture sont incomplètes
Scenario: Verification de l'immatriculation
	Given Le modèle de la voiture est "911"
	When La voiture est validée
	Then des informations de la voiture sont manquantes
	And L'erreur est la suivante "La voiture '911' n'a pas de plaque d'immatriculation valide et La voiture '911' n'a pas de couleur valide"

