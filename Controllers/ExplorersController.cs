using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Remake.Controllers
{
    [Authorize]
    public class ExplorersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
