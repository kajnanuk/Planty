using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aplikacja.Filters
{
    public class AdminTylkoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rola = context.HttpContext.Session.GetString("UserRole");
            
            if (rola != "Admin")
            {
                // jesli nie jestes adminem to przekieruj na strone logowania
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            
            base.OnActionExecuting(context);
        }
    }
}