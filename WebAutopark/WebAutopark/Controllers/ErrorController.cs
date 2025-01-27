using Microsoft.AspNetCore.Mvc;

namespace WebAutopark.Controllers
{
    public class ErrorController : Controller
    {
        public async Task<IActionResult> NotFound(string message)
        {
            ViewBag.ErrorMessage = message ?? "The requested resource was not found.";
            return View();
        }

        public async Task<IActionResult> AlreadyExist(string message)
        {
            ViewBag.ErrorMessage = message ?? "The item already exist.";
            return View();
        }

        public async Task<IActionResult> Unexpected()
        {
            ViewBag.ErrorMessage = "An unexpected error occurred.";
            return View();
        }

    }
}
