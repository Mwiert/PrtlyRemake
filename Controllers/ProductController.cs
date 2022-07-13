using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        kesifdbContext db = new kesifdbContext();
        Urunler prodct = new Urunler();
        Kategoriler category = new Kategoriler();
        public IActionResult Index()
        {
            try
            {
                ViewBag.CategoryList = db.Kategorilers.ToList();
                db.SaveChanges();
                return View(db.Urunlers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult AddNewProduct(string UrunKodu, string UrunAdi, string Marka, string Kategori, float satisFiyati, float fiyat/*,string foto*/)
        {
            try
            {
                if (!string.IsNullOrEmpty(UrunKodu) && !string.IsNullOrEmpty(UrunAdi) && !string.IsNullOrEmpty(Marka) && !string.IsNullOrEmpty(Kategori) && !string.IsNullOrEmpty(satisFiyati.ToString()) && !string.IsNullOrEmpty(fiyat.ToString()))
                {
                    prodct.UrunKodu = UrunKodu;
                    prodct.UrunAdi = UrunAdi;
                    prodct.Marka = Marka;
                    prodct.UrunKategorisi = Kategori;
                    prodct.SatisFiyati = satisFiyati;
                    prodct.UrunFiyati = fiyat;
                }
                db.Urunlers.Add(prodct);
                db.SaveChanges();

                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
