using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Aplikacja.Data;

namespace Aplikacja.Controllers
{
    public class AuthController : Controller
    {
        private readonly AplikacjaContext _context;

        public AuthController(AplikacjaContext context)
        {
            _context = context;
        }

        // zwraca widok formularza logowania
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // przetwarza dane z formularza
        [HttpPost]
        public IActionResult Login(string nazwaUzytkownika, string haslo)
        {
            if (string.IsNullOrEmpty(nazwaUzytkownika) || string.IsNullOrEmpty(haslo))
            {
                ViewBag.Error = "Podaj login i hasło.";
                return View();
            }

            // obliczenie hashu z wpisanego hasla
            string hasloHash = ComputeSha256Hash(haslo);

            // szukanie uzytkownika w bazie
            var uzytkownik = _context.Uzytkownicy
                .FirstOrDefault(u => u.NazwaUzytkownika == nazwaUzytkownika && u.HasloHash == hasloHash);

            if (uzytkownik != null)
            {
                // zapisanie danych do sesji
                HttpContext.Session.SetString("UserId", uzytkownik.Id.ToString());
                HttpContext.Session.SetString("UserName", uzytkownik.NazwaUzytkownika);
                HttpContext.Session.SetString("UserRole", uzytkownik.Rola);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Nieprawidłowy login lub hasło.";
            return View();
        }

        public IActionResult Logout()
        {
            // czyszczenie sesji po wylogowaniu
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // metoda pomocnicza do tworzenia hashu
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}