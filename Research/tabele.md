Temat projektu, który wybrałeś, jest bardzo ciekawy i idealnie wpisuje się w wymagania laboratorium. Pozwala na stworzenie bogatej 
struktury relacyjnej bazy danych i daje świetne pole do zrealizowania warunku „aplikacja musi robić coś więcej”.

Poniżej przygotowałem szczegółowy projekt struktury tabel bazy danych, dopasowany do Twojego tematu oraz wymagań z instrukcji 
(w tym obsługi logowania, ról i REST API).

Będziemy korzystać z podejścia Entity Framework Core (Code-First), gdzie tabele są generowane na podstawie modeli klas w C#.

### 1. Tabele systemowe (Autoryzacja i REST API)

Zgodnie z poleceniem, tabele te **nie wliczają się** do limitu 4 tabel projektu.

#### Tabela: `Uzytkownicy` (Users)

Przechowuje dane do logowania, role (Administrator/Zwykły użytkownik) oraz tokeny do obsługi REST API.

* `Id` (PK, Integer)
* `NazwaUzytkownika` (String) – unikalny login (np. admin).
* `HasloHash` (String) – hasło zapisane w postaci skrótu (hashu), wymóg projektowy.
* `Rola` (String) – wartość np. "Admin" lub "User".
* `ApiToken` (String) – wygenerowany ciąg znaków używany do autoryzacji w REST API (wymóg na 10 pkt).

---

### 2. Tabele dziedzinowe projektu (Minimum 4 tabele)

Zaprojektowałem 5 powiązanych ze sobą tabel, aby zrealizować Twoje pomysły.

#### Tabela 1: `Warstwy` (Layers)

Słownik warstw występowania roślin w lesie/ogrodzie (np. warstwa koron drzew, podszyt, runo, pnącza).

* `Id` (PK, Integer)
* `Nazwa` (String) – np. "Warstwa krzewów", "Runo".
* `Opis` (String) – krótka charakterystyka danej warstwy.
* *(Relacja)*: Jedna warstwa posiada wiele roślin (1:N).

#### Tabela 2: `Rosliny` (Plants)

Główny katalog roślin (encyklopedia).

* `Id` (PK, Integer)
* `NazwaPolska` (String) – np. "Pomidor zwyczajny".
* `NazwaLacinska` (String) – np. "Solanum lycopersicum".
* `Opis` (String) – ogólne informacje o roślinie.
* `WarstwaId` (FK, Integer) – klucz obcy do tabeli `Warstwy`.
* *(Relacje)*:
* Należy do jednej warstwy (N:1).
* Posiada jedne wymagania (1:1).
* Posiada wiele zależności z innymi roślinami (1:N).



#### Tabela 3: `Wymagania` (Requirements)

Szczegółowe informacje o wymaganiach stanowiskowych (oddzielenie tego do osobnej tabeli ułatwia rozbudowę i zachowuje porządek).

* `Id` (PK, Integer)
* `RoslinaId` (FK, Integer) – klucz obcy do tabeli `Rosliny`.
* `Naslonecznienie` (String) – np. "Pełne słońce", "Półcień".
* `RodzajGleby` (String) – np. "Próchniczna", "Piaszczysta".
* `ZapotrzebowanieNaWode` (String) – np. "Wysokie", "Umiarkowane", "Niskie".
* `Mrozoodpornosc` (Boolean) – czy roślina zimuje w gruncie.

#### Tabela 4: `Zaleznosci` (Dependencies / Allelopathy)

Realizuje relacje pomiędzy gatunkami (tzw. allelopatia – jak rośliny wpływają na siebie).

* `Id` (PK, Integer)
* `RoslinaGlownaId` (FK, Integer) – klucz obcy do tabeli `Rosliny` (dla kogo sprawdzamy zależność).
* `RoslinaTowarzyszacaId` (FK, Integer) – klucz obcy do tabeli `Rosliny` (z kim sąsiaduje).
* `TypZaleznosci` (String/Enum) – np. "Pozytywna" (wspomaga wzrost), "Negatywna" (konkuruje o wodę/rozsiewa choroby).
* `Opis` (String) – np. "Aksamitka odstrasza nicienie niszczące korzenie pomidorów".
* *Uwaga techniczna:* W EF Core to relacja do tej samej tabeli (Self-Referencing). Będziesz musiał ustawić zachowanie `DeleteBehavior.Restrict`, aby uniknąć pętli kaskadowego usuwania.

#### Tabela 5: `OgrodUzytkownika` (GardenPlants)

Tabela łącząca konkretnego użytkownika z roślinami, które posiada w swoim wirtualnym ogrodzie.

* `Id` (PK, Integer)
* `UzytkownikId` (FK, Integer) – klucz obcy do tabeli `Uzytkownicy`.
* `RoslinaId` (FK, Integer) – klucz obcy do tabeli `Rosliny`.
* `DataPosadzenia` (DateTime) – kiedy roślina trafiła do ogrodu.
* `IloscSztuk` (Integer) – liczba posadzonych roślin.
* `WlasneNotatki` (String) – miejsce na uwagi użytkownika.

---

### 3. Jak spełnić wymóg "Robienia czegoś więcej"? (Moduł analityczny)

Samo wstawianie, edycja i usuwanie danych to zwykły CRUD. Aby dostać punkty za "coś więcej", proponuję stworzyć w aplikacji osobny widok (Dashboard / Analiza ogrodu), który na podstawie powyższych tabel wygeneruje:

1. **Audyt sąsiedztwa (Allelopatia w praktyce):** System pobiera wszystkie rośliny dodane do `OgrodUzytkownika` i sprawdza tabelę `Zaleznosci`. Jeśli użytkownik dodał do ogrodu np. Pomidora i Ziemniaka (które mają relację negatywną, bo dzielą te same choroby), aplikacja wyświetla komunikat: *"Ostrzeżenie: W swoim ogrodzie posiadasz rośliny, które negatywnie na siebie wpływają (Ziemniak + Pomidor). Zaleca się ich rozsadzenie."*
2. **Statystyki warstwowości:** Wykres kołowy lub pasek postępu pokazujący, jak zbalansowany jest ogród (np. 10% to drzewa, 40% krzewy, 50% runo). Ekologiczny ogród powinien mieć wszystkie warstwy.
3. **Kalendarz pielęgnacji / Lista zakupów:** Na podstawie "Zapotrzebowania na wodę" z tabeli `Wymagania`, system generuje wirtualny harmonogram podlewania.

---

### 4. Co musi się dziać przy pierwszym uruchomieniu (Seeding danych)?

W metodzie konfigurującej bazę danych (w Entity Framework zazwyczaj używa się `HasData` lub specjalnej klasy seedującej przy starcie w `Program.cs`) musisz dodać kod, który sprawdzi, czy bazy są puste i jeśli tak, doda:

1. **Administratora:** Użytkownika "admin" z hashem hasła (np. dla hasła "admin123") oraz rolą "Admin".
2. **Warstwy:** Rekordy dla tabeli Warstwy (Drzewa wysokie, Drzewa niskie, Krzewy, Runo, Pnącza).
3. **Przykładowe rośliny, ich wymagania i zależności:** Np. 3-4 popularne rośliny i zdefiniowanie relacji między nimi, aby po zalogowaniu system od razu "żył" i było na czym testować logikę.

---

### 5. Co wystawić przez REST API?

Wymóg na 10 punktów mówi o dodawaniu, usuwaniu, modyfikacji i wyświetlaniu przez API zabezpieczonym Tokenem.
Najlepiej przygotować kontroler `PlantsApiController` (obsługujący tabelę `Rosliny`), który będzie przyjmował w nagłówku żądania (Header) np. `Username: admin`, `Token: <unikalny_ciag>`. Jeśli dane będą poprawne (znajdą się w tabeli `Uzytkownicy`), API zwróci listę roślin w formacie JSON lub pozwoli na ich dodanie przez POST. Demonstrację API w aplikacji konsolowej można napisać przy użyciu np. wbudowanej klasy `HttpClient`.