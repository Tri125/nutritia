using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutritia.Logic.Model.Entities
{

    /// <summary>
    /// Classe représentant un Don
    /// </summary>
    public class Don
    {
        //Global unique identifier comme no de transaction.
        //Chance presque null d'avoir une collision.
        private Guid noTransaction;

        public Guid NoTransaction { get { return noTransaction; } }

        //DateHeure où la transaction a eu lieu.
        public DateTime DateHeureTransaction { get; private set; }
        //Nom du propriétaire de la carte de crédit qui a fait le Don.
        public string NomAuteur { get; private set; }
        public double Montant { get; private set; }

        //Type de carte de crédit utilisé pour le Don.
        public ModePaiement ModePaiementTransaction { get; private set; }

        public Don()
        {
            noTransaction = Guid.NewGuid();
        }

        /// <summary>
        /// Constructeur Don sans DateTime.
        /// Lorsqu'on envoie un Don, le temps est mit sur le côté serveur.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="montant"></param>
        /// <param name="modePaiement">Type de carte de crédit</param>
        public Don(string nom, double montant, ModePaiement modePaiement)
            : this()
        {
            NomAuteur = nom;
            Montant = montant;
            ModePaiementTransaction = modePaiement;
        }

        /// <summary>
        /// Constructeur Don avec DateTime
        /// Lorsqu'on reçois un Don de la base de données, nous pouvons setter le DateTime avec le temps du Don
        /// tel qu'enregistré dans la BD.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="montant"></param>
        /// <param name="modePaiement">Type de carte de crédit</param>
        /// <param name="dateHeure"></param>
        public Don(string nom, double montant, ModePaiement modePaiement, DateTime dateHeure)
            : this(nom, montant, modePaiement)
        {
            DateHeureTransaction = dateHeure;
        }

        /// <summary>
        /// Données de Don formatté dans le format utilisé pour encoder le codeQC.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Don Nutritia Inc.");
            sb.AppendLine();
            sb.Append("No. Transaction: ");
            sb.Append(noTransaction);
            sb.AppendLine();
            sb.Append("Propriétaire de la carte : ");
            sb.Append(NomAuteur);
            sb.AppendLine();
            sb.Append("Montant: ");
            sb.Append(Montant);
            sb.Append("$");
            sb.AppendLine();
            sb.Append("Mode de paiement: ");
            sb.Append(ModePaiementTransaction);
            sb.AppendLine();
            sb.Append("Date de transaction: ");
            sb.Append(DateHeureTransaction);
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
