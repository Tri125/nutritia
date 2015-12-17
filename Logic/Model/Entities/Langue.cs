using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutritia
{
    /// <summary>
    /// Classe métier singleton représentant les langues supportés par le logiciel
    /// </summary>
    public class Langue
    {
        private Langue() { }

        /// <summary>
        /// Constructeur paramétrés
        /// </summary>
        /// <param name="nom">Nom conviviale de la langue</param>
        /// <param name="ietf">Tag de langue IETF (en-US)</param>
        private Langue(string nom, string ietf)
        {
            IETF = ietf;
            Nom = nom;
        }

        /// <summary>
        /// Propriété contenant le tag de langue IETF
        /// </summary>
        public string IETF { private set; get; }

        /// <summary>
        /// Propriété contenant un nom conviviale de la langue
        /// </summary>
        public string Nom { private set; get; }


        /// <summary>
        /// Français Canada
        /// fr-CA
        /// </summary>
        public static readonly Langue FrancaisCanada = new Langue("Français Canada", "fr-CA");

        /// <summary>
        /// Anglais États-Unis
        /// en-US
        /// </summary>
        public static readonly Langue AnglaisUSA = new Langue("Anglais États-Unis", "en-US");


        /// <summary>
        /// Méthode statique qui récupère l'instance de l'objet Langue qui correspond au tag IETF envoyé en paramètre
        /// </summary>
        /// <param name="ietf">Tag de langue IETF</param>
        /// <returns></returns>
        public static Langue LangueFromIETF(string ietf)
        {
            if (ietf == AnglaisUSA.IETF)
                return AnglaisUSA;
            else
                return FrancaisCanada;
        }
    }
}
