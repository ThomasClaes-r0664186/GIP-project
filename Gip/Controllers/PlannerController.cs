using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class PlannerController : Controller
    {
        // GET /planner
        [HttpGet]
        [Route("planner")]
        public IActionResult Index()
        {
            return View();
        }
    }
}