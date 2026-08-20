using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Enums
{
    public enum ServiceOperationStatus
    {
        Waiting = 1,
        InProgress = 2,
        WaitingForPart = 3,
        Completed = 4,
        QualityControl = 5,
        ReadyForDelivery = 6,
        Cancelled = 7
    }
}
