using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Aplikacja.Models;

namespace Aplikacja.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AplikacjaContext context)
        {
            // Upewnia się, że baza danych została utworzona
            context.Database.EnsureCreated();

            // Szukamy jakichkolwiek użytkowników. Jeśli jacyś są, to znaczy, 
            // że baza została już zainicjowana i przerywamy działanie metody.
            if (context.Uzytkownicy.Any())
            {
                return;   
            }

            // 1. Dodawanie pierwszego administratora
            var admin = new Uzytkownik 
            { 
                NazwaUzytkownika = "admin", 
                HasloHash = ComputeSha256Hash("admin123"), // Hasło to "admin123"
                Rola = "Admin",
                ApiToken = "pierwszy-token-api-123" // Wymóg do obsługi REST API
            };
            
            context.Uzytkownicy.Add(admin);
            context.SaveChanges();

            // 2. Dodawanie podstawowych warstw
            var warstwy = new Warstwa[]
            {
                new Warstwa { Nazwa = "Drzewa wysokie", Opis = "Najwyższa warstwa w ogrodzie/lesie." },
                new Warstwa { Nazwa = "Drzewa niskie", Opis = "Mniejsze drzewa, np. owocowe." },
                new Warstwa { Nazwa = "Krzewy", Opis = "Warstwa pośrednia, np. maliny, porzeczki." },
                new Warstwa { Nazwa = "Runo", Opis = "Rośliny niskie, zioła, warzywa, truskawki." },
                new Warstwa { Nazwa = "Pnącza", Opis = "Rośliny pnące się po innych lub po podporach." }
            };
            
            context.Warstwy.AddRange(warstwy);
            context.SaveChanges();

            // 3. Dodawanie przykładowej rośliny do testów
            var pomidor = new Roslina
            {
                NazwaPolska = "Pomidor zwyczajny",
                NazwaLacinska = "Solanum lycopersicum",
                Opis = "Popularne warzywo ciepłolubne.",
                WarstwaId = warstwy.Single(w => w.Nazwa == "Runo").Id
            };

            context.Rosliny.Add(pomidor);
            context.SaveChanges();
        }

        // Metoda pomocnicza do tworzenia hashu (skrótu) z hasła
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