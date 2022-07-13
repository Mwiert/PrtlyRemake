using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;
using Remake.Security.Authorization;

namespace Remake.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        Roles AutRoles = new Roles();
        kullanıcı k = new kullanıcı();
        kesifdbContext Db = new kesifdbContext();
        Roller roller = new Roller(); 
        public IActionResult Index()
        {
            try
            {
                Db.Rollers.ToList();
                Db.Kullanıcıs.ToList();
                Db.SaveChanges();
                ViewBag.RoleList = Db.Rollers.ToList();
                return View(Db.Kullanıcıs);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IActionResult AddUser(string UsersName, string UsersEmail, string UsersPassword, int RoleId /*,string UsersImg*/)
        {
            try
            {
                k.Ad = UsersName;
                k.Email = UsersEmail;
                k.Psswrd = UsersPassword;
                k.RolId = RoleId;
                Db.Kullanıcıs.Add(k);
                Db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex) 
            {

                throw ex;
            }
      
        }
        public IActionResult AddNewRole(string RoleName)
        {
            try
            {
                if (string.IsNullOrEmpty(RoleName))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    roller.RolAdi = RoleName;
                    Db.Rollers.Add(roller);
                    Db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}
