using System;
using System.Collections.Generic;

namespace MONAPPLICATION.Models;

public partial class EvaluationPerformance
{
    public int EvaluationId { get; set; }

    public int? ResponsableRhid { get; set; }

    public DateOnly DateEvaluation { get; set; }

    public decimal Note { get; set; }

    public string Commentaire { get; set; } = null!;

    public int? UtilisateurId { get; set; }

    public virtual Utilisateur? Utilisateur { get; set; }
}
