using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data.Models.Enums;

namespace WebApplication.Data.Models
{
    public class Cartridge
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(15)]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Номер должен содержать только цифры")]
        [Display(Name = "Инвентарный номер")]
        public string InventoryNumber { get; set; }

        [Display(Name = "Местоположение", AutoGenerateField = false)]
        public Guid PlaceId { get; set; }

        [Display(Name = "Текущий офис")]
        [ForeignKey(nameof(PlaceId))]
        public Place Place { get; set; }

        [Required]
        [Display(Name = "Совместимый тип принтера")]
        public PrinterType CompatiblePrinter { get; set; }

        [Display(Name = "Статус")]
        public CartridgeStatus Status { get; set; }

        //[Display(AutoGenerateField = false)]
        //public Guid? PrinterId { get; set; }

        [Display(Name = "Текущий принтер")]
        //[ForeignKey(nameof(PrinterId))]
        public Printer Printer { get; set; }

        [Display(Name = "Ожидает подтверждения")]
        public bool PendingConfirmation { get; set; }
    }
}
