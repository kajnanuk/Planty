using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aplikacja.Models;
using Aplikacja.Data;

namespace Aplikacja.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AplikacjaContext _context;

        // wstrzykujemy AplikacjaContext do kontrolera
        public HomeController(ILogger<HomeController> logger, AplikacjaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            // podstawowe ogólne statystyki
            model.TopRosliny = await _context.OgrodyUzytkownikow
                .Include(o => o.Roslina)
                .Where(o => o.Roslina != null)
                .GroupBy(o => o.Roslina.NazwaPolska)
                .Select(g => new { Nazwa = g.Key, Suma = g.Sum(o => o.IloscSztuk) })
                .OrderByDescending(x => x.Suma)
                .Take(5)
                .ToDictionaryAsync(x => x.Nazwa, x => x.Suma);

            model.RozkladWarstw = await _context.Rosliny
                .Include(r => r.Warstwa)
                .Where(r => r.Warstwa != null)
                .GroupBy(r => r.Warstwa.Nazwa)
                .Select(g => new { Warstwa = g.Key, Ilosc = g.Count() })
                .ToDictionaryAsync(x => x.Warstwa, x => x.Ilosc);

            model.CalkowitaLiczbaPosadzonych = await _context.OgrodyUzytkownikow.SumAsync(o => (int?)o.IloscSztuk) ?? 0;
            model.LiczbaUzytkownikow = await _context.Uzytkownicy.CountAsync();

            // sprawdzenie czy uzytkownik jest zalogowany
            var userIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                model.CzyZalogowany = true;

                // pobranie roslin wraz z warstwami i wymaganiami
                var mojeZasadzenia = await _context.OgrodyUzytkownikow
                    .Include(o => o.Roslina).ThenInclude(r => r.Warstwa)
                    .Include(o => o.Roslina).ThenInclude(r => r.Wymagania)
                    .Where(o => o.UzytkownikId == userId)
                    .ToListAsync();

                var mojeRoslinyIds = mojeZasadzenia
                    .Where(o => o.RoslinaId.HasValue)
                    .Select(o => o.RoslinaId.Value)
                    .Distinct()
                    .ToList();

                if (mojeRoslinyIds.Any())
                {
                    // zaleznosci negatywne
                    var zaleznosciNegatywne = await _context.Zaleznosci
                        .Include(z => z.RoslinaGlowna)
                        .Include(z => z.RoslinaTowarzyszaca)
                        .Where(z => z.TypZaleznosci == "Negatywna" && 
                                    z.RoslinaGlownaId.HasValue && z.RoslinaTowarzyszacaId.HasValue &&
                                    mojeRoslinyIds.Contains(z.RoslinaGlownaId.Value) && 
                                    mojeRoslinyIds.Contains(z.RoslinaTowarzyszacaId.Value))
                        .ToListAsync();

                    foreach (var z in zaleznosciNegatywne)
                    {
                        model.OstrzezeniaAllelopatyczne.Add(
                            $"{z.RoslinaGlowna?.NazwaPolska} + {z.RoslinaTowarzyszaca?.NazwaPolska}: {z.Opis}");
                    }

                    // statystyki warstwowe
                    int totalMojeRosliny = mojeZasadzenia.Sum(o => o.IloscSztuk);
                    if (totalMojeRosliny > 0)
                    {
                        model.ProcentowyRozkladWarstwMojegoOgrodu = mojeZasadzenia
                            .Where(o => o.Roslina != null && o.Roslina.Warstwa != null)
                            .GroupBy(o => o.Roslina.Warstwa.Nazwa)
                            .ToDictionary(
                                g => g.Key, 
                                g => Math.Round((double)g.Sum(o => o.IloscSztuk) / totalMojeRosliny * 100, 1)
                            );
                    }

                    // zapotrzebowanie wodne
                    model.HarmonogramPodlewania = mojeZasadzenia
                        .Where(o => o.Roslina != null && o.Roslina.Wymagania != null && !string.IsNullOrEmpty(o.Roslina.Wymagania.ZapotrzebowanieNaWode))
                        .Select(o => o.Roslina)
                        .Distinct() // unikamy duplikatow
                        .GroupBy(r => r.Wymagania.ZapotrzebowanieNaWode)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(r => r.NazwaPolska).ToList()
                        );
                }
            }

            return View(model);
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