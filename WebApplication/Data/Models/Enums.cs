using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Data.Models.Enums
{
    public enum PrinterType
    {
        Epson,
        Xerox,
        Acer,
        Hp
    }

    public enum CartridgeStatus {
        Filled,
        Empty,
        Broken
    }

    public enum PlaceType {
        Office,
        Stock,
        Refiling,
        Trash
    }
}
