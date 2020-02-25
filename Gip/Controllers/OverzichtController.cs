using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class OverzichtController : Controller
    {
        // GET /Overzicht
        public IActionResult Index()
        {
            return View();
        }

        // GET /overzicht/lokalen
        public ActionResult Lokalen()
        {
            return view();
        }

        // GET /overzicht/vakken
        public ActionResult Vakken()
        {
            return View();
        }
    }
}