using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class OverzichtController : Controller
    {
        // GET /Overzicht
        [HttpGet]
        [Route("overzicht")]
        public IActionResult Index()
        {
            return View();
        }

        // GET /overzicht/lokalen
        [HttpGet]
        [Route("overzicht/lokalen")]
        public ActionResult Lokalen()
        {
            return view();
        }

        // GET /overzicht/vakken
        [HttpGet]
        [Route("overzicht/vakken")]
        public ActionResult Vakken()
        {
            return View();
        }
    }
}