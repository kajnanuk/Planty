using System.ComponentModel.DataAnnotations;

namespace Aplikacja.Models
{
    public class Roslina
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Polska nazwa")]
        public string NazwaPolska { get; set; }

        [Display(Name = "Łacińska nazwa")]
        public string NazwaLacinska { get; set; }

        [Display(Name = "Opis ogólny")]
        public string Opis { get; set; }

        // Klucz obcy i właściwość nawigacyjna do tabeli Warstwy
        public int? WarstwaId { get; set; }
        public Warstwa? Warstwa { get; set; }

        // Relacje (jedna roślina ma jedne wymagania, wiele zależności i może być w wielu ogrodach)
        public Wymaganie? Wymagania { get; set; }
        public ICollection<Zaleznosc>? ZaleznosciJakoGlowna { get; set; }
        public ICollection<Zaleznosc>? ZaleznosciJakoTowarzyszaca { get; set; }
        public ICollection<OgrodUzytkownika>? OgrodUzytkownika { get; set; }
    }
}