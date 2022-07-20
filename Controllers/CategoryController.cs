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
