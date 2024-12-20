using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MONAPPLICATION.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajoute les services n?cessaires pour les contr?leurs et les vues
builder.Services.AddControllersWithViews();

// Ajoute la configuration du contexte de base de donn?es
string connectionString = builder.Configuration.GetConnectionString("GestionRH");
builder.Services.AddDbContext<GestionRhContext>(options =>
    options.UseSqlServer(connectionString)
           .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
           .EnableSensitiveDataLogging());

// Configure l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Chemin vers la page de connexion
        options.LogoutPath = "/Account/Logout"; // Chemin de d?connexion
    });

var app = builder.Build();

// Configure le pipeline de requ?tes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ajoute l'authentification et l'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Redirige vers la page de connexion par d?faut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
