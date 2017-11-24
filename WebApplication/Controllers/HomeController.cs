using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [Authorize]
        public IActionResult WorkPage()
        {
            var appControler = User.Claims.FirstOrDefault(c => c.Type == Constants.AppPageClaimName)?.Value;
            return RedirectToAction("Index", appControler);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
