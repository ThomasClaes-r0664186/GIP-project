using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class PlannerController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}