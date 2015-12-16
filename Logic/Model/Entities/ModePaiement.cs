using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutritia.Logic.Model.Entities
{
    /// <summary>
    /// Classe métier singleton représentant les types de carte de crédit supporté par le logiciel
    /// </summary>
    public class ModePaiement
    {
        private ModePaiement() { }

        public static readonly ModePaiement Inconnue = new ModePaiement();
        public static readonly ModePaiement MasterCard = new ModePaiement();
        public static readonly ModePaiement Visa = new ModePaiement();
        public static readonly ModePaiement Amex = new ModePaiement();

        /// <summary>
        /// Retourne le nom du type de la carte
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this == ModePaiement.Amex)
                return "Amex";
            if (this == ModePaiement.MasterCard)
                return "MasterCard";
            if (this == ModePaiement.Visa)
                return "Visa";
            return "Mode de Paiement Inconnue";
        }

        /// <summary>
        /// Méthode statique retournant l'instance qui représente le type de la carte
        /// </summary>
        /// <param name="s">Nom du type de carte: Visa, MasterCard ou Amex</param>
        /// <returns></returns>
        public static ModePaiement StringToValue(string s)
        {
            if (s == MasterCard.ToString())
                return MasterCard;

            if (s == Visa.ToString())
                return Visa;

            if (s == Amex.ToString())
                return Amex;

            return Inconnue;
        }
    }
}
