namespace Aplikacja.Models
{
    public class WymaganieDTO
    {
        public int Id { get; set; }
        public int? RoslinaId { get; set; }
        public string Naslonecznienie { get; set; }
        public string RodzajGleby { get; set; }
        public string ZapotrzebowanieNaWode { get; set; }
        public bool Mrozoodpornosc { get; set; }
    }
}