using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        Kategoriler kategoriler = new Kategoriler();
        kesifdbContext db = new kesifdbContext();
        List<Kategoriler> kategorilerList = new List<Kategoriler>();
        List<Urunler> urunlers = new List<Urunler>();
        Urunler prodct = new Urunler();
        public IActionResult Index()
        {
            db.Kategorilers.ToList();
            db.SaveChanges();
            return View(db.Kategorilers);
        }
        public IActionResult DeleteCat(string RowAdi)
        {
            try
            {
                string temp = RowAdi.TrimStart().ToUpper();
                kategoriler = db.Kategorilers.FirstOrDefault(x => x.KategoriAdi == temp);
                if (kategoriler != null)
                {
                    db.Kategorilers.Remove(kategoriler);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult ListIndex(string CatName)
        {
            try
            {
                List<Urunler> urunlers = new List<Urunler>();
                urunlers= db.Urunlers.Where(x => x.UrunKategorisi == CatName.ToUpper()).ToList();
                ViewBag.CatName = CatName;
                db.SaveChanges();
                return View("ListIndex",urunlers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult AddNewProduct(string UrunKodu, string UrunAdi, string Marka, string Kategori, float satisFiyati, float fiyat, int UrunAdet)
        {
            try
            {
                if (!string.IsNullOrEmpty(UrunKodu.ToUpper()) && !string.IsNullOrEmpty(UrunAdi.ToUpper()) && !string.IsNullOrEmpty(Marka.ToUpper()) && !string.IsNullOrEmpty(Kategori.ToUpper()) && !string.IsNullOrEmpty(satisFiyati.ToString()) && !string.IsNullOrEmpty(fiyat.ToString()))
                {
                    
                    urunlers = db.Urunlers.Where(x => x.UrunKodu == UrunKodu.ToUpper()).ToList();
                    if (urunlers.Count == 0)
                    {
                        prodct.UrunKodu = UrunKodu.ToUpper().Trim();
                        prodct.UrunAdi = UrunAdi.ToUpper().Trim();
                        prodct.Marka = Marka.ToUpper().Trim();
                        prodct.UrunKategorisi = Kategori.ToUpper().Trim();
                        prodct.SatisFiyati = satisFiyati;
                        prodct.UrunFiyati = fiyat;
                        prodct.UrunAdet = UrunAdet;
                        db.Urunlers.Add(prodct);
                        db.SaveChanges();
                    }
                }


                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult AddCategory(string CatName)
        {
            try
            {
                kategorilerList = db.Kategorilers.Where(x => x.KategoriAdi == CatName.ToUpper()).ToList();
                if(kategorilerList.Count ==0)
                {
                    kategoriler.KategoriAdi = CatName.ToUpper().Trim();
                    db.Kategorilers.Add(kategoriler);
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
