using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Data.Models;
using System.Security.Claims;
using WebApplication.Extentions;
using WebApplication.Data.Models.Enums;

namespace WebApplication.Services
{
    public class DatabaseSeeder
    {
        public readonly DataContext _context;
        public readonly IHostingEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DatabaseSeeder(DataContext context,
            IHostingEnvironment env,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDatabase()
        {
            if (_env.IsProduction()) 
            {
                _context.Database.Migrate();
            }
            if (_context.Database.GetPendingMigrations().Any()) 
            {
                return;
            }

            var moscowCity = _context.Cities.CreateIfNotExists(new City { Name = "Москва" }, c => c.Name == "Москва");
            var yekaterinburgCity = _context.Cities.CreateIfNotExists(new City { Name = "Екатеринбург" }, c => c.Name == "Екатеринбург");
            //var novosibirskCity = _context.Cities.CreateIfNotExists(new City { Name = "Новосибирск" }, c => c.Name == "Новосибирск");
            //var kaliningradCity = _context.Cities.CreateIfNotExists(new City { Name = "Калининград" }, c => c.Name == "Калининград");
            await _context.SaveChangesAsync();

            var firstMoscowOffice = _context.Offices.CreateIfNotExists(new Place {
                City = moscowCity,
                PlaceType = PlaceType.Office,
                Address = "Какая-то, д. 5, оф. 103",
            }, of => of.Address == "Какая-то, д. 5, оф. 103" && of.City.Name == moscowCity.Name && of.PlaceType == PlaceType.Office);
            var secondMoscowOffice = _context.Offices.CreateIfNotExists(new Place
            {
                City = moscowCity,
                PlaceType = PlaceType.Office,
                Address = "Выдуманная, д. 67, оф. 1003",
            }, of => of.Address == "Выдуманная, д. 67, оф. 1003" && of.City.Name == moscowCity.Name && of.PlaceType == PlaceType.Office);
            var firstYekaterinburgOffice = _context.Offices.CreateIfNotExists(new Place
            {
                City = yekaterinburgCity,
                PlaceType = PlaceType.Office,
                Address = "Малышева, д. 5, оф. 103",
            }, of => of.Address == "Малышева, д. 5, оф. 103" && of.City.Name == yekaterinburgCity.Name && of.PlaceType == PlaceType.Office);
            var secondYekaterinburgOffice = _context.Offices.CreateIfNotExists(new Place
            {
                City = yekaterinburgCity,
                PlaceType = PlaceType.Office,
                Address = "Малышева, д. 67, оф. 1003",
            }, of => of.Address == "Малышева, д. 67, оф. 1003" && of.City.Name == yekaterinburgCity.Name && of.PlaceType == PlaceType.Office);
            var moscowStock = _context.Offices.CreateIfNotExists(new Place
            {
                City = moscowCity,
                PlaceType = PlaceType.Stock,
                Address = "Далёкая, д. 113"
            }, s => s.Address == "Далёкая, д. 113" && s.City.Name == moscowCity.Name && s.PlaceType == PlaceType.Stock);
            var yekaterinburgStock = _context.Offices.CreateIfNotExists(new Place
            {
                City = yekaterinburgCity,
                PlaceType = PlaceType.Stock,
                Address = "Складская, д. 5"
            }, s => s.Address == "Складская, д. 5" && s.City.Name == yekaterinburgCity.Name && s.PlaceType == PlaceType.Stock);
            await _context.SaveChangesAsync();

            var adminRole = await CreateRoleIfNotExists("admin");
            await AddClaim(adminRole, new Claim(Constants.AppPageClaimName, "Admin"));
            var user = await CreateUserIfNotExists("root", "dug77543377", moscowCity);
            await AddRole(user, adminRole);

            var stockManagerRole = await CreateRoleIfNotExists("stockManager");
            await AddClaim(stockManagerRole, new Claim(Constants.AppPageClaimName, "Stock"));
            user = await CreateUserIfNotExists("stockManager", "qwe123ewq", yekaterinburgCity);
            await AddRole(user, stockManagerRole);
        }

        private async Task AddClaim(ApplicationRole role, Claim claim) 
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            var existingClaim = claims.FirstOrDefault(c => c.Type == claim.Type);
            if (existingClaim != null) return;
            await _roleManager.AddClaimAsync(role, claim); 
        }

        private async Task AddRole(ApplicationUser user, ApplicationRole role)
        {
            if (await _userManager.IsInRoleAsync(user, role.Name)) return;
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        private async Task<ApplicationUser> CreateUserIfNotExists(string name, string password, City city)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null) {
                return user;
            }
            user = new ApplicationUser
            {
                UserName = name,
                City = city
            };
            await _userManager.CreateAsync(user, password);
            return user;
        }

        private async Task<ApplicationRole> CreateRoleIfNotExists(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null) return role;
            role = new ApplicationRole { Name = roleName };
            await _roleManager.CreateAsync(role);
            return role;
        }
    }
}
