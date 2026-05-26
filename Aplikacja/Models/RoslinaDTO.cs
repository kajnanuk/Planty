namespace Aplikacja.Models
{
    public class RoslinaDTO
    {
        public int Id { get; set; }
        public string NazwaPolska { get; set; }
        public string NazwaLacinska { get; set; }
        public string Opis { get; set; }
        public int? WarstwaId { get; set; }
    }
}