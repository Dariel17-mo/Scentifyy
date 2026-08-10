using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class SesionActivaAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Verifica si la acción o el controlador tiene AllowAnonymous
        var isAllowAnonymous = context.ActionDescriptor.EndpointMetadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute));

        if (isAllowAnonymous)
        {
            base.OnActionExecuting(context);
            return;
        }

        var session = context.HttpContext.Session;
        var usuario = session.GetString("UsuarioEmail");

        if (string.IsNullOrEmpty(usuario))
        {
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
        }

        base.OnActionExecuting(context);
    }
}
