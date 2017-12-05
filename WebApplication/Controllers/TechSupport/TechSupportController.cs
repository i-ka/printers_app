using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Data.Models;
using WebApplication.Data.Models.Enums;

namespace WebApplication.Controllers.TechSupport
{
    public class TechSupportController : Controller
    {

        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly Lazy<ApplicationUser> _currentUserModel;
        private readonly Lazy<Place> _currentPlace;

        public TechSupportController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _currentUserModel = new Lazy<ApplicationUser>(() => _userManager.GetUserAsync(User).Result);
            _currentPlace = new Lazy<Place>(() => _context.Offices.Include(p => p.City).SingleOrDefault(p => p.Id == _currentUserModel.Value.PlaceId));
        }

        public IActionResult Index()
        {
            var printers = _context.Printers.Include(p => p.Cartidge).Where(p => p.OfficeId == _currentPlace.Value.Id).ToList();
            ViewData.Model = printers;
            ViewBag.CurrentPlace = _currentPlace.Value;
            return View();
        }

        [HttpGet]
        public IActionResult InsertCartrige(Guid? id)
        {
            if (id == null) return NotFound();
            var printer = _context.Printers.Include(p => p.Cartidge).SingleOrDefault(p => p.Id == id);
            if (printer == null) return NotFound();
            if (printer.Cartidge != null) return NotFound();
            var cartriges = _context.Cartridges.Where(c => c.Printer.Id == null
                                                           && c.CompatiblePrinter == printer.Type
                                                           && c.PlaceId == _currentPlace.Value.Id
                                                           && c.Status == CartridgeStatus.Filled);
            ViewBag.Cartriges = cartriges;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertCartrige(Guid? id, [Bind("Id", "CartridgeId")]Printer printer)
        {
            if (printer.Id != id) return NotFound();
            if (!ModelState.IsValid) {
                ViewBag.Cartriges = _context.Cartridges.Where(c => c.Printer.Id == null
                                                                && c.CompatiblePrinter == printer.Type
                                                                && c.PlaceId == _currentPlace.Value.Id);
                return View(printer);
            }
            try
            {
                var entry = _context.Attach(printer);
                var cartrige = _context.Cartridges.SingleOrDefault(c => c.Id == printer.CartridgeId);
                printer.Cartidge = cartrige;
                //printer.Office = _currentPlace.Value;
                entry.Reference(p => p.Cartidge).IsModified = true;
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (_context.Cartridges.Any(c => c.Id == id))
                    return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveCartrige(Guid? id)
        {
            var printer = await _context.Printers.Include(p => p.Cartidge).SingleOrDefaultAsync(p => p.Id == id);
            if (printer == null) return NotFound();
            printer.Cartidge.Printer = null;
            printer.Cartidge = null;
            _context.Update(printer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}