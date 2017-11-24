using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data.Models.Enums;

namespace WebApplication.Data.Models
{
    public class Cartidge
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public Guid Id { get; set; }

        [Display(AutoGenerateField = false)]
        public Guid OfficeId { get; set; }

        [Display(Name = "Текущий офис")]
        [ForeignKey(nameof(OfficeId))]
        public Place Office { get; set; }

        [Display(Name = "Совместимый тип принтера")]
        public PrinterType CompatiblePrinter { get; set; }

        [Display (Name = "Статус")]
        public CartridgeStatus Status { get; set; }

        [Display(AutoGenerateField = false)]
        public Guid PrinterId { get; set; }

        [Display(Name = "Текущий принтер")]
        [ForeignKey(nameof(PrinterId))]
        public Printer Printer { get; set; }

        [Display(Name = "Ожидает подтверждения")]
        public bool PendingConfirmation { get; set; }
    }
}
