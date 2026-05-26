using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aplikacja.Filters
{
    public class ZalogowanyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // odczyt z obiektu sesji
            var userId = context.HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                // jesli brak w sesji to przekieruj na strone logowania
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            
            base.OnActionExecuting(context);
        }
    }
}