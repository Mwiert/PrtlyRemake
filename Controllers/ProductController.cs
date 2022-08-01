﻿using Microsoft.AspNetCore.Authorization;
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
