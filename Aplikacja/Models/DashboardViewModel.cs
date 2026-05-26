namespace Aplikacja.Models
{
    public class DashboardViewModel
    {
        // statystyki zbiorcze (dla wszystkich)
        public Dictionary<string, int> TopRosliny { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> RozkladWarstw { get; set; } = new Dictionary<string, int>();
        public int CalkowitaLiczbaPosadzonych { get; set; }
        public int LiczbaUzytkownikow { get; set; }

        //statystyki spersonalizowanie
        public bool CzyZalogowany { get; set; }
        
        // komunikaty z ostrzezeniami
        public List<string> OstrzezeniaAllelopatyczne { get; set; } = new List<string>();
        
        // statystyi warstwowosci
        public Dictionary<string, double> ProcentowyRozkladWarstwMojegoOgrodu { get; set; } = new Dictionary<string, double>();
        
        // zapotrzebowanie na wode
        public Dictionary<string, List<string>> HarmonogramPodlewania { get; set; } = new Dictionary<string, List<string>>();
    }
}