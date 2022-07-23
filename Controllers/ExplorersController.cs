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
        Kesifmekanholder kesifmekanholder = new Kesifmekanholder();
        Kesifler kesifler = new Kesifler();
        Mekantürleri mekanturleri = new Mekantürleri();
        public IActionResult Index()
        {
            db.Kesiflers.ToList();
            db.SaveChanges();
            return View(db.Kesiflers);
        }
        public IActionResult MekanIndex(int RowId)
        {
            db.Urunlers.ToList();
            ViewBag.List= db.Mekantürleris.ToList();
            explorers= db.Kesiflers.Find(RowId);
            ViewBag.KesifAdi = explorers.Ad;
            db.Mekantürleris.ToList();
          
            return View("~/Views/Explorers/MekanIndex.cshtml",db.Urunlers);  // MekanIndex'te seçilen select'in değerine göre ürünler listelenecek.
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
        public IActionResult AddMekanToExplorer(string MekanAdi, string KesifAdi)
        {
            kesifler = db.Kesiflers.FirstOrDefault(x => x.Ad == KesifAdi);
            int RowId = kesifler.Id;
            mekanturleri = db.Mekantürleris.FirstOrDefault(x => x.MekanAdi == MekanAdi);
            kesifmekanholder.MekanId = mekanturleri.Id;
            kesifmekanholder.OtelId = kesifler.Id;
            db.Kesifmekanholders.Add(kesifmekanholder);
            db.SaveChanges();
            return RedirectToAction("MekanIndex", new { RowId = RowId });
        }
        public IActionResult AddProductToMekan()
        {
            return RedirectToAction("MekanIndex");
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
