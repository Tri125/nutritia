﻿# Nutritia
﻿
[![Build status](https://ci.appveyor.com/api/projects/status/axic4ds0j9mcl3dc/branch/develop?svg=true)](https://ci.appveyor.com/project/Tri125/nutritia)
[![Version number](https://img.shields.io/badge/Version-2.0.1.0-blue.svg)](https://github.com/Nutritia/nutritia/releases)

Nutritia est une application permettant de générer automatique un menu pour vos repas de la
semaine ou pour quelques jours, le tout selon vos préférences et restrictions alimentaires.
Également, vous pouvez utiliser l’application pour calculer la valeur nutritive de vos repas ou même
pour faire votre liste d’épicerie.

L'application inclus un guide d'utilisateur et la majorité des fenêtres sont disponibles en anglais.
Pour changer de langue, allez dans la fenêtre des paramètres.

Un compte administrateur est créé avec la base de données:

| Nom de compte | Mot de passe  |
| ------------- |:-------------:|
| admin         | admin         |


### Progression du travail

[![Throughput Graph](https://graphs.waffle.io/nutritia/nutritia/throughput.svg)](https://waffle.io/nutritia/nutritia/metrics)

### Procédure de déploiement du logiciel Nutritia

#### Note de départ

Les *fichiers de configurations* sont: 
* `Nutritia_Creation.sql`
* `Nutritia_Insertion.sql`
* `Nutritia.exe.config`

Pour utiliser Nutritia, il est nécessaire d'avoir une connexion à une base de données MySql.

Veuillez vous assurez d'avoir accès à un base de données MySql avec des privilèges de création, d'insertion et de sélection pour pouvoir créer la base de données. Il est aussi nécessaire d'avoir le privilège d'update pour utiliser le logiciel Nutritia:

| Privilege     | Column        | Context                         |
| ------------- |:-------------:| -----:                          |
| CREATE        | Create_priv   | databases, tables, or indexes   |
| INSERT        | Insert_priv   |   tables or columns             |
| SELECT        | Select_priv   |    tables or columns            |
| UPDATE        | Update_priv   |    tables or columns            |
	
Le script de création créera la base de donnée avec le nom `420-5A5-A15_Nutritia` par défault, mais il est possible de choisir son propre nom. Il suffit de modifier les lignes contenant `420-5A5-A15_Nutritia`dans les *fichiers de configurations* par le nom désiré.

### Installation

- Connectez-vous sur votre serveur de base de données avec votre logiciel de développement de votre choix.

  - Lancer le script de création de base de données `Nutritia_Creation.sql`

  - Lancer le script d'insertion de données `Nutritia_Insertion.sql`


- **Depuis 1.5.0.0**.

Cette option fonctionne uniquement si vous avez déjà installé la base de données localement et que vous voulez vous connecter sur un autre serveur. Vous pouvez uniquement changer de serveur si vous êtes actuellement administrateur, si vous êtes administrateur sur le nouveu serveur, si votre nom d'utilisateur et mot de passe est le même sur le nouveau serveur. Si vous ne pouvez pas installer la BD localement, voir l'option deux qui modifie manuellement le fichier de config. Cette deuxième option n'est plus supporté depuis 1.5.0.0 et entrainera des erreurs.

   1. Lancer l'application
   2.  Connectez-vous sur un compte administrateur
   3.  Aller dans le menu Paramètres
   4.  Cliquer sur "Connexion", cette option est désactivé si vous n'êtes pas administrateur.
   5.  Remplisser les champs. Le champ "Nom de session" est un nom commun qui vous servira à vous rappeler de la session.
   6.  Cliquer sur "Sauvegarde"
   7.  À partir de la liste, sélectionné votre nouvelle session
   8.  Cliquer sur "Connexion"
   9.  Une fois connecté, le nom de la session sera en gras.


- **Alternative qui n'est plus supporté depuis 1.5.0.0** 

Dans le dossier contenant l'exécutable Nutritia.exe, modifier le fichier `Nutritia.exe.config`.

   1. Localiser la ligne:
`            <setting name="ActiveSession" serializeAs="String">
                <value>{name=local;server=localhost;userid=root;password=;database=420-5A5-A15_Nutritia}</value>
            </setting>`
   2.  Remplacer `localhost` par l'adresse de votre serveur de base de données. Alternativement, laissez le à `localhost` si votre serveur est local.
   3.  Remplacer `root` par votre nom d'utilisateur.
   4.  Si votre compte utilise un mot de passe, rajouter votre mot de passe suivant `password=`, mais avant le point-virgule.
   5.  Si vous avez remplacé le nom de la base de données, remplacer `420-5A5-A15_Nutritia` par le nom choisi.
   6.  Si cette option est utilisé et que vous allez à la fenêtre de Connexion à partir des paramètres, l'application lancera une exception.

### Démonstration

![Menu Principal](https://i.imgur.com/mVn9HBi.png)

Menu principal.

![Menu de connexion](https://i.imgur.com/mccHgfl.png)

Menu de connexion.

![Menu principal connecté](https://i.imgur.com/tdkf0VE.png)

Menu principal une fois connecté.

![Profil](https://i.imgur.com/OJpbjQz.png)

Profil du compte d'utilisateur.

![Calculatrice nutritionnelle](https://i.imgur.com/J3RioXD.png)

Calculatrice nutritionnelle.

![Générateur de menus](https://i.imgur.com/S0L9qR3.png)

Générateur de menus.

![Liste d'épicerie](https://i.imgur.com/FRD5MNG.png)

Liste d'épicerie.

![Ajout de plat](https://i.imgur.com/EdINuJo.png)

Ajout d'un plat par un utilisateur.

![Menu de vote](https://i.imgur.com/54gMqWY.png)

Menu de vote de plats.

![Menu d'administration](https://i.imgur.com/PjVUcj0.png)

![Guide d'utilisateur](https://i.imgur.com/2oR2YyN.png)

Guide d'utilisateur.

![Changement de langues](https://i.imgur.com/qyeCejj.png)

Changement de langues.

![Changement de connexion bd](https://i.imgur.com/S58FVpz.png)

Changement de connexion bd.