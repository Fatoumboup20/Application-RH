using System;
using System.Collections.Generic;

namespace MONAPPLICATION.Models;

public partial class Utilisateur
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public DateOnly? DateNaissance { get; set; }

    public string Adresse { get; set; } = null!;

    public DateOnly? DateEmbauche { get; set; }

    public decimal Salaire { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Contrat> Contrats { get; set; } = new List<Contrat>();

    public virtual ICollection<Demande> Demandes { get; set; } = new List<Demande>();

    public virtual ICollection<EvaluationPerformance> EvaluationPerformances { get; set; } = new List<EvaluationPerformance>();

    public virtual ICollection<Tache> Taches { get; set; } = new List<Tache>();
}
