using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class EditController : Controller
    {
        // GET /edit/lokaal
        [Route("edit/lokaal")]
        public IActionResult Lokaal()
        {
            
            return View();
        }
    }
}