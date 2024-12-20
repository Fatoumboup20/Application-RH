using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MONAPPLICATION.Models;
using System;

namespace MONAPPLICATION.Controllers
{
    public class DemandesController : Controller
    {
        private readonly GestionRhContext _context;

        public DemandesController(GestionRhContext context)
        {
            _context = context;
        }

        // GET: Demandes
        /*  public async Task<IActionResult> Index()
          {
              var gestionRhContext = _context.Demandes.Include(d => d.Utilisateur);
              return View(await gestionRhContext.ToListAsync());
          }*/
        public async Task<IActionResult> Index()
        {
            // Récupérer l'email de l'utilisateur connecté
            var emailConnecte = User.Identity.Name;

            // Récupérer l'utilisateur connecté dans la base de données
            var utilisateurConnecte = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == emailConnecte);

            if (utilisateurConnecte == null)
            {
                return NotFound(); // Si l'utilisateur connecté n'est pas trouvé
            }

            // Vérifier le rôle de l'utilisateur connecté
            if (utilisateurConnecte.Role == "Employe")
            {
                // Si c'est un employé, ne montrer que ses propres demandes
                var demandes = await _context.Demandes
                    .Include(d => d.Utilisateur)
                    .Where(d => d.UtilisateurId == utilisateurConnecte.Id)
                    .ToListAsync();

                return View(demandes);
            }
            else
            {
                // Si ce n'est pas un employé, montrer toutes les demandes
                var demandes = await _context.Demandes
                    .Include(d => d.Utilisateur)
                    .ToListAsync();

                return View(demandes);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatut(int id, string statut)
        {
            // Vérifier le rôle de l'utilisateur connecté
            if (!User.IsInRole("ResponsableRH"))
            {
                return Forbid(); // Retourne une erreur si l'utilisateur n'est pas autorisé
            }

            var demande = await _context.Demandes.FindAsync(id);

            if (demande == null)
            {
                return NotFound();
            }

            demande.Statut = statut;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                return BadRequest("Erreur lors de la mise à jour.");
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Demandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demande = await _context.Demandes
                .Include(d => d.Utilisateur)
                .FirstOrDefaultAsync(m => m.DemandeId == id);
            if (demande == null)
            {
                return NotFound();
            }

            return View(demande);
        }

        // GET: Demandes/Create
        public IActionResult Create()
        {
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id");
            return View();
        }

        // POST: Demandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeDemande,DateDemande")] Demande demande)
        {
            // Vérifier si l'utilisateur est authentifié
            var utilisateurIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(utilisateurIdClaim))
            {
                return Unauthorized("Vous devez être connecté pour créer une demande.");
            }

            // Associer la demande à l'utilisateur connecté
            demande.UtilisateurId = int.Parse(utilisateurIdClaim);
            demande.Statut = "En attente";
           // demande.DateDemande = DateTime.Now;
            demande.DateDemande = DateOnly.FromDateTime(DateTime.Now);



            try
            {
                _context.Add(demande);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Rediriger après la création
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de la demande : {ex.Message}");
                ModelState.AddModelError("", "Une erreur est survenue lors de la création de la demande.");
            }

            return View(demande); // Revenir à la vue en cas d'erreur
        }


        // GET: Demandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demande = await _context.Demandes.FindAsync(id);
            if (demande == null)
            {
                return NotFound();
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", demande.UtilisateurId);
            return View(demande);
        }

        // POST: Demandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DemandeId,TypeDemande,DateDemande,Statut,UtilisateurId")] Demande demande)
        {
            if (id != demande.DemandeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandeExists(demande.DemandeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", demande.UtilisateurId);
            return View(demande);
        }

        // GET: Demandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demande = await _context.Demandes
                .Include(d => d.Utilisateur)
                .FirstOrDefaultAsync(m => m.DemandeId == id);
            if (demande == null)
            {
                return NotFound();
            }

            return View(demande);
        }

        // POST: Demandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var demande = await _context.Demandes.FindAsync(id);
            if (demande != null)
            {
                _context.Demandes.Remove(demande);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemandeExists(int id)
        {
            return _context.Demandes.Any(e => e.DemandeId == id);
        }
    }
}
