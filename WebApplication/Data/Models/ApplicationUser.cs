using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
    }
}
