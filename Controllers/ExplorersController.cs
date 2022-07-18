using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    [Authorize]
    public class ExplorersController : Controller
    {
        kesifdbContext db = new kesifdbContext();
        Kesifler explorers = new Kesifler();
        public IActionResult Index()
        {
            db.Kesiflers.ToList();
            db.SaveChanges();
            return View(db.Kesiflers);
        }
        public IActionResult AddExplore(string ExpName)
        {
            try
            {
                if (!string.IsNullOrEmpty(ExpName))
                {
                    explorers.Ad = ExpName;
                    db.Kesiflers.Add(explorers);
                    db.SaveChanges();
                }  
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
