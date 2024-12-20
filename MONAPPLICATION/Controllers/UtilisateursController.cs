using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MONAPPLICATION.Models;

namespace MONAPPLICATION.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly GestionRhContext _context;

        public UtilisateursController(GestionRhContext context)
        {
            _context = context;
        }

        // GET: Utilisateurs
        public async Task<IActionResult> Index()
        {
            // Récupérer le rôle de l'utilisateur connecté
            var roleConnecte = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            // Obtenir l'email ou le nom d'utilisateur de l'administrateur connecté
            var utilisateurConnecte = User.Identity.Name;

            // Récupérer tous les utilisateurs
            var utilisateurs = _context.Utilisateurs.AsQueryable();

            // Si l'utilisateur est ResponsableRH, exclure les administrateurs
            if (roleConnecte == "ResponsableRH")
            {
                utilisateurs = utilisateurs.Where(u => u.Role != "Administrateur");
            }

            // Exclure l'utilisateur connecté lui-même
            utilisateurs = utilisateurs.Where(u => u.Email != utilisateurConnecte);

            // Charger la liste des utilisateurs après application des filtres
            var listeUtilisateurs = await utilisateurs.ToListAsync();

            return View(listeUtilisateurs);
        }



        // GET: Utilisateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Role,Nom,Prenom,DateNaissance,Adresse,DateEmbauche,Salaire,IsActive")] Utilisateur utilisateur)
        {
            // Récupérer le rôle de l'utilisateur connecté
            var roleConnecte = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            // Si le rôle est ResponsableRH, forcer le rôle "Employe"
            if (roleConnecte == "ResponsableRH" && utilisateur.Role != "Employe")
            {
                ModelState.AddModelError("Role", "Vous n'êtes pas autorisé à attribuer ce rôle.");
                return View(utilisateur);
            }

            if (ModelState.IsValid)
            {
                _context.Add(utilisateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(utilisateur);
        }


        // GET: Utilisateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Role,Nom,Prenom,DateNaissance,Adresse,DateEmbauche,Salaire,IsActive")] Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilisateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilisateurExists(utilisateur.Id))
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
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur != null)
            {
                _context.Utilisateurs.Remove(utilisateur);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            utilisateur.IsActive = true; // Activer l'utilisateur
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: Utilisateurs/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            utilisateur.IsActive = false; // Désactiver l'utilisateur
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.Id == id);
        }
    }
}
