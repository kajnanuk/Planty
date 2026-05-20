using System.ComponentModel.DataAnnotations;

namespace Aplikacja.Models
{
    public class Uzytkownik
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa użytkownika")]
        public string NazwaUzytkownika { get; set; }

        [Display(Name = "Hasło (Hash)")]
        public string HasloHash { get; set; }

        [Display(Name = "Rola w systemie")]
        public string Rola { get; set; }

        public string? ApiToken { get; set; }

        // Relacja: Użytkownik ma wiele roślin w swoim ogrodzie
        public ICollection<OgrodUzytkownika>? OgrodUzytkownika { get; set; }
    }
}