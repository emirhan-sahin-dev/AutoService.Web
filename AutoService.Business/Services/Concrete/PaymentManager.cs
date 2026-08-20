using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.PaymentDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class PaymentManager : IPaymentService
{
    private readonly IPaymentRepository
        _paymentRepository;

    private readonly IServiceRecordRepository
        _serviceRecordRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    public PaymentManager(
        IPaymentRepository paymentRepository,
        IServiceRecordRepository serviceRecordRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository =
            paymentRepository;

        _serviceRecordRepository =
            serviceRecordRepository;

        _unitOfWork =
            unitOfWork;
    }

    public async Task<List<ResultPaymentDto>>
        GetByServiceRecordIdAsync(
            int serviceRecordId)
    {
        var values =
            await _paymentRepository
                .GetByServiceRecordIdAsync(
                    serviceRecordId);

        return values
            .Select(x => new ResultPaymentDto
            {
                PaymentId =
                    x.PaymentId,

                ServiceRecordId =
                    x.ServiceRecordId,

                Amount =
                    x.Amount,

                PaymentMethod =
                    x.PaymentMethod,

                PaymentDate =
                    x.PaymentDate,

                Description =
                    x.Description,

                TransactionReference =
                    x.TransactionReference
            })
            .ToList();
    }

    public async Task<decimal>
        GetTotalPaidAmountAsync(
            int serviceRecordId)
    {
        return await _paymentRepository
            .GetTotalPaidAmountAsync(
                serviceRecordId);
    }

    public async Task AddAsync(
        CreatePaymentDto dto)
    {
        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdWithDetailsAsync(
                    dto.ServiceRecordId);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            throw new Exception(
                "Servis kaydı bulunamadı.");
        }

        if (dto.Amount <= 0)
        {
            throw new Exception(
                "Ödeme tutarı sıfırdan büyük olmalıdır.");
        }

        var activeOperations =
            serviceRecord.ServiceOperations
                .Where(x => !x.IsDeleted)
                .ToList();

        var laborTotal =
            activeOperations
                .Sum(x =>
                    x.CustomerLaborPrice);

        var partsTotal =
            activeOperations
                .SelectMany(x =>
                    x.ServiceOperationParts)
                .Where(x =>
                    !x.IsDeleted)
                .Sum(x =>
                    x.TotalPrice);

        var serviceTotal =
            laborTotal +
            partsTotal;

        if (serviceTotal <= 0)
        {
            throw new Exception(
                "Ödeme alınabilmesi için servis kaydında ücret oluşturan en az bir işlem bulunmalıdır.");
        }

        var totalPaid =
            await _paymentRepository
                .GetTotalPaidAmountAsync(
                    dto.ServiceRecordId);

        var remainingAmount =
            serviceTotal -
            totalPaid;

        if (remainingAmount <= 0)
        {
            throw new Exception(
                "Bu servis kaydının borcu tamamen ödenmiş.");
        }

        if (dto.Amount >
            remainingAmount)
        {
            throw new Exception(
                $"Ödeme tutarı kalan borcu aşamaz. " +
                $"Kalan borç: {remainingAmount:N2} ₺");
        }

        var entity =
            new Payment
            {
                ServiceRecordId =
                    dto.ServiceRecordId,

                Amount =
                    dto.Amount,

                PaymentMethod =
                    dto.PaymentMethod,

                PaymentDate =
                    dto.PaymentDate,

                Description =
                    dto.Description?.Trim(),

                TransactionReference =
                    dto.TransactionReference?.Trim(),

                CreatedDate =
                    DateTime.Now,

                IsDeleted =
                    false
            };

        await _paymentRepository
            .AddAsync(entity);

        await _unitOfWork
            .SaveChangesAsync();
    }

    public async Task DeleteAsync(
        int paymentId)
    {
        var entity =
            await _paymentRepository
                .GetByIdAsync(
                    paymentId);

        if (entity == null ||
            entity.IsDeleted)
        {
            throw new Exception(
                "Ödeme kaydı bulunamadı.");
        }

        await _paymentRepository
            .SoftDeleteAsync(entity);

        await _unitOfWork
            .SaveChangesAsync();
    }
}