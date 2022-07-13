using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Remake.Models;
using Remake.Security.Authorization;
using System.Security.Claims;

namespace Remake.Controllers
{
    public class LoginController : Controller
    {
        //Roles AutRoles = new Roles();
        //List<Roller> KeepRoles = new List<Roller>();
        public IActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml");
        }
        public async Task<IActionResult> check(string username, string password)
        {
            try
            {
                var claims = new List<Claim>();
                bool check = false;
                kesifdbContext kesifdb = new kesifdbContext();
                kullanıcı kul = new kullanıcı();
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
            return View();
        }
    }
}
