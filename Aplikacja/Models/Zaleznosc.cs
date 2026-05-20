using System.ComponentModel.DataAnnotations;

namespace Aplikacja.Models
{
    public class Zaleznosc
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Roślina główna")]
        public int? RoslinaGlownaId { get; set; }
        public Roslina? RoslinaGlowna { get; set; }

        [Display(Name = "Roślina towarzysząca")]
        public int? RoslinaTowarzyszacaId { get; set; }
        public Roslina? RoslinaTowarzyszaca { get; set; }

        [Display(Name = "Typ zależności")]
        public string TypZaleznosci { get; set; } // Np. Pozytywna, Negatywna

        [Display(Name = "Opis wpływu")]
        public string Opis { get; set; }
    }
}