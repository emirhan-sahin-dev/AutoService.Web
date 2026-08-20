using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.SystemSettingDtos
{
    public class UpdateSystemSettingDto
    {
        public int SystemSettingId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string? CompanyPhone { get; set; }

        public string? CompanyEmail { get; set; }

        public string? CompanyAddress { get; set; }

        public decimal VatRate { get; set; }

        public int CriticalStockLevel { get; set; }

        public int SessionTimeoutMinutes { get; set; }

        public string Currency { get; set; } = "TRY";
    }
}
