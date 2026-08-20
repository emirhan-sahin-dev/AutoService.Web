using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class PaymentRepository
    : GenericRepository<Payment>,
      IPaymentRepository
{
    public PaymentRepository(
        AutoServiceContext context)
        : base(context)
    {
    }

    public async Task<List<Payment>>
        GetByServiceRecordIdAsync(
            int serviceRecordId)
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(x =>
                x.ServiceRecordId ==
                    serviceRecordId &&
                !x.IsDeleted)
            .OrderByDescending(x =>
                x.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal>
        GetTotalPaidAmountAsync(
            int serviceRecordId)
    {
        return await _context.Payments
            .Where(x =>
                x.ServiceRecordId ==
                    serviceRecordId &&
                !x.IsDeleted)
            .SumAsync(x =>
                (decimal?)x.Amount)
            ?? 0;
    }
}