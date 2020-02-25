using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class VakController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}