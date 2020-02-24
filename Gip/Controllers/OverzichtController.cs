using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class OverzichtController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}