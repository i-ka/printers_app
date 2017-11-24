using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data.Models
{
    public class City
    {
        public City()
        {
            Offices = new HashSet<Place>();
            Users = new HashSet<ApplicationUser>();
        }

        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        public HashSet<Place> Offices { get; set; }
        public HashSet<ApplicationUser> Users { get; set; }
    }
}
