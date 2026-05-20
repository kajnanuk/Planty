using System.ComponentModel.DataAnnotations;

namespace Aplikacja.Models
{
    public class OgrodUzytkownika
    {
        [Key]
        public int Id { get; set; }

        public int? UzytkownikId { get; set; }
        public Uzytkownik? Uzytkownik { get; set; }

        public int? RoslinaId { get; set; }
        public Roslina? Roslina { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data posadzenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataPosadzenia { get; set; }

        [Display(Name = "Ilość sztuk")]
        public int IloscSztuk { get; set; }

        [Display(Name = "Własne notatki")]
        [DisplayFormat(NullDisplayText = "Brak")]
        public string? WlasneNotatki { get; set; }
    }
}