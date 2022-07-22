using Microsoft.AspNetCore.Mvc;

namespace Remake.Controllers
{
    public class MekanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
