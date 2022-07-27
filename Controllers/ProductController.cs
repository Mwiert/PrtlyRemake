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
        public IActionResult DeleteProduct(string ProductCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(ProductCode))
                {

                string temp = ProductCode.TrimStart().ToUpper();
                prodct = db.Urunlers.FirstOrDefault(x => x.UrunKodu == temp);
                   
                    if (db.Urunholders.Where(x => x.UrunId == prodct.Id).ToList().Count() == 0)
                    {
                        if (prodct != null)
                        {
                            db.Urunlers.Remove(prodct);
                            db.SaveChanges();
                            //kesif aitliği null değil ise silme işlemi olmayacak alert verdir.
                        }
                    }
                }
               
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public JsonResult GetProductByCode(string Send)
        {
            Urunler urunler = new Urunler();
            urunler = db.Urunlers.FirstOrDefault(x => x.UrunKodu == Send.ToUpper());
            return Json(urunler);
        }
        public IActionResult AddNewProduct(string UrunKodu, string UrunAdi, string Marka, string Kategori, float satisFiyati, float fiyat,int UrunAdet)
        {
            try
            {
                if (!string.IsNullOrEmpty(UrunKodu.ToUpper()) && !string.IsNullOrEmpty(UrunAdi.ToUpper()) && !string.IsNullOrEmpty(Marka.ToUpper()) && !string.IsNullOrEmpty(Kategori.ToUpper()) && !string.IsNullOrEmpty(satisFiyati.ToString()) && !string.IsNullOrEmpty(fiyat.ToString()))
                {
                    urunlers = db.Urunlers.Where(x => x.UrunKodu == UrunKodu.ToUpper()).ToList();
                    if(urunlers.Count == 0)
                    {
                        prodct.KullanilanUrunAdet = 0;
                        prodct.UrunKodu = UrunKodu.ToUpper().Trim();
                        prodct.UrunAdi = UrunAdi.ToUpper().Trim();
                        prodct.Marka = Marka.ToUpper().Trim();
                        prodct.UrunKategorisi = Kategori.ToUpper();
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
    }
}
