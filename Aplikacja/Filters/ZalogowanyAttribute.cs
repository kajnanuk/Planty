using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aplikacja.Filters
{
    public class ZalogowanyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Odczyt z obiektu sesji
            var userId = context.HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                // Jeśli brak w sesji, przekieruj na stronę logowania
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            
            base.OnActionExecuting(context);
        }
    }
}