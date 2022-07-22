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
        public IActionResult MekanIndex(int RowId)
        {
            db.Mekantürleris.ToList();
            explorers= db.Kesiflers.Find(RowId);
            ViewBag.KesifAdi = explorers.Ad;
            db.Mekantürleris.ToList();

            return View("~/Views/Explorers/MekanIndex.cshtml",db.Mekantürleris);  // MekanIndex'te seçilen select'in değerine göre ürünler listelenecek.
        }
        public IActionResult DeleteExpolore(string rowAdi)
        {
            try
            {
                if (!string.IsNullOrEmpty(rowAdi))
                {
                    string temp = rowAdi.TrimStart();
                    explorers = db.Kesiflers.FirstOrDefault(x => x.Ad == temp);
                    if (explorers != null)
                    {
                        db.Kesiflers.Remove(explorers);
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
        public List<Mekantürleri> getProductsForMekan(string MekanAdi)
        {
            List<Mekantürleri> alls = new List<Mekantürleri>();
            alls = db.Mekantürleris.Where(x => x.MekanAdi == MekanAdi).ToList();
            return alls;
        }
        public IActionResult AddExplore(string ExpName)
        {
            try
            {
                if (!string.IsNullOrEmpty(ExpName))
                {
                    Kesiflers = db.Kesiflers.Where(x => x.Ad == ExpName).ToList();
                    if (Kesiflers.Count == 0)
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
