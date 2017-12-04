using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Data.Models;
using WebApplication.Data.Models.Enums;
using WebApplication.Data.ViewModels;

namespace WebApplication.Controllers.Office
{
    [Authorize(Roles ="officeManager,admin")]
    public class OfficeController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private Lazy<ApplicationUser> currentUserModel;
        private readonly Lazy<Place> currentPlace;

        public OfficeController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            currentUserModel = new Lazy<ApplicationUser>(() => _userManager.GetUserAsync(User).Result);
            currentPlace = new Lazy<Place>(() => _context.Offices.Include(p => p.City).SingleOrDefault(p => p.Id == currentUserModel.Value.PlaceId));
        }

        public IActionResult Index()
        {
            var model = new OfficeViewModel
            {
                Cartriges = _context.Cartridges.Include(c => c.Printer).Include(c => c.Place).ThenInclude(pl => pl.City)
                    .Where(c => c.PlaceId == currentPlace.Value.Id),
                Printers = _context.Printers.Include(p => p.Cartidge)
                .Include(p => p.Office).ThenInclude(pl => pl.City)
                .Where(p => p.OfficeId == currentPlace.Value.Id)
            };
            ViewBag.CurrentPlace = currentPlace.Value;
            ViewData.Model = model;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmStatus(Guid? id)
        {
            if (id == null) return NotFound();
            var cartrige = await _context.Cartridges.SingleOrDefaultAsync(c => c.Id == id);
            if (cartrige == null) return NotFound();
            cartrige.PendingConfirmation = false;
            _context.Update(cartrige);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddPrinter() => View();

        [HttpPost]
        public async Task<IActionResult> AddPrinter([Bind("Id", "Type")]Printer printer)
        {
            if (!ModelState.IsValid) return View();
            //var appUser = await _userManager.GetUserAsync(User);
            //var userPlace = _context.Offices.Include(o => o.City).FirstOrDefault(o => o.Id == appUser.PlaceId);
            printer.Office = currentPlace.Value;
            _context.Add(printer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}