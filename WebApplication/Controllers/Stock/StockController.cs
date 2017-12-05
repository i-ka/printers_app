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

        private readonly Lazy<ApplicationUser> _currentUserModel;
        private readonly Lazy<Place> _currentPlace;

        public StockController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _currentUserModel = new Lazy<ApplicationUser>(() => _userManager.GetUserAsync(User).Result);
            _currentPlace = new Lazy<Place>(() => _context.Offices.Include(p => p.City).SingleOrDefault(p => p.Id == _currentUserModel.Value.PlaceId));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.CurrentPlace = _currentPlace.Value;
            ViewData.Model = _context.Cartridges.Include(c => c.Place).ThenInclude(p => p.City).Where(c => c.Place.City == _currentPlace.Value.City); ;
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
        public async Task<IActionResult> SendCartrige(Guid? id, [Bind("Id","InventoryNumber","PlaceId")]Cartridge cartrige)
        {
            if (cartrige.Id != id) return NotFound();
            if (!ModelState.IsValid) {
                ViewBag.Places = _context.Offices.Include(p => p.City);
                return View(cartrige);
            }
            
            cartrige.PendingConfirmation = true;
            try {
                var entry = _context.Cartridges.Attach(cartrige);
                entry.Property(c => c.PlaceId).IsModified = true;
                entry.Property(c => c.PendingConfirmation).IsModified = true;
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

        [HttpGet]
        public async Task<IActionResult> ReturnFromRefiling(Guid? id)
        {
            if (id == null) return NotFound();
            var cartrige = _context.Cartridges.SingleOrDefault(c => c.Id == id);
            if (cartrige == null) return NotFound();
            var entry = _context.Attach(cartrige);
            cartrige.Status = CartridgeStatus.Filled;
            cartrige.Place = _currentPlace.Value;
            cartrige.PendingConfirmation = true;
            entry.Property(c => c.Status).IsModified = true;
            entry.Reference(c => c.Place).IsModified = true;
            entry.Property(c => c.PendingConfirmation).IsModified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}