using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    [Authorize(Roles = "ADMIN,MUHASEBE")]
    public class ExcelController : Controller
    {
        List<Kesifler> kesifler = new List<Kesifler>();
        Kesifler kesif = new Kesifler();
        ExcelEntites xle = new ExcelEntites();
        List<Urunler> urunlerList = new List<Urunler>();
        Urunholder UrunHolder = new Urunholder();
        Alanholder alanholder = new Alanholder();
        public IActionResult Index()
        {
            using (var db = new kesifdbContext())
            {
                kesifler = new List<Kesifler>();
                kesifler = db.Kesiflers.ToList();
                
            }
                return View(kesifler);
        }
        public JsonResult ExcelKaydet(int kesifid,float DolarKuru)
        {
            xle = new ExcelEntites();
            urunlerList = new List<Urunler>();
            kesif = new Kesifler();
            string requestName = User.Identity.Name;
            using (var db = new kesifdbContext())
            {
                kesif = db.Kesiflers.FirstOrDefault(x=>x.Id ==kesifid);
                var AlanAndUrun = (from alan in db.Alanholders
                                   join kesf in db.Kesiflers
                                   on alan.KesifId equals kesf.Id
                                   where kesf.Id == kesifid
                                   join uholder in db.Urunholders
                                   on alan.Id equals uholder.AlanId
                                   join urun in db.Urunlers
                                   on uholder.UrunId equals urun.Id
                                   select new
                                   {
                                       UrunAdi =urun.UrunAdi,
                                       urunAdedi = uholder.UrunAdet,
                                       UrunKodu = urun.UrunKodu,
                                       UrunKategorisi = urun.UrunKategorisi,
                                       BirimFiyat = urun.UrunFiyati,
                                       SatisFiyati= urun.SatisFiyati,
                                   }) ;
            }
            return Json(0);
        }
        public JsonResult ExcelMailGonder(int kesidId, decimal DolarKuru)
        {
            using (var db = new kesifdbContext())
            {

            }
            return Json(0);
        }
    }
}
