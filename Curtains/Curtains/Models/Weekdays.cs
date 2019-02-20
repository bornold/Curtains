using System;
namespace Curtains.Models
{
    [Flags]
    public enum Days
    {
        non = 0x0,
        sun = 0x1,
        mon = 0x2,
        tue = 0x4,
        wed = 0x8,
        thu = 0x10,
        fri = 0x20,
        sat = 0x40,
        all = 0x7F,
    }
}
