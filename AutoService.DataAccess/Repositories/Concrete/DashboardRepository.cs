using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.DashboardDtos;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class DashboardRepository : IDashboardRepository
{
    private readonly AutoServiceContext _context;

    public DashboardRepository(AutoServiceContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetStatisticsAsync()
    {
        var today = DateTime.Today;

        var firstDayThisMonth =
            new DateTime(today.Year, today.Month, 1);

        var firstDayNextMonth =
            firstDayThisMonth.AddMonths(1);

        var firstDayLastMonth =
            firstDayThisMonth.AddMonths(-1);

        /*
         * Bir servis kaydının müşteriye yansıtılan toplamı:
         *
         * İşlem işçilik toplamı
         * +
         * İşlemlerde kullanılan parça toplamı
         */

        var monthlyRevenue = await _context.ServiceRecords
            .AsNoTracking()
            .GroupBy(x => new
            {
                x.CheckInDate.Year,
                x.CheckInDate.Month
            })
            .OrderBy(x => x.Key.Year)
            .ThenBy(x => x.Key.Month)
            .Select(x => new MonthlyRevenueDto
            {
                Month =
                    $"{x.Key.Month:00}/{x.Key.Year}",

                Revenue = x.Sum(serviceRecord =>
                    serviceRecord.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .Sum(operation =>
                            (decimal?)operation.CustomerLaborPrice) ?? 0)
                    +
                    x.SelectMany(serviceRecord =>
                            serviceRecord.ServiceOperations
                                .Where(operation => !operation.IsDeleted))
                        .SelectMany(operation =>
                            operation.ServiceOperationParts
                                .Where(part => !part.IsDeleted))
                        .Sum(part =>
                            (decimal?)part.TotalPrice) ?? 0
            })
            .ToListAsync();

        /*
         * Son açılan servis kayıtları.
         */

        var recentServices = await _context.ServiceRecords
            .AsNoTracking()
            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Customer)
            .OrderByDescending(x => x.CheckInDate)
            .Take(6)
            .Select(x => new RecentServiceDto
            {
                Plate =
                    x.Vehicle.Plate,

                CustomerName =
                    x.Vehicle.Customer.FullName,

                CheckInDate =
                    x.CheckInDate,

                Status =
                    x.Status.ToString(),

                TotalPrice =
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .Sum(operation =>
                            (decimal?)operation.CustomerLaborPrice) ?? 0
                    +
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .SelectMany(operation =>
                            operation.ServiceOperationParts
                                .Where(part => !part.IsDeleted))
                        .Sum(part =>
                            (decimal?)part.TotalPrice) ?? 0
            })
            .ToListAsync();

        /*
         * Kritik stoklar.
         */

        var criticalStocks = await _context.SpareParts
            .AsNoTracking()
            .Where(x => x.StockQuantity <= 5)
            .OrderBy(x => x.StockQuantity)
            .Select(x => new CriticalStockDto
            {
                PartName =
                    x.PartName,

                StockQuantity =
                    x.StockQuantity
            })
            .ToListAsync();

        /*
         * Tahmini teslim tarihi yaklaşan,
         * fakat müşteriye henüz teslim edilmemiş araçlar.
         */

        var upcomingDeliveries = await _context.ServiceRecords
            .AsNoTracking()
            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Customer)
            .Where(x =>
                x.EstimatedDeliveryDate != null &&
                x.ActualDeliveryDate == null)
            .OrderBy(x => x.EstimatedDeliveryDate)
            .Take(5)
            .Select(x => new UpcomingDeliveryDto
            {
                Plate =
                    x.Vehicle.Plate,

                CustomerName =
                    x.Vehicle.Customer.FullName,

                /*
                 * DTO alanının adı DeliveryDate olarak kaldığı için
                 * tahmini teslim tarihini bu alana aktarıyoruz.
                 */
                DeliveryDate =
                    x.EstimatedDeliveryDate!.Value
            })
            .ToListAsync();

        var newCustomersThisMonth =
            await _context.Customers.CountAsync(x =>
                x.CreatedDate >= firstDayThisMonth &&
                x.CreatedDate < firstDayNextMonth);

        /*
         * Bu ayın toplam cirosu.
         */

        var revenueThisMonth =
            await _context.ServiceRecords
                .Where(x =>
                    x.CheckInDate >= firstDayThisMonth &&
                    x.CheckInDate < firstDayNextMonth)
                .Select(x =>
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .Sum(operation =>
                            (decimal?)operation.CustomerLaborPrice) ?? 0
                    +
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .SelectMany(operation =>
                            operation.ServiceOperationParts
                                .Where(part => !part.IsDeleted))
                        .Sum(part =>
                            (decimal?)part.TotalPrice) ?? 0)
                .SumAsync();

        /*
         * Geçen ayın toplam cirosu.
         */

        var revenueLastMonth =
            await _context.ServiceRecords
                .Where(x =>
                    x.CheckInDate >= firstDayLastMonth &&
                    x.CheckInDate < firstDayThisMonth)
                .Select(x =>
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .Sum(operation =>
                            (decimal?)operation.CustomerLaborPrice) ?? 0
                    +
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .SelectMany(operation =>
                            operation.ServiceOperationParts
                                .Where(part => !part.IsDeleted))
                        .Sum(part =>
                            (decimal?)part.TotalPrice) ?? 0)
                .SumAsync();

        /*
         * Bu ay müşteriye gerçekten teslim edilen servisler.
         */

        var completedServicesThisMonth =
            await _context.ServiceRecords.CountAsync(x =>
                x.ActualDeliveryDate != null &&
                x.ActualDeliveryDate >= firstDayThisMonth &&
                x.ActualDeliveryDate < firstDayNextMonth);

        decimal revenueChangePercentage = 0;

        if (revenueLastMonth > 0)
        {
            revenueChangePercentage =
                ((revenueThisMonth - revenueLastMonth) /
                 revenueLastMonth) * 100;
        }
        else if (revenueThisMonth > 0)
        {
            revenueChangePercentage = 100;
        }

        /*
         * En aktif usta artık ServiceRecord üzerinden değil,
         * ServiceOperation üzerinden hesaplanır.
         */

        var mostActiveMechanicData =
            await _context.ServiceOperations
                .AsNoTracking()
                .Where(x =>
                    !x.IsDeleted &&
                    x.ServiceRecord.CheckInDate >= firstDayThisMonth &&
                    x.ServiceRecord.CheckInDate < firstDayNextMonth)
                .GroupBy(x => new
                {
                    x.MechanicId,
                    x.Mechanic.FirstName,
                    x.Mechanic.LastName
                })
                .Select(x => new
                {
                    MechanicName =
                        x.Key.FirstName + " " +
                        x.Key.LastName,

                    ServiceCount =
                        x.Count()
                })
                .OrderByDescending(x => x.ServiceCount)
                .FirstOrDefaultAsync();

        var mostActiveMechanic =
            mostActiveMechanicData?.MechanicName
            ?? "Henüz veri yok";

        var mostActiveMechanicServiceCount =
            mostActiveMechanicData?.ServiceCount
            ?? 0;

        /*
         * Bu ay en çok servise gelen araç markası.
         */

        var mostPopularBrandData =
            await _context.ServiceRecords
                .AsNoTracking()
                .Where(x =>
                    x.CheckInDate >= firstDayThisMonth &&
                    x.CheckInDate < firstDayNextMonth)
                .GroupBy(x => new
                {
                    x.Vehicle.Model.Brand.BrandId,
                    x.Vehicle.Model.Brand.BrandName
                })
                .Select(x => new
                {
                    BrandName =
                        x.Key.BrandName,

                    ServiceCount =
                        x.Count()
                })
                .OrderByDescending(x => x.ServiceCount)
                .FirstOrDefaultAsync();

        var mostPopularBrand =
            mostPopularBrandData?.BrandName
            ?? "Henüz veri yok";

        var mostPopularBrandServiceCount =
            mostPopularBrandData?.ServiceCount
            ?? 0;

        /*
         * Sistemde kayıtlı araçların marka dağılımı.
         */

        var brandDistributions =
            await _context.Vehicles
                .AsNoTracking()
                .GroupBy(x => new
                {
                    x.Model.Brand.BrandId,
                    x.Model.Brand.BrandName
                })
                .Select(x => new BrandDistributionDto
                {
                    BrandName =
                        x.Key.BrandName,

                    VehicleCount =
                        x.Count()
                })
                .OrderByDescending(x => x.VehicleCount)
                .Take(6)
                .ToListAsync();

        /*
         * Servis kayıtlarının durum dağılımı.
         */

        var serviceStatusCharts =
            await _context.ServiceRecords
                .AsNoTracking()
                .GroupBy(x => x.Status)
                .Select(x => new ServiceStatusChartDto
                {
                    StatusName =
                        x.Key.ToString(),

                    Count =
                        x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

        /*
         * Son aktiviteler.
         */

        var customerActivities =
            await _context.Customers
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .Select(x => new RecentActivityDto
                {
                    Title =
                        "Yeni müşteri eklendi",

                    Description =
                        x.FullName,

                    ActivityDate =
                        x.CreatedDate,

                    ActivityType =
                        "Customer"
                })
                .ToListAsync();

        var vehicleActivities =
            await _context.Vehicles
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .Select(x => new RecentActivityDto
                {
                    Title =
                        "Yeni araç kaydı oluşturuldu",

                    Description =
                        x.Plate,

                    ActivityDate =
                        x.CreatedDate,

                    ActivityType =
                        "Vehicle"
                })
                .ToListAsync();

        var serviceActivities =
            await _context.ServiceRecords
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .Select(x => new RecentActivityDto
                {
                    Title =
                        "Yeni servis kaydı açıldı",

                    Description =
                        x.Vehicle.Plate,

                    ActivityDate =
                        x.CreatedDate,

                    ActivityType =
                        "Service"
                })
                .ToListAsync();

        var sparePartActivities =
            await _context.SpareParts
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .Select(x => new RecentActivityDto
                {
                    Title =
                        "Yeni yedek parça eklendi",

                    Description =
                        x.PartName,

                    ActivityDate =
                        x.CreatedDate,

                    ActivityType =
                        "SparePart"
                })
                .ToListAsync();

        var recentActivities = customerActivities
            .Concat(vehicleActivities)
            .Concat(serviceActivities)
            .Concat(sparePartActivities)
            .OrderByDescending(x => x.ActivityDate)
            .Take(8)
            .ToList();

        /*
         * Genel toplam ciro.
         */

        var totalRevenue =
            await _context.ServiceRecords
                .Select(x =>
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .Sum(operation =>
                            (decimal?)operation.CustomerLaborPrice) ?? 0
                    +
                    x.ServiceOperations
                        .Where(operation => !operation.IsDeleted)
                        .SelectMany(operation =>
                            operation.ServiceOperationParts
                                .Where(part => !part.IsDeleted))
                        .Sum(part =>
                            (decimal?)part.TotalPrice) ?? 0)
                .SumAsync();

        /*
         * Ortalama servis fiyatı.
         */

        var serviceRecordCount =
            await _context.ServiceRecords.CountAsync();

        var averageServicePrice =
            serviceRecordCount > 0
                ? totalRevenue / serviceRecordCount
                : 0;

        return new DashboardDto
        {
            TotalCustomers =
                await _context.Customers.CountAsync(),

            TotalVehicles =
                await _context.Vehicles.CountAsync(),

            TotalMechanics =
                await _context.Mechanics.CountAsync(),

            TotalServiceRecords =
                serviceRecordCount,

            TotalRevenue =
                totalRevenue,

            TotalSpareParts =
                await _context.SpareParts.CountAsync(),

            /*
             * Gerçek teslim tarihi olmayan kayıtlar
             * hâlâ aktif serviste kabul edilir.
             */
            ActiveServiceCount =
                await _context.ServiceRecords.CountAsync(x =>
                    x.ActualDeliveryDate == null),

            AverageServicePrice =
                averageServicePrice,

            WaitingDelivery =
                await _context.ServiceRecords.CountAsync(x =>
                    x.ActualDeliveryDate == null),

            TodayServiceCount =
                await _context.ServiceRecords.CountAsync(x =>
                    x.CheckInDate >= today &&
                    x.CheckInDate < today.AddDays(1)),

            CriticalStockCount =
                await _context.SpareParts.CountAsync(x =>
                    x.StockQuantity <= 5),

            RecentServices =
                recentServices,

            CriticalStocks =
                criticalStocks,

            UpcomingDeliveries =
                upcomingDeliveries,

            MonthlyRevenues =
                monthlyRevenue,

            NewCustomersThisMonth =
                newCustomersThisMonth,

            RevenueThisMonth =
                revenueThisMonth,

            RevenueLastMonth =
                revenueLastMonth,

            CompletedServicesThisMonth =
                completedServicesThisMonth,

            RevenueChangePercentage =
                revenueChangePercentage,

            MostActiveMechanic =
                mostActiveMechanic,

            MostActiveMechanicServiceCount =
                mostActiveMechanicServiceCount,

            MostPopularBrand =
                mostPopularBrand,

            MostPopularBrandServiceCount =
                mostPopularBrandServiceCount,

            BrandDistributions =
                brandDistributions,

            ServiceStatusCharts =
                serviceStatusCharts
        };
    }
}