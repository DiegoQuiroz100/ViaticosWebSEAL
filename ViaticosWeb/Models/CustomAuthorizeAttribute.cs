using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ViaticosWeb.Models
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Verificar si la sesión contiene el ID del usuario
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                // Redirigir al login si no hay sesión activa
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    }
                );
            }
            base.OnActionExecuting(filterContext);
        }
    }
}