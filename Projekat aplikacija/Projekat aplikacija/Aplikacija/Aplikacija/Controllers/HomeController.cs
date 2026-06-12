using Aplikacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;


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

                    if (!user.ClanarinaAktivna && roles.Contains("User"))
                    {
                        pendingUsers.Add(user);
                    }
                }

                ViewBag.PendingUsers = pendingUsers;
            }

            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.FindByIdAsync(userId);

                ViewBag.ClanarinaAktivna = currentUser.ClanarinaAktivna;

              
                var sada = DateTime.Now;


                if (currentUser.ClanarinaAktivna)
                {
                    var loyalty = await _context.LoyaltyProgram
                        .FirstOrDefaultAsync(l => l.KorisnikId == userId);

                    ViewBag.LoyaltyPoints = loyalty?.UkupniBodovi ?? 0;
                }

                ViewBag.ImaAktivnuSesiju = await _context.Sesija.AnyAsync(s =>
       s.KorisnikId == userId &&
       s.Status == StatusSesije.Aktivna &&
       s.VrijemePocetka <= sada &&
       s.VrijemeZavrsetka >= sada);
            }


            if (User.Identity != null && User.Identity.IsAuthenticated &&
    (User.IsInRole("Employee") || User.IsInRole("Administrator")))
            {
                var sada = DateTime.Now;

                ViewBag.AktivneNarudzbe = await _context.Narudzba
                    .Include(n => n.Korisnik)
                    .Include(n => n.Sesija)
                        .ThenInclude(s => s.Uredjaj)
                    .Where(n => n.Sesija.Status == StatusSesije.Aktivna
                             && n.Sesija.VrijemePocetka <= sada
                             && n.Sesija.VrijemeZavrsetka >= sada)
                    .OrderByDescending(n => n.VrijemeSlanja)
                    .Take(10)
                    .ToListAsync();
            }




            var proslaTakmicenja = await _context.Takmicenje
    .Where(t => t.Datum < DateTime.Today)
    .ToListAsync();

            if (proslaTakmicenja.Any())
            {
                _context.Takmicenje.RemoveRange(proslaTakmicenja);
                await _context.SaveChangesAsync();
            }

            var takmicenja = await _context.Takmicenje
                .Where(t => t.Datum >= DateTime.Today)
                .OrderBy(t => t.Datum).Take(10)
                .ToListAsync();

            ViewBag.Takmicenja = takmicenja;

            return View();


        }


        [Authorize(Roles = "Administrator")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UcitajTakmicenja()
        {

            var proslaTakmicenja = await _context.Takmicenje
        .Where(t => t.Datum < DateTime.Today)
        .ToListAsync();

            if (proslaTakmicenja.Any())
            {
                _context.Takmicenje.RemoveRange(proslaTakmicenja);
                await _context.SaveChangesAsync();
            }

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
        public async Task<IActionResult> ApproveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            user.ClanarinaAktivna = true;
            await _userManager.UpdateAsync(user);

            var loyalty = await _context.LoyaltyProgram
                .FirstOrDefaultAsync(x => x.KorisnikId == userId);

            if (loyalty == null)
            {
                loyalty = new LoyaltyProgram
                {
                    KorisnikId = userId,
                    UkupniBodovi = 0,
                    PopustPoBodu = 0.01
                };

                _context.LoyaltyProgram.Add(loyalty);
                await _context.SaveChangesAsync();
            }

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
