using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using GG.Models;

namespace GG.Controllers
{
    public class UserController : Controller
    {
        private GGContext _dBContext; 
        public UserController(GGContext context)
        {
            _dBContext = context;
        }
        //Check if user in session 
        public IActionResult CheckSession(string view)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return View(view);
            }
            return RedirectToAction("Index", "Home");
        }
        //GET: /
        [HttpGet("")]
        public IActionResult Index() => CheckSession("Index");

        //GET: /login
        [HttpGet("login")]
        public IActionResult Login() => CheckSession("Login");

        //POST: /login/process
        [HttpPost("login/process")]
        public IActionResult LoginProcess(UserLogin model)
        {
            if(ModelState.IsValid)
            {
                //Check if email exists 
                List<User> users = _dBContext.Users.Where(u => u.Email == model.LoginEmail).ToList();
                if(users.Count == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Incorrect email/password combination");
                    return View("Login", model);
                }
                else 
                {
                    //Check hashed password 
                    PasswordHasher<UserLogin> hasher = new PasswordHasher<UserLogin>();
                    PasswordVerificationResult result = hasher.VerifyHashedPassword(model, users[0].Password, model.LoginPassword);
                    if(result == PasswordVerificationResult.Failed)
                    {
                        ModelState.AddModelError("LoginPassword", "Incorrect emai/password combination");
                        return View("Login", model);
                    }
                    else 
                    {
                        HttpContext.Session.SetInt32("id", users[0].ID);
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
            return View("Login", model);
        }

        //GET: /register
        [HttpGet("register")]
        public IActionResult Register() => CheckSession("Register");

        //POST: /register/process
        [HttpPost("register/process")]
        public IActionResult RegisterProcess(UserRegister model)
        {
            if(ModelState.IsValid)
            {
                //Check for unique username and email
                List<User> usernames = _dBContext.Users.Where(u => u.Username == model.Username).ToList();
                List<User> emails = _dBContext.Users.Where(u => u.Email == model.Email).ToList();
                if(usernames.Count > 0)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View("Register", model);
                }
                else if(emails.Count > 0)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View("Register", model);
                }
                else 
                {
                    //Hash user password
                    PasswordHasher<UserRegister> hasher = new PasswordHasher<UserRegister>();
                    string hashedPassword = hasher.HashPassword(model, model.Password);
                    //Create new instance of user and add to db 
                    User newUser = new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Password = hashedPassword
                    };
                    _dBContext.Add(newUser);
                    _dBContext.SaveChanges();
                    HttpContext.Session.SetInt32("id", newUser.ID);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Register", model);
        }
    }
}
