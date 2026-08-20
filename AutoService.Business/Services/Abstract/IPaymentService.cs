using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.PaymentDtos;

namespace AutoService.Business.Services.Abstract;

public interface IPaymentService
{
    Task<List<ResultPaymentDto>>
        GetByServiceRecordIdAsync(
            int serviceRecordId);

    Task<decimal>
        GetTotalPaidAmountAsync(
            int serviceRecordId);

    Task AddAsync(
        CreatePaymentDto dto);

    Task DeleteAsync(
        int paymentId);
}
