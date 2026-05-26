using Aplikacja.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aplikacja.Filters
{
    public class ApiAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // pobranie kontekstu bazy danych
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AplikacjaContext>();
            var request = context.HttpContext.Request;

            // sprawdzenie czy sa naglowki
            if (!request.Headers.TryGetValue("X-Username", out var username) ||
                !request.Headers.TryGetValue("X-Api-Token", out var token))
            {
                context.Result = new UnauthorizedObjectResult("Brak wymaganych nagłówków: X-Username lub X-Api-Token.");
                return;
            }

            //weryfikacja w bazie danych
            var user = dbContext.Uzytkownicy.FirstOrDefault(u => 
                u.NazwaUzytkownika == username.ToString() && 
                u.ApiToken == token.ToString());

            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult("Nieprawidłowa nazwa użytkownika lub token API.");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}