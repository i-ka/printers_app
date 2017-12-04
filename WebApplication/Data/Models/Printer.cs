using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data.Models.Enums;

namespace WebApplication.Data.Models
{
    public class Printer
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public Guid Id { get; set; }

        [Display(Name = "Тип принтера")]
        public PrinterType Type { get; set; }

        [Display(AutoGenerateField = false)]
        public Guid OfficeId { get; set; }

        [Display(Name = "Офис")]
        [ForeignKey(nameof(OfficeId))]
        public Place Office { get; set; }

        [Display(AutoGenerateField = false)]
        public Guid? CartridgeId { get; set; }

        [Display(Name = "Текущий картридж")]
        [ForeignKey(nameof(CartridgeId))]
        public Cartridge Cartidge { get; set; }
    }
}
