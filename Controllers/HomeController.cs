using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;

namespace Remake.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        kesifdbContext db = new kesifdbContext();
        public IActionResult Index()
        {
            List<HomeExportEntites> entitesList = new List<HomeExportEntites>();
            HomeExportEntites entites = new HomeExportEntites();
            entites.KesifSayisi = db.Kesiflers.Count().ToString();
            entites.UrunSayisi= db.Urunlers.Count().ToString();
            entites.SumUrunAdedi = db.Urunholders.Sum(x => x.UrunAdet).ToString();
            entites.KategoriSayisi = db.Kategorilers.Count().ToString();
            entites.KullanıcıSayisi = db.Kullanıcıs.Count().ToString();
            entitesList.Add(entites);
            ViewBag.Entities = entitesList;
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
