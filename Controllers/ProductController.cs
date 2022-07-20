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
        List<Urunler> urunlers = new List<Urunler>();
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
        public IActionResult DeleteProduct(string ProductName)
        {
            try
            {
                if (!string.IsNullOrEmpty(ProductName))
                {

                string temp = ProductName.TrimStart();
                prodct = db.Urunlers.FirstOrDefault(x => x.UrunAdi == temp);
                if(prodct != null)
                    {
                        db.Urunlers.Remove(prodct);
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
        public IActionResult AddNewProduct(string UrunKodu, string UrunAdi, string Marka, string Kategori, float satisFiyati, float fiyat/*,string foto*/)
        {
            try
            {
                if (!string.IsNullOrEmpty(UrunKodu) && !string.IsNullOrEmpty(UrunAdi) && !string.IsNullOrEmpty(Marka) && !string.IsNullOrEmpty(Kategori) && !string.IsNullOrEmpty(satisFiyati.ToString()) && !string.IsNullOrEmpty(fiyat.ToString()))
                {
                    urunlers = db.Urunlers.Where(x => x.UrunAdi == UrunAdi && x.Marka == Marka).ToList();
                    if(urunlers.Count == 0)
                    {
                        prodct.UrunKodu = UrunKodu;
                        prodct.UrunAdi = UrunAdi;
                        prodct.Marka = Marka;
                        prodct.UrunKategorisi = Kategori;
                        prodct.SatisFiyati = satisFiyati;
                        prodct.UrunFiyati = fiyat;
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
    }
}
