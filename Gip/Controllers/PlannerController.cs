using Gip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class PlannerController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        // GET /planner
        [HttpGet]
        [Route("planner")]
        public IActionResult Index()
        {
            return View();
        }
    }
}