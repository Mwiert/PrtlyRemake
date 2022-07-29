using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MekanController : Controller
    {
        kesifdbContext db = new kesifdbContext();
        Mekantürleri mkn = new Mekantürleri();
        List<Alanholder> alanHolders = new List<Alanholder>();
        List<Urunholder> urunHolders = new List<Urunholder>();
        List<Urunler> urunlers = new List<Urunler>();
        Mekantürleri mt = new Mekantürleri();
        
        public IActionResult Index()
        {
            db.Mekantürleris.ToList();
            return View(db.Mekantürleris);
        }
        public JsonResult DeleteMekan(string DeleteMekan)
        {
           mt= db.Mekantürleris.FirstOrDefault(x => x.MekanAdi == DeleteMekan);
            var check = db.Alanholders.FirstOrDefault(x => x.MekanId == mt.Id);
            if(check != null)
            {
                return Json("HATA");
            }
            else
            {
                db.Mekantürleris.Remove(mt);
                db.SaveChanges();
                return Json("OK");
            }
        }
        public JsonResult getProductForSelected(string p)
        {
            List<Urunler> list = new List<Urunler>();
            urunlers = db.Urunlers.ToList();
            if(p == "--")
            {
                list = urunlers;
            }
            else
            {
            mkn = db.Mekantürleris.FirstOrDefault(x => x.MekanAdi == p);
            alanHolders = db.Alanholders.Where(x => x.MekanId == mkn.Id).ToList();
            if(alanHolders.Count != 0)
            {

            urunHolders = db.Urunholders.ToList();
            foreach(var item in urunHolders)
            {
                foreach(var item2 in alanHolders)
                {

                if (item.AlanId ==item2.Id)
                {
                        foreach(var item3 in urunlers)
                        {
                            if (item.UrunId == item3.Id)
                            {
                                list.Add(item3);
                            }
                        }
                }
                }
            }
            }
            }
            return Json(list);
        }
        public IActionResult AddNewMekan(string MekanAdi)
        {

            mkn.MekanAdi = MekanAdi.ToUpper().Trim();
            db.Mekantürleris.Add(mkn);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
