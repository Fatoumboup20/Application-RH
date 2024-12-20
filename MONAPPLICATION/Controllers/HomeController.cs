using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MONAPPLICATION.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MONAPPLICATION.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GestionRhContext _context;

        public HomeController(ILogger<HomeController> logger, GestionRhContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string userName = null;
            string userSurname = null;
            string role = null;

            // Vérifier si l'utilisateur est authentifié
            if (User.Identity.IsAuthenticated)
            {
                // Récupérer le prénom et le nom de l'utilisateur connecté
                userName = User.FindFirst(ClaimTypes.GivenName)?.Value; // Prénom
                userSurname = User.FindFirst(ClaimTypes.Surname)?.Value; // Nom de famille
                role = User.FindFirst(ClaimTypes.Role)?.Value; // Rôle

                // Log pour débogage
                Console.WriteLine($"Prénom: {userName}, Nom: {userSurname}, Rôle: {role}");

                // Passer les informations à la vue
                ViewBag.Nom = userSurname;    // Nom
                ViewBag.Prenom = userName;    // Prénom
                ViewBag.Role = role;          // Rôle

                // Si l'utilisateur est un administrateur, récupérer les statistiques
                if (role == "Administrateur")
                {
                    ViewBag.TotalUtilisateurs = _context.Utilisateurs.Count();
                    ViewBag.TotalContrats = _context.Contrats.Count();
                    ViewBag.TotalTaches = _context.Taches.Count();
                    ViewBag.TotalSalaires = _context.Utilisateurs.Sum(e => e.Salaire);
                }
            }
            else
            {
                ViewBag.Nom = "Invité";
                ViewBag.Prenom = string.Empty;
                ViewBag.Role = "Non authentifié";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
