using AdminUtility.Areas.Admin.Models;
using AdminUtility.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminUtility.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoggedController : Controller
    {
        private  readonly UserManager<IdentityCustomeUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly SignInManager<IdentityCustomeUser> signinManager;

        public LoggedController(ApplicationDbContext _context, SignInManager<IdentityCustomeUser> _signinManager, UserManager<IdentityCustomeUser> _userManager)
        {
            userManager = _userManager;
            signinManager = _signinManager;
            context = _context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {

            if (ModelState.IsValid)
            {
                var user = context.Users.SingleOrDefault(e => e.UserName == model.UserEmail);
                if (user != null)
                {

                    var result = await signinManager.PasswordSignInAsync(model.UserEmail, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ShowUser", "Logged", new { @Areas = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login Credentials!");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "invalid user!");
                    return View();
                }

            }
            else
            {
                return View(model);
            }

        }

        public IActionResult SignUp()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Signup model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityCustomeUser()
                {
                    //Id= model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    PasswordHash = model.Password,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    Phone = model.Phone,
                    City = model.City



                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.message = "User Create Successfully !";
                    return View();
                }
                else
                {
                    foreach (var er in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, er.Description);
                    }
                    return View();
                }
            }
            else
            {
                return View(model);
            }

        }

        public IActionResult ShowUser()
        {
            var count = context.Users.ToList();
            var emp = count.Count();
            ViewBag.UsersCount = count.Count();
            return View(emp);
        }
        public IActionResult GetallData()
        {
            var con = context.Users.ToList();
            return View(con);
        }

    }




}
