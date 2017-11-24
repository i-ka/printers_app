using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data.Models.Enums;

namespace WebApplication.Data.Models
{
    public class Place
    {
        public Place()
        {
            Cartidges = new Cartidge[0];
            Printers = new Printer[0];
        }

        [Key]
        [Display(AutoGenerateField = false)]
        public Guid Id { get; set; }

        [Display(AutoGenerateField = false)]
        public Guid CityId { get; set; }

        [Display(Name = "Город")]
        [ForeignKey(nameof(CityId))]
        public City City { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Тип места")]
        public PlaceType PlaceType { get; set; }

        public Cartidge[] Cartidges { get; set; }
        public Printer[] Printers { get; set; }
    }
}
