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
    public class ContratsController : Controller
    {
        private readonly GestionRhContext _context;

        public ContratsController(GestionRhContext context)
        {
            _context = context;
        }

        // GET: Contrats
        /*public async Task<IActionResult> Index()
        {
            var gestionRhContext = _context.Contrats.Include(c => c.Utilisateur);
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
                return NotFound(); // Si l'utilisateur connecté n'est pas trouvé, afficher une erreur
            }

            // Vérifier le rôle de l'utilisateur connecté
            if (utilisateurConnecte.Role == "Employe")
            {
                // Si c'est un employé, ne montrer que son contrat
                var contrats = await _context.Contrats
                    .Include(c => c.Utilisateur)
                    .Where(c => c.UtilisateurId == utilisateurConnecte.Id)
                    .ToListAsync();

                return View(contrats);
            }
            else
            {
                // Si ce n'est pas un employé, afficher tous les contrats
                var contrats = await _context.Contrats
                    .Include(c => c.Utilisateur)
                    .ToListAsync();

                return View(contrats);
            }
        }


        // GET: Contrats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrat = await _context.Contrats
                .Include(c => c.Utilisateur)
                .FirstOrDefaultAsync(m => m.ContratId == id);
            if (contrat == null)
            {
                return NotFound();
            }

            return View(contrat);
        }

        // GET: Contrats/Create
        public IActionResult Create()
        {
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id");
            return View();
        }

        // POST: Contrats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContratId,TypeContrat,DateDebut,DateFin,Salaire,UtilisateurId")] Contrat contrat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contrat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", contrat.UtilisateurId);
            return View(contrat);
        }

        // GET: Contrats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrat = await _context.Contrats.FindAsync(id);
            if (contrat == null)
            {
                return NotFound();
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", contrat.UtilisateurId);
            return View(contrat);
        }

        // POST: Contrats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContratId,TypeContrat,DateDebut,DateFin,Salaire,UtilisateurId")] Contrat contrat)
        {
            if (id != contrat.ContratId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contrat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratExists(contrat.ContratId))
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
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", contrat.UtilisateurId);
            return View(contrat);
        }

        // GET: Contrats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrat = await _context.Contrats
                .Include(c => c.Utilisateur)
                .FirstOrDefaultAsync(m => m.ContratId == id);
            if (contrat == null)
            {
                return NotFound();
            }

            return View(contrat);
        }

        // POST: Contrats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contrat = await _context.Contrats.FindAsync(id);
            if (contrat != null)
            {
                _context.Contrats.Remove(contrat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Print(int id) { 
            var contrat = await _context.Contrats.Include(c => c.Utilisateur).FirstOrDefaultAsync(c => c.ContratId == id); if (contrat == null) { return NotFound(); }
            return View(contrat); }
        private bool ContratExists(int id)
        {
            return _context.Contrats.Any(e => e.ContratId == id);
        }
    }
}
