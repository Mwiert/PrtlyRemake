using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remake.Models;
using Remake.Security.Authorization;
using Remake.Security.Validations.ResetPassword;
using System.Security.Claims;

namespace Remake.Controllers
{
    public class LoginController : Controller
    {
        //Roles AutRoles = new Roles();
        //List<Roller> KeepRoles = new List<Roller>();
        kesifdbContext kesifdb = new kesifdbContext();
        Kullanıcı kul = new Kullanıcı();
        List<Kullanıcı> kuls = new List<Kullanıcı>();
        rePassword rePass = new rePassword();
        public IActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml",kesifdb.Kullanıcıs);
        }
        public JsonResult CheckEmail(string userEmail)
        {
            bool isExits = kesifdb.Kullanıcıs.ToList().Exists(x => x.Email.Equals(userEmail, StringComparison.CurrentCultureIgnoreCase));
            if(isExits == true)
            {
                rePass.SendVerifyMail(userEmail);
            }
            return Json(isExits);
        }
        public JsonResult updatePassword(string passwordFirst, string redirectEmail)
        {
            kul = kesifdb.Kullanıcıs.FirstOrDefault(x => x.Email == redirectEmail);
            kul.Psswrd = passwordFirst;
            kesifdb.Entry(kul).State = EntityState.Modified;
            kesifdb.SaveChanges();
            return Json(true);
        }
        public JsonResult CheckVerifyCode(int verifyCode)
        {
            bool temp = false;
            if (rePass.CheckVerifyCode(verifyCode) == true)
            {
                temp = true;
                return Json(temp);
            }
            else { return Json(temp); }
        }
        public async Task<IActionResult> check(string username, string password)
        {
            try
            {
                var claims = new List<Claim>();

                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                {
                    return View("~/Views/Login/Login.cshtml");
                }
                else
                {
                    kul = kesifdb.Kullanıcıs.FirstOrDefault(x => x.Email == username && x.Psswrd == password);
                    if( kul == null)
                    {
                        return View("~/Views/Login/Login.cshtml");
                    }
                    else
                    {
                        kesifdb.Rollers.FirstOrDefault(x => x.RolId == kul.RolId);
                        claims.Add(new Claim(ClaimTypes.Email, username));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                        claims.Add(new Claim(ClaimTypes.Role, kul.Rol.RolAdi));
                        claims.Add(new Claim(ClaimTypes.Name, kul.Ad));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrencipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrencipal);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
