using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.ReportDtos;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ReportRepository : IReportRepository
{
    private readonly AutoServiceContext _context;

    public ReportRepository(AutoServiceContext context)
    {
        _context = context;
    }

    public async Task<ReportDashboardDto> GetDashboardReportAsync(
        DateTime? startDate,
        DateTime? endDate)
    {
        var serviceQuery = _context.ServiceRecords
            .AsNoTracking()
            .AsQueryable();

        // Başlangıç tarihi seçildiyse o tarihten sonraki kayıtlar alınır.
        if (startDate.HasValue)
        {
            var start = startDate.Value.Date;

            serviceQuery = serviceQuery.Where(x =>
                x.CheckInDate >= start);
        }

        /*
         * Bitiş tarihini gün sonu dahil olacak şekilde filtreler.
         *
         * Örneğin 15.07.2026 seçildiyse,
         * 16.07.2026 tarihinden küçük kayıtlar alınır.
         */
        if (endDate.HasValue)
        {
            var endExclusive =
                endDate.Value.Date.AddDays(1);

            serviceQuery = serviceQuery.Where(x =>
                x.CheckInDate < endExclusive);
        }

        /*
         * Servis kaydıyla birlikte:
         *
         * - Servis işlemleri
         * - Atanan ustalar
         * - Usta uzmanlıkları
         * - Kullanılan parçalar
         * - Yedek parça bilgileri
         *
         * yüklenir.
         */
        var serviceRecords = await serviceQuery
            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.Mechanic)
                    .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.ServiceOperationParts)
                    .ThenInclude(x => x.SparePart)

            .OrderByDescending(x => x.CheckInDate)
            .ToListAsync();

        /*
         * Soft delete yapılmamış bütün servis işlemleri.
         */
        var serviceOperations = serviceRecords
            .SelectMany(x => x.ServiceOperations)
            .Where(x => !x.IsDeleted)
            .ToList();

        /*
         * Aktif işlemlere bağlı, soft delete yapılmamış parçalar.
         */
        var operationParts = serviceOperations
            .SelectMany(x => x.ServiceOperationParts)
            .Where(x => !x.IsDeleted)
            .ToList();

        var lowStockParts = await _context.SpareParts
            .AsNoTracking()
            .Where(x => x.StockQuantity <= 10)
            .OrderBy(x => x.StockQuantity)
            .ThenBy(x => x.PartName)
            .ToListAsync();

        /*
         * Müşteriye yansıtılan toplam işçilik tutarı.
         */
        var totalLaborRevenue = serviceOperations
            .Sum(x => x.CustomerLaborPrice);

        /*
         * Müşteriye yansıtılan toplam parça tutarı.
         */
        var totalPartRevenue = operationParts
            .Sum(x => x.TotalPrice);

        /*
         * Müşterinin ödeyeceği genel toplam.
         */
        var totalRevenue =
            totalLaborRevenue +
            totalPartRevenue;

        var report = new ReportDashboardDto
        {
            StartDate = startDate,
            EndDate = endDate,

            TotalServiceCount =
                serviceRecords.Count,

            TotalRevenue =
                totalRevenue,

            /*
             * DTO alanının adı TotalLaborCost olarak kaldı.
             * Buraya müşteriye yansıtılan toplam işçilik
             * bedelini aktarıyoruz.
             */
            TotalLaborCost =
                totalLaborRevenue,

            /*
             * Gerçek teslim tarihi bulunan araçlar,
             * müşteriye teslim edilmiş kabul edilir.
             */
            DeliveredVehicleCount =
                serviceRecords.Count(x =>
                    x.ActualDeliveryDate.HasValue),

            /*
             * Gerçek teslim tarihi bulunmayan kayıtlar
             * hâlâ aktif serviste kabul edilir.
             */
            ActiveServiceCount =
                serviceRecords.Count(x =>
                    !x.ActualDeliveryDate.HasValue),

            TotalUsedPartQuantity =
                operationParts.Sum(x => x.Quantity),

            LowStockPartCount =
                lowStockParts.Count
        };

        /*
         * Aylık gelir raporu.
         */
        report.MonthlyRevenues = serviceRecords
            .GroupBy(x => new
            {
                x.CheckInDate.Year,
                x.CheckInDate.Month
            })
            .Select(group =>
            {
                var groupedOperations = group
                    .SelectMany(x => x.ServiceOperations)
                    .Where(x => !x.IsDeleted)
                    .ToList();

                var groupedParts = groupedOperations
                    .SelectMany(x => x.ServiceOperationParts)
                    .Where(x => !x.IsDeleted)
                    .ToList();

                var laborRevenue = groupedOperations
                    .Sum(x => x.CustomerLaborPrice);

                var partRevenue = groupedParts
                    .Sum(x => x.TotalPrice);

                return new MonthlyRevenueDto
                {
                    Year =
                        group.Key.Year,

                    Month =
                        group.Key.Month,

                    MonthName = CultureInfo
                        .GetCultureInfo("tr-TR")
                        .DateTimeFormat
                        .GetMonthName(group.Key.Month),

                    ServiceCount =
                        group.Count(),

                    TotalRevenue =
                        laborRevenue + partRevenue,

                    LaborRevenue =
                        laborRevenue,

                    PartRevenue =
                        partRevenue
                };
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        /*
         * Usta performansları artık ServiceRecord üzerinden
         * değil ServiceOperation üzerinden hesaplanır.
         */
        report.MechanicPerformances = serviceOperations
            .Where(x => x.Mechanic != null)
            .GroupBy(x => new
            {
                x.MechanicId,
                x.Mechanic.FirstName,
                x.Mechanic.LastName,

                Specialty =
                    x.Mechanic.MechanicSpecialty != null
                        ? x.Mechanic.MechanicSpecialty.Name
                        : x.Mechanic.Specialty
            })
            .Select(group =>
            {
                var mechanicParts = group
                    .SelectMany(x => x.ServiceOperationParts)
                    .Where(x => !x.IsDeleted)
                    .ToList();

                var mechanicServiceRecordIds = group
                    .Select(x => x.ServiceRecordId)
                    .Distinct()
                    .ToList();

                var deliveredServiceCount = group
                    .Where(x =>
                        x.ServiceRecord != null &&
                        x.ServiceRecord.ActualDeliveryDate.HasValue)
                    .Select(x => x.ServiceRecordId)
                    .Distinct()
                    .Count();

                var activeServiceCount = group
                    .Where(x =>
                        x.ServiceRecord != null &&
                        !x.ServiceRecord.ActualDeliveryDate.HasValue)
                    .Select(x => x.ServiceRecordId)
                    .Distinct()
                    .Count();

                var mechanicLaborRevenue = group
                    .Sum(x => x.CustomerLaborPrice);

                var mechanicPartRevenue = mechanicParts
                    .Sum(x => x.TotalPrice);

                return new MechanicPerformanceDto
                {
                    MechanicId =
                        group.Key.MechanicId,

                    MechanicName =
                        group.Key.FirstName + " " +
                        group.Key.LastName,

                    Specialty =
                        group.Key.Specialty ?? "-",

                    /*
                     * Aynı serviste aynı usta birden fazla işlem yaptıysa,
                     * servis kaydı yalnızca bir kez sayılır.
                     */
                    TotalServiceCount =
                        mechanicServiceRecordIds.Count,

                    DeliveredServiceCount =
                        deliveredServiceCount,

                    ActiveServiceCount =
                        activeServiceCount,

                    TotalRevenue =
                        mechanicLaborRevenue +
                        mechanicPartRevenue,

                    /*
                     * Eski DTO adı korunuyor.
                     * Bu alanda müşteriye yazılan işçilik toplamı gösterilir.
                     */
                    TotalLaborCost =
                        mechanicLaborRevenue
                };
            })
            .OrderByDescending(x => x.TotalServiceCount)
            .ThenByDescending(x => x.TotalRevenue)
            .ToList();

        /*
         * En çok kullanılan yedek parçalar.
         */
        report.MostUsedParts = operationParts
            .Where(x => x.SparePart != null)
            .GroupBy(x => new
            {
                x.SparePartId,
                x.SparePart.PartName,
                x.SparePart.PartCode
            })
            .Select(group => new MostUsedPartDto
            {
                SparePartId =
                    group.Key.SparePartId,

                PartName =
                    group.Key.PartName,

                PartCode =
                    group.Key.PartCode,

                TotalQuantity =
                    group.Sum(x => x.Quantity),

                UsageCount =
                    group.Count(),

                TotalRevenue =
                    group.Sum(x => x.TotalPrice)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ThenByDescending(x => x.UsageCount)
            .Take(10)
            .ToList();

        /*
         * Düşük stoklu parçalar.
         */
        report.LowStockParts = lowStockParts
            .Select(x => new LowStockPartDto
            {
                SparePartId =
                    x.SparePartId,

                PartName =
                    x.PartName,

                PartCode =
                    x.PartCode,

                StockQuantity =
                    x.StockQuantity,

                UnitPrice =
                    x.UnitPrice,

                StockStatus =
                    GetStockStatus(x.StockQuantity)
            })
            .ToList();

        return report;
    }

    private static string GetStockStatus(
        int stockQuantity)
    {
        if (stockQuantity <= 0)
        {
            return "Stok Tükendi";
        }

        if (stockQuantity <= 5)
        {
            return "Kritik Stok";
        }

        return "Düşük Stok";
    }
}