using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data.Models;

namespace WebApplication.Data.ViewModels
{
    public class OfficeViewModel
    {
        public IEnumerable<Cartridge> Cartriges { get; set; }
        public IEnumerable<Printer> Printers { get; set; }
    }
}
