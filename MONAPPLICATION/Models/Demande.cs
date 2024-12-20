using System;
using System.Collections.Generic;

namespace MONAPPLICATION.Models
{
    public partial class Demande
    {
        public int DemandeId { get; set; }
        public string TypeDemande { get; set; } = null!;
        public DateOnly DateDemande { get; set; }

        
        public string Statut { get; set; }

        public int? UtilisateurId { get; set; }
        public virtual Utilisateur? Utilisateur { get; set; }

       // Constructeur
        public Demande()
        {
            Statut = "En attente"; 
        }
    }
}