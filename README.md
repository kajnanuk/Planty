# Planty - Aplikacja do zarządzania ogrodem
## Autorzy
Paweł Czyżewski - czeski@student.agh.edu.pl 


Aleksandra Dousa - adousa@student.agh.edu.pl
## Opis
Projekt realizuje zaawansowany system wsparcia projektowania oraz zarządzania ogrodami permakulturowymi i ogrodami leśnymi (tzw. food forests). Głównym celem aplikacji jest dostarczenie użytkownikom narzędzia do optymalizacji nasadzeń pod kątem dwóch fundamentalnych kryteriów ekologicznych: warstwowości roślinności (piętrowego układu lasu) oraz zależności allelopatycznych (wzajemnego biochemicznego wpływu roślin na siebie).

System nie tylko działa jako klasyczna, encyklopedyczna baza danych, ale przede wszystkim implementuje dedykowaną logikę analizującą stan wirtualnego ogrodu każdego zalogowanego użytkownika. Aplikacja ułatwia planowanie zrównoważonych upraw poprzez automatyczne generowanie alertów o konfliktach sąsiedztwa i dostarczanie spersonalizowanych harmonogramów pielęgnacyjnych.

Aplikacja korzysta z relacyjnej bazy danych SQLite, a komunikacja z nią odbywa się w całości za pośrednictwem warstwy mapowania obiektowo-relacyjnego Entity Framework Core. System operuje na 6 powiązanych tabelach.

| Nazwa tabeli | Typ relacji i powiązania | Przeznaczenie i kluczowe pola |
|-----------------------|--------------------------|-------------------------------|
| Warstwa | Relacja 1:N z tabelą Rosliny | Definiuje piętra roślinności w ekosystemie (np. Drzewa wysokie, Krzewy, Runo, Pnącza). Pola: Id, Nazwa, Opis. |
| Roslina | Klucz obcy do Warstwy, relacje 1:1 z Wymagania, 1:N z Zaleznosci oraz Ogrody | Główna encja encyklopedyczna. Przechowuje polską i łacińską nazwę oraz ogólne parametry botaniczne. |
| Wymaganie | Relacja 1:1 z tabelą Rosliny (ścisłe powiązanie) | Specyfikacja siedliskowa danego gatunku. Pola: Naslonecznienie, RodzajGleby, ZapotrzebowanieNaWode, Mrozoodpornosc. |
| Zaleznosc | Powiązanie samoreferencyjne 1:N z Rosliny | Modeluje allelopatię. Łączy dwie rośliny i określa typ wpływu (Pozytywna/Negatywna) wraz z opisem. |
| Uzytkownik | Relacja 1:N z tabelą OgrodyUzytkownikow | Przechowuje dane kont (Nazwa, Rola w systemie, bezpieczny skrót HasloHash oraz unikalny ApiToken autoryzacyjny). |
| OgrodUzytkownika | Tabela łącząca (Klucze obce do Uzytkownicy oraz Rosliny) | Zindywidualizowany rejestr nasadzeń konkretnego ogrodnika. Przechowuje ilość sztuk, datę posadzenia oraz notatki. |

System transformuje stronę główną w inteligentny, dynamiczny panel analityczny. Po zalogowaniu system pobiera kontekst zalogowanego użytkownika i generuje trzy niezależne zestawienia:
1. Audyt sąsiedztwa (Allelopatia w praktyce): Algorytm analizuje zestaw unikalnych identyfikatorów roślin posadzonych przez danego użytkownika i krzyżowo sprawdza je z tabelą Zaleznosci dla typu "Negatywna". W przypadku wykrycia konfliktu system generuje wyraźny alert ostrzegawczy z opisem zagrożenia.
2. Statystyki warstwowości: Aplikacja agreguje sumaryczną ilość sztuk wszystkich nasadzeń użytkownika i oblicza procentowy udział poszczególnych pięter ekosystemu. Wynik prezentowany jest za pomocą skumulowanego paska postępu w technologii Bootstrap, dając
wizualny pogląd na to, czy ogród jest poprawnie zbalansowany ekologicznie.
3. Wirtualny kalendarz pielęgnacji: Rośliny z ogrodu użytkownika są automatycznie grupowane według
kryterium zapotrzebowania na wodę pobieranego bezpośrednio z tabeli Wymagania, tworząc przejrzysty, kafelkowy podział zadań dla ułatwienia organizacji nawadniania.

