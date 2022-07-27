using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        List<Urunler> urunlers = new List<Urunler>();
        Kesifmekanholder kesifmekanholder = new Kesifmekanholder();
        Kesifler kesifler = new Kesifler();
        Urunler urun = new Urunler();
        Mekantürleri mekanturleri = new Mekantürleri();
        Alanholder alanHolder = new Alanholder();
        Urunholder Urunholder = new Urunholder();
        public static int KesifIdInt;
        public IActionResult Index()
        {
            db.Kesiflers.ToList();
            db.SaveChanges();
            return View(db.Kesiflers);
        }
        public IActionResult DeleteAlan(string RowAdi)
        {
            alanHolder = db.Alanholders.FirstOrDefault(x => x.AlanAdi == RowAdi && x.KesifId == KesifIdInt);
            var isBool = db.Urunholders.Where(x => x.AlanId == alanHolder.Id).ToList();
            if(isBool.Count == 0)
            {
            db.Alanholders.Remove(alanHolder);
                db.SaveChanges();
            }
            else
            {
                //içerisinde ürün varken silemezsiniz.
            }
            return RedirectToAction("MekanIndex", new {RowId = KesifIdInt});
        }
        public JsonResult mekangetir(string p)
        {
            Mekantürleri jsnmt = new Mekantürleri();
            if (p == "--")
            {
                alanHolders = db.Alanholders.Where(x => x.KesifId == KesifIdInt).ToList();
            }
            else { 
            jsnmt = db.Mekantürleris.FirstOrDefault(x => x.MekanAdi == p);
            if(jsnmt != null)
            {
                alanHolders = db.Alanholders.Where(x => x.MekanId == jsnmt.Id && x.KesifId ==KesifIdInt).ToList();
            }
            }
            return Json(alanHolders);
        }
        public IActionResult DeleteProductFromAlan(int Urunid, int alanid)
        {
            Urunholder = db.Urunholders.FirstOrDefault(x => x.UrunId == Urunid && x.AlanId == alanid);

            alanHolder = db.Alanholders.FirstOrDefault(x => x.Id == alanid);
            db.Alanholders.Remove(alanHolder);
            return RedirectToAction("AlanIndex", new { RowId = alanid });
        }
        public IActionResult MekanIndex(int RowId)
        {
            //db.Kesifmekanholders.Where(x => x.KesifId == RowId).ToList();
            db.Alanholders.ToList();
            ViewBag.List = db.Mekantürleris.ToList();
            explorers = db.Kesiflers.Find(RowId);
            KesifIdInt = RowId;
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
        public IActionResult AlanIndex(int RowId)
        {
            alanHolder = db.Alanholders.FirstOrDefault(x => x.Id == RowId);
            ViewBag.AlanAdi = alanHolder.AlanAdi;
            ViewBag.RowId = alanHolder.Id ;
            ViewBag.UrunList =db.Urunholders.ToList();
            ViewBag.CatList = db.Kategorilers.ToList();
            db.Urunlers.ToList();
            return View("AlanIndex", db.Urunlers);
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
            alanHolder.AlanAdi = AlanAdi.ToUpper().Trim();
            alanHolder.Konum = Konum;
            alanHolder.Not = Not;
            db.Alanholders.Add(alanHolder);
            db.SaveChanges();
            }

            return RedirectToAction("MekanIndex", new { RowId = RowId });
        } 
        public JsonResult GetUrunForSelected(string p)
        {
            if (p == "--")
            {
                urunlers = db.Urunlers.ToList();
            }
            else
            {
            urunlers  = db.Urunlers.Where(x => x.UrunKategorisi == p ).ToList();
            }
            return Json(urunlers);
        }
        public JsonResult getProductAdetLeft(string p)
        {
            if (p != null)
            {
                if(p.Trim() =="--")
                {
                    return Json("Ürün Seçiniz");
                }
                else
                {
                urun = db.Urunlers.FirstOrDefault(x => x.UrunKodu == p);
                    if(urun.KullanilanUrunAdet == null)
                    {
                        if(urun.UrunAdet == null)
                        {
                            urun.UrunAdet = 0;
                        }
                        urun.KullanilanUrunAdet = 0;
                    }
                var EklenebilecekUrun = ((urun.UrunAdet - urun.KullanilanUrunAdet));
                    return Json(EklenebilecekUrun);
                }
               
            }
            else
            {
                return Json("0");
            }
        
        }
        public IActionResult AddProductToAlan(string UrunKodu, int UrunAdedi, int RowId)
        {
            int Compare;
            Urunler urunler = new Urunler();
            Urunholder urunholder = new Urunholder();
            urunler = db.Urunlers.FirstOrDefault(x => x.UrunKodu == UrunKodu);
            urunholder = db.Urunholders.FirstOrDefault(x => x.UrunId == urunler.Id && x.AlanId == RowId);
            int cross = ((int)(urunler.UrunAdet - urunler.KullanilanUrunAdet));
            if (urunler.KullanilanUrunAdet <= urunler.UrunAdet)
            {
                if(UrunAdedi <= cross)
                {     
                    Compare = ((int)(urunler.KullanilanUrunAdet + UrunAdedi));
                    if (urunler.UrunAdet <= Compare)
                    { 
                        urunholder.UrunAdet = urunler.UrunAdet;
                        urunler.KullanilanUrunAdet = urunler.UrunAdet;
                    }
                    else
                    {
                        urunholder.UrunAdet = Compare;
                        urunler.KullanilanUrunAdet = Compare;
                    }
                    
                }
                else
                {
            urunholder.UrunAdet = (urunler.UrunAdet - urunler.KullanilanUrunAdet);
                    //HTTP RESPONSE İLE DURUM MESAJI VERİLİR. STOKTA BU KADAR BULUNDUĞU İÇİN ŞU KADAR EKLENMİŞTİR.
                }
            }
            urunholder.AlanId = RowId;
            urunholder.UrunId = urunler.Id;
            db.Entry(urunholder).State = EntityState.Modified;
            db.Entry(urunler).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AlanIndex", new { RowId = RowId });
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
