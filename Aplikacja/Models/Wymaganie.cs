using System.ComponentModel.DataAnnotations;

namespace Aplikacja.Models
{
    public class Wymaganie
    {
        [Key]
        public int Id { get; set; }

        // klucz obcy do rosliny
        public int? RoslinaId { get; set; }
        public Roslina? Roslina { get; set; }

        [Display(Name = "Nasłonecznienie")]
        public string Naslonecznienie { get; set; }

        [Display(Name = "Rodzaj gleby")]
        public string RodzajGleby { get; set; }

        [Display(Name = "Zapotrzebowanie na wodę")]
        public string ZapotrzebowanieNaWode { get; set; }

        [Display(Name = "Mrozoodporność")]
        public bool Mrozoodpornosc { get; set; }
    }
}