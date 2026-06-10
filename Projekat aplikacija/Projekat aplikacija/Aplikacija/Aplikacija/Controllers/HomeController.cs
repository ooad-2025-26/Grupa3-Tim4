using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;


namespace Aplikacija.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<Korisnik> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
    ApplicationDbContext context,
    UserManager<Korisnik> userManager,
    RoleManager<IdentityRole> roleManager,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Administrator"))
            {
                var users = await _userManager.Users.ToListAsync();
                var pendingUsers = new List<Korisnik>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (!roles.Any())
                    {
                        pendingUsers.Add(user);
                    }
                }

                ViewBag.PendingUsers = pendingUsers;
            }

            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var loyalty = await _context.LoyaltyProgram
                    .FirstOrDefaultAsync(l => l.KorisnikId == userId);

                ViewBag.LoyaltyPoints = loyalty != null ? loyalty.UkupniBodovi : 0;
            }

            

            var takmicenja = await _context.Takmicenje
    .OrderBy(t => t.Datum)
    .Take(5)
    .ToListAsync();

            ViewBag.Takmicenja = takmicenja;

            return View();


        }


        [Authorize(Roles = "Administrator")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UcitajTakmicenja()
        {
            string apiUrl = _configuration["PandaScore:ApiUrl"];
            string token = _configuration["PandaScore:Token"];

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var json = await client.GetStringAsync(apiUrl);

            var podaci = JsonSerializer.Deserialize<List<PandaScoreMatch>>(json);

            foreach (var item in podaci)
            {
                DateTime datum = DateTime.Now;

                if (item.begin_at != null)
                    DateTime.TryParse(item.begin_at, out datum);

                bool postoji = await _context.Takmicenje.AnyAsync(t =>
                    t.Naziv == item.name &&
                    t.Datum.Date == datum.Date
                );

                if (!postoji)
                {
                    var takmicenje = new Takmicenje
                    {
                        Naziv = item.name ?? "Esports Match",
                        Igra = item.videogame?.name ?? "Game",
                        Datum = datum
                    };

                    _context.Takmicenje.Add(takmicenje);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Takmičenja su učitana iz PandaScore vanjskog sistema.";

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ApproveUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> RejectUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
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
