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
        List<Mekantürleri> mekanturleris = new List<Mekantürleri>();
        List<Kesifmekanholder> KMHolderList = new List<Kesifmekanholder>();
        List<Alanholder> alanHolders = new List<Alanholder>();
        Kesifmekanholder kesifmekanholder = new Kesifmekanholder();
        Kesifler kesifler = new Kesifler();
        Mekantürleri mekanturleri = new Mekantürleri();
        Alanholder alanHolder = new Alanholder();
        public IActionResult Index()
        {
            db.Kesiflers.ToList();
            db.SaveChanges();
            return View(db.Kesiflers);
        }
        public IActionResult MekanIndex(int RowId)
        {
            //db.Kesifmekanholders.Where(x => x.KesifId == RowId).ToList();
            db.Alanholders.ToList();
            ViewBag.List = db.Mekantürleris.ToList();
            explorers = db.Kesiflers.Find(RowId);
            ViewBag.KesifId = RowId;
            ViewBag.KesifAdi = explorers.Ad;
            db.Mekantürleris.ToList();
          
            return View("~/Views/Explorers/MekanIndex.cshtml", db.Alanholders);  // MekanIndex'te seçilen select'in değerine göre ürünler listelenecek.
        }
        public IActionResult DeleteExpolore(string rowAdi)
        {
            try
            {
                if (!string.IsNullOrEmpty(rowAdi.ToUpper()))
                {
                    string temp = rowAdi.TrimStart().ToUpper();
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
        public IActionResult AddAlanToExplorer(string AlanAdi, string KesifAdi,string MekanAdi, string Konum, string Not, string BaglantiNoktasi)
        {
            kesifler = db.Kesiflers.FirstOrDefault(x => x.Ad == KesifAdi.ToUpper());
            int RowId = kesifler.Id;
            bool check1, check2;
            mekanturleri = db.Mekantürleris.FirstOrDefault(x => x.MekanAdi == MekanAdi);

            KMHolderList = db.Kesifmekanholders.Where(x => x.KesifId == RowId & x.MekanId == mekanturleri.Id).ToList();

            if (KMHolderList.Count == 0)
            {
                kesifmekanholder.MekanId = mekanturleri.Id;
                kesifmekanholder.KesifId = kesifler.Id;
                db.Kesifmekanholders.Add(kesifmekanholder);
                db.SaveChanges();
            }
            alanHolders = db.Alanholders.Where(x => x.KesifId == RowId & x.MekanId == mekanturleri.Id & x.AlanAdi == AlanAdi).ToList();
            if(alanHolders.Count == 0)
            {
            alanHolder.MekanId = mekanturleri.Id;
            alanHolder.KesifId = RowId;
            alanHolder.AlanAdi = AlanAdi.ToUpper();
            alanHolder.Konum = Konum;
            alanHolder.Not = Not;
            db.Alanholders.Add(alanHolder);
            db.SaveChanges();
            }

            return RedirectToAction("MekanIndex", new { RowId = RowId });
        }
        public IActionResult AddProductToMekan()
        {
            return RedirectToAction("MekanIndex");
        }
        public List<Mekantürleri> getProductsForMekan(string MekanAdi)
        {
            List<Mekantürleri> alls = new List<Mekantürleri>();
            alls = db.Mekantürleris.Where(x => x.MekanAdi == MekanAdi.ToUpper()).ToList();
            return alls;
        }
        public IActionResult AddExplore(string ExpName)
        {
            try
            {
                if (!string.IsNullOrEmpty(ExpName.ToUpper()))
                {
                    Kesiflers = db.Kesiflers.Where(x => x.Ad == ExpName.ToUpper()).ToList();
                    if (Kesiflers.Count == 0)
                    {
                        explorers.Ad = ExpName.ToUpper();
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
