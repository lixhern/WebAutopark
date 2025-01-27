using Microsoft.AspNetCore.Mvc;

namespace WebAutopark.Controllers
{
    public class ErrorController : Controller
    {
        public async Task<IActionResult> NotFound()
        {
            var errorMessage = HttpContext.Items["ErrorMessage"]?.ToString() ?? "The requested resource was not found.";
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }
    }
}
