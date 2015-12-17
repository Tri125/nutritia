using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Nutritia
{
    public class MySqlVersionLogicielService : IVersionLogicielService
    {
        private MySqlConnexion connexion;

        /// <summary>
        /// Méthode construisant un objet VersionLogiciel à partir d'un DataRow de réponse d'une requête MySql
        /// </summary>
        /// <param name="version">DataRow de réponse d'une requête MySql</param>
        /// <returns></returns>
        private VersionLogiciel ConstruireVersionLogiciel(DataRow version)
        {
            return new VersionLogiciel(version["version"].ToString(), version["changelog"].ToString(), version["downloadLink"].ToString(), (DateTime)version["datePublication"]);
        }

        /// <summary>
        /// Méthode qui récupère la VersionLogiciel la plus récente de la base de données à partir d'une VIEW
        /// </summary>
        /// <returns></returns>
        public VersionLogiciel RetrieveLatest()
        {
            try
            {
                VersionLogiciel latestVersion;
                connexion = new MySqlConnexion();
                string requete = "SELECT * FROM CurrentVersion";

                DataSet dataSetVersionsLogiciel = connexion.Query(requete);
                DataTable tableVersionsLogiciel = dataSetVersionsLogiciel.Tables[0];

                latestVersion = ConstruireVersionLogiciel(tableVersionsLogiciel.Rows[0]);
                return latestVersion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new VersionLogiciel("0.0.0.0", "Empty", "Empty", DateTime.MinValue);
            }

        }
    }
}
