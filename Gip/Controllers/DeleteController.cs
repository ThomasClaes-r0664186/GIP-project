using Gip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class DeleteController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        
        // GET /delete/lokaal
        [HttpGet]
        [Route("delete/lokaal")]
        public IActionResult Lokaal()
        {
            return View();
        }

        // GET /delete/vakken
        [HttpGet]
        [Route("delete/vak")]
        public IActionResult Vak()
        {
            
            
            return View();
        }
    }
}