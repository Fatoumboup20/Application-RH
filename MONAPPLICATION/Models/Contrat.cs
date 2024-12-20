using System;
using System.Collections.Generic;

namespace MONAPPLICATION.Models;

public partial class Contrat
{
    public int ContratId { get; set; }

    public string TypeContrat { get; set; } = null!;

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }

    public decimal Salaire { get; set; }

    public int? UtilisateurId { get; set; }

    public virtual Utilisateur? Utilisateur { get; set; }
}
