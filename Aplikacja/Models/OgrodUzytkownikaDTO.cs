using System;

namespace Aplikacja.Models
{
    public class OgrodUzytkownikaDTO
    {
        public int Id { get; set; }
        public int? UzytkownikId { get; set; }
        public int? RoslinaId { get; set; }
        public DateTime DataPosadzenia { get; set; }
        public int IloscSztuk { get; set; }
        public string WlasneNotatki { get; set; }
    }
}