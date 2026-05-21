namespace Aplikacja.Models
{
    public class DashboardViewModel
    {
        // --- Statystyki ogólne (dla wszystkich) ---
        public Dictionary<string, int> TopRosliny { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> RozkladWarstw { get; set; } = new Dictionary<string, int>();
        public int CalkowitaLiczbaPosadzonych { get; set; }
        public int LiczbaUzytkownikow { get; set; }

        // --- Statystyki spersonalizowane (tylko dla zalogowanych) ---
        public bool CzyZalogowany { get; set; }
        
        // Audyt sąsiedztwa: Lista komunikatów z ostrzeżeniami
        public List<string> OstrzezeniaAllelopatyczne { get; set; } = new List<string>();
        
        // Statystyki warstwowości MOJEGO ogrodu: Nazwa warstwy -> Procentowy udział
        public Dictionary<string, double> ProcentowyRozkladWarstwMojegoOgrodu { get; set; } = new Dictionary<string, double>();
        
        // Kalendarz pielęgnacji: Zapotrzebowanie na wodę -> Lista nazw roślin
        public Dictionary<string, List<string>> HarmonogramPodlewania { get; set; } = new Dictionary<string, List<string>>();
    }
}