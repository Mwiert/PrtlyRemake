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
                string temp = RowAdi.TrimStart();
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
                urunlers= db.Urunlers.Where(x => x.UrunKategorisi == CatName).ToList();
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
                if (!string.IsNullOrEmpty(UrunKodu) && !string.IsNullOrEmpty(UrunAdi) && !string.IsNullOrEmpty(Marka) && !string.IsNullOrEmpty(Kategori) && !string.IsNullOrEmpty(satisFiyati.ToString()) && !string.IsNullOrEmpty(fiyat.ToString()))
                {
                    
                    urunlers = db.Urunlers.Where(x => x.UrunKodu == UrunKodu).ToList();
                    if (urunlers.Count == 0)
                    {
                        prodct.UrunKodu = UrunKodu;
                        prodct.UrunAdi = UrunAdi;
                        prodct.Marka = Marka;
                        prodct.UrunKategorisi = Kategori;
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
                kategorilerList = db.Kategorilers.Where(x => x.KategoriAdi == CatName).ToList();
                if(kategorilerList.Count ==0)
                {
                    kategoriler.KategoriAdi = CatName;
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
