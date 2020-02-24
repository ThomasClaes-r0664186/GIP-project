using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class DeleteController : Controller
    {
        // GET /delete/lokaal
        public IActionResult Lokaal()
        {
            return View();
        }

        // GET /delete/vakken
        public IActionResult Vakken()
        {
            return View();
        }
    }
}