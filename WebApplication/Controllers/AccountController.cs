using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using WebApplication.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebApplication.Data.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData.Model = new LoginViewModel { ReturnUrl = returnUrl };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]LoginViewModel loginData)
        {
            if (!ModelState.IsValid) {
                return View(loginData);
            }
            var result = await _signInManager.PasswordSignInAsync(loginData.Username, loginData.Password, false, true);
            if (result.Succeeded) {
                return string.IsNullOrWhiteSpace(loginData.ReturnUrl) ?
                    (IActionResult)RedirectToAction("Index", "Home") :
                    Redirect(loginData.ReturnUrl);
            } else {
                ModelState.AddModelError("", "Invalid login or password");
            }
            return View(loginData);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}