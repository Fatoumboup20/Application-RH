using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MONAPPLICATION.Models;

public partial class GestionRhContext : DbContext
{
    public GestionRhContext()
    {
    }

    public GestionRhContext(DbContextOptions<GestionRhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contrat> Contrats { get; set; }

    public virtual DbSet<Demande> Demandes { get; set; }

    public virtual DbSet<EvaluationPerformance> EvaluationPerformances { get; set; }

    public virtual DbSet<Tache> Taches { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9T57506\\MSSQLSERVER01;Database=GestionRH;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contrat>(entity =>
        {
            entity.HasKey(e => e.ContratId).HasName("PK__Contrat__65C1D90C7806337A");

            entity.ToTable("Contrat");

            entity.Property(e => e.Salaire).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TypeContrat).HasMaxLength(255);

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Contrats)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("FK_Contrat_Utilisateur");
        });

        modelBuilder.Entity<Demande>(entity =>
        {
            entity.HasKey(e => e.DemandeId).HasName("PK__Demande__20C6D0D6C6B89651");

            entity.ToTable("Demande");

            entity.Property(e => e.Statut).HasMaxLength(250);
            entity.Property(e => e.TypeDemande).HasMaxLength(250);

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Demandes)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("FK_Demande_Utilisateur");
        });

        modelBuilder.Entity<EvaluationPerformance>(entity =>
        {
            entity.HasKey(e => e.EvaluationId).HasName("PK__Evaluati__36AE68F3ED18963D");

            entity.ToTable("EvaluationPerformance");

            entity.Property(e => e.Commentaire).HasMaxLength(500);
            entity.Property(e => e.Note).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.ResponsableRhid).HasColumnName("ResponsableRHId");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.EvaluationPerformances)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("FK_EvaluationPerformance_Utilisateur");
        });

        modelBuilder.Entity<Tache>(entity =>
        {
            entity.HasKey(e => e.TacheId).HasName("PK__Tache__93384F456EF03F6C");

            entity.ToTable("Tache");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Statut).HasMaxLength(255);
            entity.Property(e => e.Titre).HasMaxLength(255);

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Taches)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("FK_Tache_Utilisateur");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Utilisat__3214EC077426D80C");

            entity.ToTable("Utilisateur");

            entity.Property(e => e.Adresse)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Prenom)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Salaire).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
