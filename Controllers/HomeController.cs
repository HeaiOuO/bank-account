using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccount.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Controllers
{
    public class HomeController : Controller
    {
        private logRegContext dbContext;
        public HomeController(logRegContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Register");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    dbContext.Add(user);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("AccountPage", user);
                }
            }
            else
            {
                return View("Register");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == user.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Password does not exist!");
                    return View("Login");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return RedirectToAction("AccountPage", userInDb);
                }
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        
        [HttpGet]
        [Route("accountpage")]
        public IActionResult AccountPage()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                System.Console.WriteLine("*************************** User in session ***************************");
                ViewBag.Transaction= dbContext.transactions.Where(a =>a.UserId == HttpContext.Session.GetInt32("UserId"));
                User User=dbContext.users.FirstOrDefault( u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                ViewBag.User = User;
                decimal sum = dbContext.transactions.Sum(x=>x.Amount);
                Info newinfo = new Info{
                    sum = sum,
                    
                };
                return View();
            }
        }

        [HttpPost]
        [Route("transactions")]
        public IActionResult Transactions(Transaction transaction)
        {
            dbContext.Add(transaction);
            dbContext.SaveChanges();
            return RedirectToAction("AccountPage");
        }

    }
}
