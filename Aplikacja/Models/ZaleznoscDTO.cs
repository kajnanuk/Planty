namespace Aplikacja.Models
{
    public class ZaleznoscDTO
    {
        public int Id { get; set; }
        public int? RoslinaGlownaId { get; set; }
        public int? RoslinaTowarzyszacaId { get; set; }
        public string TypZaleznosci { get; set; }
        public string Opis { get; set; }
    }
}