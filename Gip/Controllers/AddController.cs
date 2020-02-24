using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class AddController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}