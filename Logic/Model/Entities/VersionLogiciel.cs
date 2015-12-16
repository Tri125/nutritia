using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutritia
{
    /// <summary>
    /// Struct représentant une version du logiciel pour lorsqu'une nouvelle version est disponible et une notification est affiché avec ces informations
    /// </summary>
    public struct VersionLogiciel
    {
        /// <summary>
        /// Numéro de version en string
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Texte détaillant les modifications apportées
        /// </summary>
        public string Changelog { get; private set; }

        /// <summary>
        /// URL où la version peut être téléchargé
        /// </summary>
        public string DownloadLink { get; private set; }

        /// <summary>
        /// Date et heure à partir laquel la version a été publié
        /// </summary>
        public DateTime DatePublication { get; private set; }

        public VersionLogiciel(string version, string changeLog, string downloadLink, DateTime datePublication)
            :this()
        {
            Version = version;
            Changelog = changeLog;
            DownloadLink = downloadLink;
            DatePublication = datePublication;
        }
    }
}
