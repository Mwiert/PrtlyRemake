using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    public class MekanController : Controller
    {
        kesifdbContext db = new kesifdbContext();
        Mekantürleri mkn = new Mekantürleri();
        public IActionResult Index()
        {
            db.Mekantürleris.ToList();
            return View(db.Mekantürleris);
        }
        public IActionResult AddNewMekan(string MekanAdi)
        {
            mkn.MekanAdi = MekanAdi;
            db.Mekantürleris.Add(mkn);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
