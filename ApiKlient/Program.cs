using System.Net;
using System.Text.Json;

// UWAGA: Upewnij się, że port (np. 5000 / 5001 / 7158) zgadza się z adresem Twojej uruchomionej aplikacji MVC!
string baseUrl = "http://localhost:5116/api/WarstwaApi";
string username = "admin";
string apiToken = "pierwszy-token-api-123";

Console.WriteLine("--- TESTOWANIE PEŁNEGO CYKLU CRUD W REST API ---");

try
{
    // 1. POBRANIE DANYCH (GET)
    Console.WriteLine("\n[GET] Pobieranie warstw:");
    var getReq = (HttpWebRequest)WebRequest.Create(baseUrl);
    getReq.Method = "GET";
    getReq.Headers.Add("X-Username", username);
    getReq.Headers.Add("X-Api-Token", apiToken);

    using (var response = (HttpWebResponse)getReq.GetResponse())
    using (var streamReader = new StreamReader(response.GetResponseStream()))
    {
        Console.WriteLine(streamReader.ReadToEnd());
    }

    // 2. DODANIE DANYCH (POST)
    Console.WriteLine("\n[POST] Dodawanie nowej warstwy:");
    var postReq = (HttpWebRequest)WebRequest.Create(baseUrl);
    postReq.Method = "POST";
    postReq.ContentType = "application/json";
    postReq.Headers.Add("X-Username", username);
    postReq.Headers.Add("X-Api-Token", apiToken);

    using (var streamWriter = new StreamWriter(postReq.GetRequestStream()))
    {
        string json = JsonSerializer.Serialize(new { Nazwa = "Warstwa Testowa", Opis = "Zostanie zaraz zmieniona" });
        streamWriter.Write(json);
    }

    int newId = 0;
    using (var response = (HttpWebResponse)postReq.GetResponse())
    using (var streamReader = new StreamReader(response.GetResponseStream()))
    {
        string responseText = streamReader.ReadToEnd();
        Console.WriteLine("Odpowiedź serwera: " + responseText);
        
        // Wyciągamy ID nowo utworzonej warstwy z formatu JSON
        using (JsonDocument doc = JsonDocument.Parse(responseText))
        {
            newId = doc.RootElement.GetProperty("id").GetInt32();
        }
    }

    Console.WriteLine($"\n[INFO] Udało się utworzyć warstwę o przypisanym ID: {newId}");

    // 3. AKTUALIZACJA DANYCH (PUT)
    Console.WriteLine($"\n[PUT] Aktualizacja warstwy o ID: {newId}:");
    var putReq = (HttpWebRequest)WebRequest.Create($"{baseUrl}/{newId}");
    putReq.Method = "PUT";
    putReq.ContentType = "application/json";
    putReq.Headers.Add("X-Username", username);
    putReq.Headers.Add("X-Api-Token", apiToken);

    using (var streamWriter = new StreamWriter(putReq.GetRequestStream()))
    {
        // Pamiętaj, aby wysłać ID zgodne z tym w URLu (zabezpieczenie przed over-postingiem)
        string json = JsonSerializer.Serialize(new { Id = newId, Nazwa = "Warstwa Zaktualizowana", Opis = "Ten opis został zmieniony przez PUT" });
        streamWriter.Write(json);
    }

    using (var response = (HttpWebResponse)putReq.GetResponse())
    {
        Console.WriteLine($"Status odpowiedzi (NoContent = 204 to sukces!): {(int)response.StatusCode} {response.StatusCode}");
    }

    // 4. USUNIĘCIE DANYCH (DELETE)
    Console.WriteLine($"\n[DELETE] Usuwanie warstwy o ID: {newId}:");
    var deleteReq = (HttpWebRequest)WebRequest.Create($"{baseUrl}/{newId}");
    deleteReq.Method = "DELETE";
    deleteReq.Headers.Add("X-Username", username);
    deleteReq.Headers.Add("X-Api-Token", apiToken);

    using (var response = (HttpWebResponse)deleteReq.GetResponse())
    {
        Console.WriteLine($"Status odpowiedzi (NoContent = 204 to sukces!): {(int)response.StatusCode} {response.StatusCode}");
    }
}
catch (WebException ex)
{
    Console.WriteLine($"\nWystąpił błąd sieci: {ex.Message}");
    if (ex.Response != null)
    {
        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
        {
            Console.WriteLine("Szczegóły błędu: " + reader.ReadToEnd());
        }
    }
}

Console.WriteLine("\nNaciśnij Enter, aby zakończyć...");
Console.ReadLine();