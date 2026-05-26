using System;
using System.IO;
using System.Net;

namespace ApiKlient
{
    class Program
    {
        static string baseUrl = "http://localhost:5116/api"; 
        static string endpoint = "WarstwaApi";
        static string username = "admin";
        static string apiToken = "pierwszy-token-api-123";

        static void Main(string[] args)
        {
            bool dziala = true;
            while (dziala)
            {
                Console.Clear();
                Console.WriteLine("--- APLIKACJA KONSOLOWA REST API ---");
                Console.WriteLine($"[1] Adres docelowy: {baseUrl}/{endpoint}");
                Console.WriteLine($"[2] Zalogowany jako: {username} (Token: {apiToken})");
                Console.WriteLine("------------------------------------");
                Console.WriteLine("[3] GET - Pobierz wszystkie rekordy");
                Console.WriteLine("[4] GET - Pobierz rekord po ID");
                Console.WriteLine("[5] POST - Dodaj nowy rekord");
                Console.WriteLine("[6] PUT - Modyfikuj rekord");
                Console.WriteLine("[7] DELETE - Usuń rekord");
                Console.WriteLine("[0] Zakończ program");
                Console.Write("\nWybierz opcję: ");

                string wybor = Console.ReadLine();

                try
                {
                    switch (wybor)
                    {
                        case "1":
                            Console.Write("Podaj nową nazwę endpointu (np. WarstwaApi, RoslinaApi): ");
                            string nowyEndpoint = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(nowyEndpoint)) endpoint = nowyEndpoint;
                            break;
                        case "2":
                            Console.Write("Podaj nazwę użytkownika: ");
                            username = Console.ReadLine();
                            Console.Write("Podaj token API: ");
                            apiToken = Console.ReadLine();
                            break;
                        case "3":
                            WykonajZadanie("GET", "");
                            break;
                        case "4":
                            Console.Write("Podaj ID rekordu: ");
                            string idGet = Console.ReadLine();
                            WykonajZadanie("GET", $"/{idGet}");
                            break;
                        case "5":
                            Console.WriteLine("Podaj dane w formacie JSON.");
                            Console.WriteLine("Przykład dla Warstwy: {\"Nazwa\":\"Nowa Warstwa\",\"Opis\":\"Przykładowy opis\"}");
                            Console.Write("JSON: ");
                            string jsonPost = Console.ReadLine();
                            WykonajZadanie("POST", "", jsonPost);
                            break;
                        case "6":
                            Console.Write("Podaj ID rekordu do edycji: ");
                            string idPut = Console.ReadLine();
                            Console.WriteLine("Podaj nowe dane w formacie JSON (Pamiętaj, żeby zawrzeć \"id\": " + idPut + " wewnątrz JSON-a!).");
                            Console.Write("JSON: ");
                            string jsonPut = Console.ReadLine();
                            WykonajZadanie("PUT", $"/{idPut}", jsonPut);
                            break;
                        case "7":
                            Console.Write("Podaj ID rekordu do usunięcia: ");
                            string idDel = Console.ReadLine();
                            WykonajZadanie("DELETE", $"/{idDel}");
                            break;
                        case "0":
                            dziala = false;
                            break;
                        default:
                            Console.WriteLine("Nieznana opcja.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[BŁĄD]: {ex.Message}");
                }

                if (dziala)
                {
                    Console.WriteLine("\nNaciśnij ENTER, aby kontynuować...");
                    Console.ReadLine();
                }
            }
        }

        // metoda do obslugi zadan http
        static void WykonajZadanie(string metoda, string parametryUrl, string cialoJson = null)
        {
            string pelnyUrl = $"{baseUrl}/{endpoint}{parametryUrl}";
            Console.WriteLine($"\n>>> Wysyłam żądanie {metoda} na adres: {pelnyUrl}");

            var request = (HttpWebRequest)WebRequest.Create(pelnyUrl);
            request.Method = metoda;
            request.Headers.Add("X-Username", username);
            request.Headers.Add("X-Api-Token", apiToken);

            // jesli POST albo PUT to dodajemy json
            if (!string.IsNullOrEmpty(cialoJson) && (metoda == "POST" || metoda == "PUT"))
            {
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(cialoJson);
                }
            }

            try
            {
                // oczekujemy na odpowiedz i czytamy ja
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"\n[STATUS ODPOWIEDZI]: {(int)response.StatusCode} {response.StatusCode}");
                    
                    // nie ma czego czytac
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            Console.WriteLine("[CIAŁO ODPOWIEDZI]:");
                            Console.WriteLine(streamReader.ReadToEnd());
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"\n[BŁĄD SIECI / API]: {ex.Message}");
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        Console.WriteLine($"[STATUS BŁĘDU]: {(int)response.StatusCode} {response.StatusCode}");
                        Console.WriteLine("[SZCZEGÓŁY]: " + streamReader.ReadToEnd());
                    }
                }
            }
        }
    }
}