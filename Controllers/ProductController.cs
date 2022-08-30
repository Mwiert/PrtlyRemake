using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remake.Models;
using System.Globalization;

namespace Remake.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        kesifdbContext db = new kesifdbContext();
        Urunler prodct = new Urunler();
        Kategoriler category = new Kategoriler();
        List<Urunler> urunlers = new List<Urunler>();
        Urunler urun = new Urunler();
        Paket pack = new Paket();
        List<Paket> packs = new List<Paket>();
        public static List<Paketholder> listPaketStatic = new List<Paketholder>();
        Paketholder packHolder = new Paketholder();
        List<Paketholder> paketholders = new List<Paketholder>();
        int counter;
        static int paketidHolder;
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
        public JsonResult getPackUruns(int packid)
        {
            var data = from c in db.Paketholders
                       join o in db.Urunlers
                       on c.UrunId equals o.Id
                       where c.PaketId == packid
                       select new
                       {
                           urunAdi = o.UrunAdi,
                           urunKodu = o.UrunKodu,
                           urunAdedi = c.UrunAdeti
                       };

            return Json(data);
        }
        public JsonResult GetPaketList()
        {
            packs = new List<Paket>();
            packs = db.Pakets.ToList();
            return Json(packs);
            
        }
        public JsonResult restorePackList()
        {
            listPaketStatic.Clear();
            return Json(1);
        }
        public JsonResult SavePack(int paketid)
        {
            paketid = paketidHolder;
            if (paketid == 0)
            {
              return Json(2);
            }
            if (listPaketStatic.Count == 0)
            {
                return Json(0);
            }
            else
            {
                packHolder = new Paketholder();
                foreach(var item in listPaketStatic)
                {
                        packHolder = item;
                        db.Paketholders.Add(packHolder);
                        db.SaveChanges();
                }
                return Json(1);
            }
        }
        public JsonResult AddToPaketHolderTwo(string urunKodu, int adet, string packId)
        {
            urun = new Urunler();
            urun = db.Urunlers.FirstOrDefault(x => x.UrunKodu == urunKodu);
            pack = new Paket();
            pack = db.Pakets.FirstOrDefault(x => x.PaketAdi == packId);

            if (listPaketStatic.Count == 0)
            {
                packHolder = new Paketholder();
                packHolder.UrunAdeti = adet;
                packHolder.UrunId = urun.Id;
                packHolder.PaketId = pack.Id;
                listPaketStatic.Add(packHolder);
                return Json(urun);
            }
            else
            {
                foreach (var item in listPaketStatic)
                {
                    if (item.UrunId == urun.Id)
                    {
                        return Json(0);  // ürün zaten ekli
                    }
                    else
                    {
                        counter++;
                    }
                }
                if (counter == listPaketStatic.Count)
                {
                    packHolder = new Paketholder();
                    packHolder.UrunAdeti = adet;
                    packHolder.UrunId = urun.Id;
                    packHolder.PaketId = pack.Id;
                    listPaketStatic.Add(packHolder);
                    return Json(urun);
                }
            }
            return Json(1);
        }
        public JsonResult AddToPaketHolder(string urunKodu,int adet,int packId)
        {
            urun = new Urunler();
            urun = db.Urunlers.FirstOrDefault(x=>x.UrunKodu== urunKodu);
            Paketholder phnew = new Paketholder();
            phnew = db.Paketholders.FirstOrDefault(x => x.PaketId == packId && x.UrunId == urun.Id);
            if (phnew == null)
            {
                if (listPaketStatic.Count == 0)
                {
                    packHolder = new Paketholder();
                    packHolder.UrunAdeti = adet;
                    packHolder.UrunId = urun.Id;
                    packHolder.PaketId = packId;
                    listPaketStatic.Add(packHolder);
                    return Json(urun);
                }
                else
                {
                    foreach (var item in listPaketStatic)
                    {
                        if (item.UrunId == urun.Id)
                        {
                            return Json(0);  // ürün zaten ekli
                        }
                        else
                        {
                            counter++;
                        }
                    }
                    if (counter == listPaketStatic.Count)
                    {
                        packHolder = new Paketholder();
                        packHolder.UrunAdeti = adet;
                        packHolder.UrunId = urun.Id;
                        packHolder.PaketId = packId;
                        listPaketStatic.Add(packHolder);
                        return Json(urun);
                    }
                }
            }
            else
            {
                return Json(0);
            }
            return Json(1);
        }
        public JsonResult GetUrunList()
        {
            urunlers = new List<Urunler>();
            db = new kesifdbContext();
            urunlers = db.Urunlers.ToList();
            return Json(urunlers);
        }
        public JsonResult delPaket(int Paketid)
        {
            pack = new Paket();
            pack = db.Pakets.FirstOrDefault(x=>x.Id == Paketid);
            db.Pakets.Remove(pack);
            db.SaveChanges();
            return Json(1);
        }
        public JsonResult listPaket()
        {
            db = new kesifdbContext();
            packs = new List<Paket>();
            packs = db.Pakets.ToList();
            return Json(packs);
        }
        public JsonResult addPack(string PName)
        {
            if (string.IsNullOrEmpty(PName))
            {
                return Json(0);
            }
            else
            {
                string PCName =PName.ToUpper().Trim();
                pack = db.Pakets.FirstOrDefault(x => x.PaketAdi == PCName);
                if (pack != null)
                {
                    return Json(2);
                }
                else {            
                    pack = new Paket();
                    pack.PaketAdi = PCName;
                    db.Pakets.Add(pack);
                    db.SaveChanges();
                    paketidHolder = pack.Id;
                    return Json(1);
                }
            }
        }
        public JsonResult updateProduct(string UK,string UM, string USF , int UA, string UF, string UC, string UAdi)
        {
            Stokısenabled senab = new Stokısenabled();
            senab = db.Stokısenableds.FirstOrDefault(x=>x.Id==1);
            float USFCon = float.Parse(USF, CultureInfo.InvariantCulture.NumberFormat);
            float UFCon = float.Parse(UF, CultureInfo.InvariantCulture.NumberFormat);
            urun = db.Urunlers.FirstOrDefault(x => x.UrunKodu == UK);
            if(senab.IsEnabled == 1)
            {
            if(urun.KullanilanUrunAdet > UA)
            {
               return Json("HATA");
            }
            else
            {
            urun.UrunAdet = UA;
            urun.UrunAdi = UAdi.ToUpper();
            urun.UrunFiyati = UFCon;
            urun.SatisFiyati = USFCon;
            urun.Marka = UM;
            urun.UrunKategorisi = UC;
            db.Entry(urun).State = EntityState.Modified;
            db.SaveChanges();
            return Json(urun);
            }
            }
            else
            {
                urun.UrunAdet = UA;
                urun.UrunAdi = UAdi.ToUpper();
                urun.UrunFiyati = UFCon;
                urun.SatisFiyati = USFCon;
                urun.Marka = UM;
                urun.UrunKategorisi = UC;
                db.Entry(urun).State = EntityState.Modified;
                db.SaveChanges();
                return Json(urun);
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
        public IActionResult AddNewProduct(string UrunKodu, string UrunAdi, string Marka, string Kategori, string satisFiyati, string fiyat,int UrunAdet)
        {
            try
            {
                float USFCon = float.Parse(satisFiyati, CultureInfo.InvariantCulture.NumberFormat);
                float UFCon = float.Parse(fiyat, CultureInfo.InvariantCulture.NumberFormat);
                Stokısenabled isStokKontrolEnabled = new Stokısenabled();
                isStokKontrolEnabled = db.Stokısenableds.FirstOrDefault(x => x.Id == 1);
                if(isStokKontrolEnabled.IsEnabled == 1)
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
                        prodct.SatisFiyati = USFCon;
                        prodct.UrunFiyati = UFCon;
                        prodct.UrunAdet = UrunAdet;
                        db.Urunlers.Add(prodct);
                        db.SaveChanges();
                    }

                }
                }
                else
                {
                    if (!string.IsNullOrEmpty(UrunKodu.ToUpper()) && !string.IsNullOrEmpty(UrunAdi.ToUpper()) && !string.IsNullOrEmpty(Marka.ToUpper()) && !string.IsNullOrEmpty(Kategori.ToUpper()) && !string.IsNullOrEmpty(satisFiyati.ToString()) && !string.IsNullOrEmpty(fiyat.ToString()))
                    {
                        urunlers = db.Urunlers.Where(x => x.UrunKodu == UrunKodu.ToUpper()).ToList();
                        if (urunlers.Count == 0)
                        {
                            prodct.KullanilanUrunAdet = 0;
                            prodct.UrunKodu = UrunKodu.ToUpper().Trim();
                            prodct.UrunAdi = UrunAdi.ToUpper().Trim();
                            prodct.Marka = Marka.ToUpper().Trim();
                            prodct.UrunKategorisi = Kategori.ToUpper();
                            prodct.SatisFiyati = USFCon;
                            if(UrunAdet == null)
                            {
                                UrunAdet = 0;
                            }
                            else
                            {
                            prodct.UrunAdet = UrunAdet;
                            }
                            prodct.UrunFiyati = UFCon;
                            db.Urunlers.Add(prodct);
                            db.SaveChanges();
                        }

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
