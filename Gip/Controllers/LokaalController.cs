using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class LokaalController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}