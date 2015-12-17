using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Nutritia.Logic.Model.Entities;

namespace Nutritia
{
    public class MySqlDonService : IDonService
    {
        private MySqlConnexion connexion;

        /// <summary>
        /// Méthode qui récupère tout les dons contenu dans la base de données
        /// </summary>
        /// <returns>IList de tout les dons</returns>
        public IList<Don> RetrieveAll()
        {
            IList<Don> resultat = new List<Don>();

            try
            {
                connexion = new MySqlConnexion();
                string requete = "SELECT * FROM AllDons";

                DataSet dataSetDon = connexion.Query(requete);
                DataTable tableDon = dataSetDon.Tables[0];

                foreach (DataRow don in tableDon.Rows)
                {
                    resultat.Add(ConstruireDon(don));
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            return resultat;
        }

        /// <summary>
        /// Méthode qui insère dans la base de données un Don
        /// </summary>
        /// <param name="don">Don à insérer</param>
        public void Insert(Don don)
        {
            try
            {
                connexion = new MySqlConnexion();
                string requete = string.Format("INSERT INTO Dons (idModePaiement, nom, montant, noTransaction) VALUES ( (SELECT idModePaiement FROM ModesPaiement WHERE nom = '{0}'), '{1}', {2}, '{3}')", don.ModePaiementTransaction, don.NomAuteur, don.Montant, don.NoTransaction);
                connexion.Query(requete);
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        /// <summary>
        /// Méthode qui construit un objet Don à partir d'un DataRow
        /// </summary>
        /// <param name="don">DataRow de réponse d'une requête MySql</param>
        /// <returns></returns>
        private Don ConstruireDon(DataRow don)
        {
            DateTime DateHeureTransaction = (DateTime)don["dateDon"];
            float Montant = (float)don["montant"];
            string NomAuteur = (string)don["Auteur"];
            //À partir du nom, la méthode StringToValue de ModePaiement retournera l'instance recherché pour le type de carte.
            ModePaiement mode = ModePaiement.StringToValue((string)don["ModePaiement"]);
            return new Don(NomAuteur, Montant, mode, DateHeureTransaction);
        }

        /// <summary>
        /// Méthode permettant d'associer un don à un membre
        /// </summary>
        /// <param name="membre">Membre envoyant le don</param>
        /// <param name="transaction">Don à insérer dans la base de données</param>
        public void Insert(Membre membre, Don transaction)
        {
            //Appel la méthode qui ne fait qu'insérer le Don dans la bd.
            Insert(transaction);
            //Puis on exécute le code qui rajoutera les données nécessaire dans la table de correspondance DonsMembres pour associé le don au membre.
            try
            {
                connexion = new MySqlConnexion();
                string requete = string.Format("INSERT INTO DonsMembres (idMembre, idDon) VALUES ( (SELECT idMembre FROM Membres WHERE nomUtilisateur = '{0}'), (SELECT idDon FROM Dons WHERE noTransaction = '{1}')) ", membre.NomUtilisateur, transaction.NoTransaction);
                connexion.Query(requete);
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        /// <summary>
        /// Méthode appelant une VIEW pour obtenir le DateTime du don le plus récents dans la base de données
        /// </summary>
        /// <returns></returns>
        public DateTime LastTimeDon()
        {
            DateTime last = DateTime.MinValue;
            try
            {
                connexion = new MySqlConnexion();
                string requete = " SELECT * FROM LastTimeDon";

                DataSet dataSetDon = connexion.Query(requete);
                DataTable tableDon = dataSetDon.Tables[0];
                if (!tableDon.Rows[0].IsNull("derniereMaj"))
                    last = (DateTime)tableDon.Rows[0]["derniereMaj"];
            }
            catch (MySqlException)
            {
                throw;
            }
            return last;
        }
    }
}
