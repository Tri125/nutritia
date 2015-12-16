using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutritia.Logic.Model.Entities
{
    /// <summary>
    /// Struct représentant une session de connexion à un serveur MySql
    /// </summary>
    public struct Session : IEquatable<Session>
    {
        /// <summary>
        /// Hostname ou adresse IP
        /// </summary>
        public string HostName_IP { get; private set; }

        /// <summary>
        /// Nom conviviale pour se souvenir de la session
        /// </summary>
        public string Nom { get; private set; }

        /// <summary>
        /// Nom d'utilisateur ayant accès au serveur MySql
        /// </summary>
        public string NomUtilisateur { get; private set; }

        /// <summary>
        /// Mot de passe du compte de l'utilisateur ayant accès au serveur MySql
        /// </summary>
        public string MotDePasse { get; private set; }

        /// <summary>
        /// Port du serveur MySql
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Nom de la base de données/schema Nutritia qui sera utilisé
        /// </summary>
        public string NomBD { get; private set; }

        /// <summary>
        /// Struct représentant une session de connexion à un serveur MySql
        /// </summary>
        /// <param name="nom">Nom conviviale pour se souvenir de la session</param>
        /// <param name="host">Hostname ou adresse IP du serveur MySql</param>
        /// <param name="user">Nom d'utilisateur du compte ayant accès au serveur MySql</param>
        /// <param name="password">Mot de passe du compte d'utilisateur ayant accès au serveur MySql</param>
        /// <param name="database">Nom de la base de données (schema) Nutritia sur le serveur MySql</param>
        /// <param name="port">Port du serveur MySql</param>
        public Session(string nom, string host, string user, string password, string database, int port = 3306)
            : this()
        {
            HostName_IP = host;
            Nom = nom;
            NomUtilisateur = user;
            MotDePasse = password;
            NomBD = database;
            Port = port;
        }

        /// <summary>
        /// Format name=x;server=y;userid=z;password=;database=a
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("name={0};server={1};userid={2};password={3};database={4}", Nom, HostName_IP, NomUtilisateur, MotDePasse, NomBD);
            return sb.ToString();
        }

        /// <summary>
        /// Retourne une string dans le format de connexion string de MySql
        /// server=y;userid=z;password=;database=a
        /// </summary>
        /// <returns></returns>
        public string ToConnexionString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("server={0};userid={1};password={2};database={3}", HostName_IP, NomUtilisateur, MotDePasse, NomBD);
            return sb.ToString();
        }

        #region IEquatable

        public override int GetHashCode()
        {
            return Tuple.Create(Nom, HostName_IP, NomUtilisateur, MotDePasse, Port, NomBD).GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (object.ReferenceEquals(right, null))
                return false;

            if (object.ReferenceEquals(this, right))
                return true;

            if (this.GetType() != right.GetType())
                return false;

            return this.Equals((Session)right);
        }

        public bool Equals(Session other)
        {
            return (this.NomBD == other.NomBD &&
            this.Nom == other.Nom &&
            this.HostName_IP == other.HostName_IP &&
            this.MotDePasse == other.MotDePasse &&
            this.Port == other.Port &&
            this.NomUtilisateur == other.NomUtilisateur);
        }

        public static bool operator ==(Session left, Session right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Session left, Session right)
        {
            return !(left == right);
        }

        #endregion
    }
}
