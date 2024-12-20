using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MONAPPLICATION.Models;
using System.Security.Claims;
using MONAPPLICATION.Models;
using Microsoft.EntityFrameworkCore;


namespace MONAPPLICATION.Controllers
{
    public class AccountController : Controller
    {
        private readonly GestionRhContext _context;

        // Constructeur pour injecter le contexte de la base de données
        public AccountController(GestionRhContext context)
        {
            _context = context;
        }

        // Action GET pour afficher la page de connexion
        public IActionResult Login()
        {
            return View();
        }

        // Action POST pour traiter la soumission du formulaire de connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Rechercher l'utilisateur dans la base de données
                var user = await _context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower() && u.Password == model.Password);

                if (user != null)
                {
                    // Vérifier si le compte est désactivé
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError("", "Votre compte est désactivé. Veuillez contacter l'administrateur.");
                        return View(model);
                    }

                    // Créer les revendications, y compris le prénom, le nom et le rôle de l'utilisateur
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.Prenom ?? "Prénom par défaut"), // Utiliser Prenom
                new Claim(ClaimTypes.Surname, user.Nom ?? "Nom par défaut"), // Utiliser Nom
                new Claim(ClaimTypes.Role, user.Role) // Ajoute le rôle
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect.");
            }

            return View(model);
        }

        // Action pour la déconnexion
        public IActionResult Logout()
        {
            // Supprimer les informations de l'utilisateur de la session
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Rediriger l'utilisateur vers la page de connexion
            return RedirectToAction("Login", "Account");
        }
    }

}
