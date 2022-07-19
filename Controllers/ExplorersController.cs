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
        List<Kesifler> Kesiflers = new List<Kesifler>();
        public IActionResult Index()
        {
            db.Kesiflers.ToList();
            db.SaveChanges();
            return View(db.Kesiflers);
        }
        public IActionResult DeleteExpolore(int RowId)
        {
            try
            {
                if (!string.IsNullOrEmpty(RowId.ToString()))
                {
                    explorers = db.Kesiflers.Find(RowId);
                    db.Kesiflers.Remove(explorers);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Index");
        }
        public IActionResult AddExplore(string ExpName)
        {
            try
            {
                if (!string.IsNullOrEmpty(ExpName))
                {
                    Kesiflers = db.Kesiflers.Where(x => x.Ad == ExpName).ToList();
                    if (Kesiflers == null)
                    {
                        explorers.Ad = ExpName;
                        db.Kesiflers.Add(explorers);
                        db.SaveChanges();
                    }

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
