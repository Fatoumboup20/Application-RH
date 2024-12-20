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
    public class EvaluationPerformancesController : Controller
    {
        private readonly GestionRhContext _context;

        public EvaluationPerformancesController(GestionRhContext context)
        {
            _context = context;
        }

        // GET: EvaluationPerformances
        public async Task<IActionResult> Index()
        {
            var gestionRhContext = _context.EvaluationPerformances.Include(e => e.Utilisateur);
            return View(await gestionRhContext.ToListAsync());
        }

        // GET: EvaluationPerformances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluationPerformance = await _context.EvaluationPerformances
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluationPerformance == null)
            {
                return NotFound();
            }

            return View(evaluationPerformance);
        }

        // GET: EvaluationPerformances/Create
        public IActionResult Create()
        {
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id");
            return View();
        }

        // POST: EvaluationPerformances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluationId,ResponsableRhid,DateEvaluation,Note,Commentaire,UtilisateurId")] EvaluationPerformance evaluationPerformance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evaluationPerformance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", evaluationPerformance.UtilisateurId);
            return View(evaluationPerformance);
        }

        // GET: EvaluationPerformances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluationPerformance = await _context.EvaluationPerformances.FindAsync(id);
            if (evaluationPerformance == null)
            {
                return NotFound();
            }
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", evaluationPerformance.UtilisateurId);
            return View(evaluationPerformance);
        }

        // POST: EvaluationPerformances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationId,ResponsableRhid,DateEvaluation,Note,Commentaire,UtilisateurId")] EvaluationPerformance evaluationPerformance)
        {
            if (id != evaluationPerformance.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluationPerformance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationPerformanceExists(evaluationPerformance.EvaluationId))
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
            ViewData["UtilisateurId"] = new SelectList(_context.Utilisateurs, "Id", "Id", evaluationPerformance.UtilisateurId);
            return View(evaluationPerformance);
        }

        // GET: EvaluationPerformances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluationPerformance = await _context.EvaluationPerformances
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluationPerformance == null)
            {
                return NotFound();
            }

            return View(evaluationPerformance);
        }

        // POST: EvaluationPerformances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluationPerformance = await _context.EvaluationPerformances.FindAsync(id);
            if (evaluationPerformance != null)
            {
                _context.EvaluationPerformances.Remove(evaluationPerformance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluationPerformanceExists(int id)
        {
            return _context.EvaluationPerformances.Any(e => e.EvaluationId == id);
        }
    }
}
