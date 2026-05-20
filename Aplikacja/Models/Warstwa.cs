using System.ComponentModel.DataAnnotations;

namespace Aplikacja.Models
{
    public class Warstwa
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa warstwy")]
        public string Nazwa { get; set; }

        [Display(Name = "Opis")]
        public string Opis { get; set; }

        // Relacja 1:N (Jedna warstwa, wiele roślin)
        public ICollection<Roslina>? Rosliny { get; set; }
    }
}