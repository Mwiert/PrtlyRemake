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
        public IActionResult Index()
        {
            db.Kategorilers.ToList();
            db.SaveChanges();
            return View(db.Kategorilers);
        }
        public IActionResult AddCategory(string CatName)
        {
            try
            {
                kategoriler.KategoriAdi = CatName;
                db.Kategorilers.Add(kategoriler);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Index");
        }
    }
}
