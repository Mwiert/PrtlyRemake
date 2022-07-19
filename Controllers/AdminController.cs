﻿using Microsoft.AspNetCore.Authorization;
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
        List<kullanıcı> kullanıcıs = new List<kullanıcı>();
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
        public IActionResult DeleteUser(int RowId)
        {
            try
            {
                if (!string.IsNullOrEmpty(RowId.ToString()))
                {
                    k = Db.Kullanıcıs.Find(RowId);
                    Db.Kullanıcıs.Remove(k);
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("Index");
        }
        public IActionResult AddUser(string UsersName, string UsersEmail, string UsersPassword, int RoleId /*,string UsersImg*/)
        {
            try
            {
                kullanıcıs = Db.Kullanıcıs.Where(x => x.Email == UsersEmail).ToList();
                if(kullanıcıs.Count ==0 )
                {
                    k.Ad = UsersName;
                    k.Email = UsersEmail;
                    k.Psswrd = UsersPassword;
                    k.RolId = RoleId;
                    Db.Kullanıcıs.Add(k);
                    Db.SaveChanges();
                }
              
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
