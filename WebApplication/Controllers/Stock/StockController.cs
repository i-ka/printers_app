using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using WebApplication.Data.Models;
using WebApplication.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers.Stock
{
    [Authorize(Roles = "stockManager,admin")]
    public class StockController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StockController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appUser = await _userManager.GetUserAsync(User);
            _context.Entry(appUser).Reference(u => u.Place).Load();
            _context.Entry(appUser.Place).Reference(p => p.City).Load();
            ViewData["currentPlace"] = $"{appUser.Place.PlaceType}, {appUser.Place.City.Name}, {appUser.Place.Address}";
            ViewData.Model = _context.Cartridges.Include(c => c.Place).ThenInclude(p => p.City).Where(c => c.Place == appUser.Place); ;
            return View();
        }

        [HttpGet]
        public IActionResult CreateCartrige() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCartrige([Bind("Id,InventoryNumber,CompatiblePrinter")]Cartridge model)
        {
            if (!ModelState.IsValid) return View();
            var appUser = await _userManager.GetUserAsync(User);
            var userPlace = _context.Offices.Include(o => o.City).FirstOrDefault(o => o.Id == appUser.PlaceId);
            model.Place = userPlace;
            model.Status = CartridgeStatus.Filled;
            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SendCartrige(Guid? id)
        {
            if (id == null) return NotFound();
            var promoCode = await _context.Cartridges.SingleOrDefaultAsync(m => m.Id == id);
            if (promoCode == null) return NotFound();
            ViewBag.Places = _context.Offices.Include(p => p.City);
            return View(promoCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCartrige(Guid? id, [Bind("Id,InventoryNumber,PlaceId")]Cartridge cartrige)
        {
            if (cartrige.Id != id) return NotFound();
            if (!ModelState.IsValid) {
                ViewBag.Places = _context.Offices.Include(p => p.City);
                return View(cartrige);
            }
            cartrige.PendingConfirmation = true;
            try {
                _context.Update(cartrige);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (_context.Cartridges.Any(c => c.Id == id))
                    return NotFound();
                
            }
            return RedirectToAction(nameof(Index));
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
    }
}