using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MONAPPLICATION.Models;

namespace MONAPPLICATION.Controllers
{
    public class TachesController : Controller
    {
        private readonly GestionRhContext _context;

        public TachesController(GestionRhContext context)
        {
            _context = context;
        }

        // GET: Taches
        /* public async Task<IActionResult> Index()
         {
             var gestionRhContext = _context.Taches.Include(t => t.Utilisateur);
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
                // Si c'est un employé, ne montrer que ses propres tâches
                var taches = await _context.Taches
                    .Include(t => t.Utilisateur)
                    .Where(t => t.UtilisateurId == utilisateurConnecte.Id)
                    .ToListAsync();

                return View(taches);
            }
            else
            {
                // Si ce n'est pas un employé, montrer toutes les tâches
                var taches = await _context.Taches
                    .Include(t => t.Utilisateur)
                    .ToListAsync();

                return View(taches);
            }
        }


        // GET: Taches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Taches
                .Include(t => t.Utilisateur)
                .FirstOrDefaultAsync(m => m.TacheId == id);
            if (tache == null)
            {
                return NotFound();
            }

            return View(tache);
        }

        // GET: Taches/Create
        public IActionResult Create()
        {
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id");
            return View();
        }

        // POST: Taches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TacheId,Titre,Description,DateDebut,DateFin,Statut,UtilisateurId")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                tache.Statut = "En attente"; // Initialisation du statut
                _context.Add(tache); await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); }
                ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", tache.UtilisateurId);
            return View(tache);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Terminer(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache != null)
            {
                tache.Statut = "Terminé"; // Changez le statut de la tâche
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index)); // Redirige vers la liste des tâches
        }
        // GET: Taches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Taches.FindAsync(id);
            if (tache == null)
            {
                return NotFound();
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", tache.UtilisateurId);
            return View(tache);
        }

        // POST: Taches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TacheId,Titre,Description,DateDebut,DateFin,Statut,UtilisateurId")] Tache tache)
        {
            if (id != tache.TacheId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tache);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TacheExists(tache.TacheId))
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
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", tache.UtilisateurId);
            return View(tache);
        }

        // GET: Taches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tache = await _context.Taches
                .Include(t => t.Utilisateur)
                .FirstOrDefaultAsync(m => m.TacheId == id);
            if (tache == null)
            {
                return NotFound();
            }

            return View(tache);
        }

        // POST: Taches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tache = await _context.Taches.FindAsync(id);
            if (tache != null)
            {
                _context.Taches.Remove(tache);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TacheExists(int id)
        {
            return _context.Taches.Any(e => e.TacheId == id);
        }
    }
}
