using Gip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class EditController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        
        // GET /edit/lokaal
        [HttpGet]
        [Route("edit/lokaal")]
        public IActionResult Lokaal()
        {
            return View();
        }

        // GET /edit/vak
        [HttpGet]
        [Route("edit/vak")]
        public ActionResult Vak()
        {
            return View();
        }
        
        
    }
}