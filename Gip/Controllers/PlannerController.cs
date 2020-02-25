using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class PlannerController : Controller
    {
        // GET /planner
        public IActionResult Index()
        {
            return View();
        }
    }
}