using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Enums;

public enum ServiceStatus
{
    Bekliyor = 1,
    Islemde = 2,
    ParcaBekleniyor = 3,
    Tamamlandi = 4,
    IptalEdildi = 5
}
