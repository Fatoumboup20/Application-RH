using System;
using System.Collections.Generic;

namespace MONAPPLICATION.Models
{
    public partial class Tache
    {
        public int TacheId { get; set; }

        public string Titre { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateOnly DateDebut { get; set; }

        public DateOnly DateFin { get; set; }

        public string Statut { get; set; } = "En attente"; // Ajout du champ Statut avec valeur par défaut

        public int? UtilisateurId { get; set; }

        public virtual Utilisateur? Utilisateur { get; set; }
    }
}
