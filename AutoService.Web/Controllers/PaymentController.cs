using AutoService.Business.Services.Abstract;
using AutoService.Dto.PaymentDtos;
using AutoService.Entity.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Web.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService
        _paymentService;

    private readonly IServiceRecordService
        _serviceRecordService;

    public PaymentController(
        IPaymentService paymentService,
        IServiceRecordService serviceRecordService)
    {
        _paymentService =
            paymentService;

        _serviceRecordService =
            serviceRecordService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        int serviceRecordId)
    {
        var model =
            await BuildPageModelAsync(
                serviceRecordId);

        if (model == null)
        {
            TempData["Error"] =
                "Servis kaydı bulunamadı.";

            return RedirectToAction(
                "Index",
                "ServiceRecord");
        }

        LoadPaymentMethods();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
        PaymentPageDto model)
    {
        model.NewPayment.ServiceRecordId =
            model.ServiceRecordId;

        if (!ModelState.IsValid)
        {
            var pageModel =
                await BuildPageModelAsync(
                    model.ServiceRecordId);

            if (pageModel == null)
            {
                TempData["Error"] =
                    "Servis kaydı bulunamadı.";

                return RedirectToAction(
                    "Index",
                    "ServiceRecord");
            }

            pageModel.NewPayment =
                model.NewPayment;

            LoadPaymentMethods();

            return View(
                "Index",
                pageModel);
        }

        try
        {
            await _paymentService
                .AddAsync(
                    model.NewPayment);

            TempData["Success"] =
                "Ödeme başarıyla kaydedildi.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    serviceRecordId =
                        model.ServiceRecordId
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            var pageModel =
                await BuildPageModelAsync(
                    model.ServiceRecordId);

            if (pageModel == null)
            {
                TempData["Error"] =
                    "Servis kaydı bulunamadı.";

                return RedirectToAction(
                    "Index",
                    "ServiceRecord");
            }

            pageModel.NewPayment =
                model.NewPayment;

            LoadPaymentMethods();

            return View(
                "Index",
                pageModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        int paymentId,
        int serviceRecordId)
    {
        try
        {
            await _paymentService
                .DeleteAsync(
                    paymentId);

            TempData["Success"] =
                "Ödeme kaydı silindi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Index),
            new
            {
                serviceRecordId
            });
    }

    private async Task<PaymentPageDto?>
        BuildPageModelAsync(
            int serviceRecordId)
    {
        var serviceRecord =
            await _serviceRecordService
                .GetByIdAsync(
                    serviceRecordId);

        if (serviceRecord == null)
        {
            return null;
        }

        var payments =
            await _paymentService
                .GetByServiceRecordIdAsync(
                    serviceRecordId);

        var totalPaid =
            payments.Sum(x =>
                x.Amount);

        var serviceTotal =
            serviceRecord.TotalPrice;

        var remainingAmount =
            serviceTotal -
            totalPaid;

        if (remainingAmount < 0)
        {
            remainingAmount = 0;
        }

        return new PaymentPageDto
        {
            ServiceRecordId =
                serviceRecord.ServiceRecordId,

            Plate =
                serviceRecord.Plate,

            CustomerName =
                serviceRecord.CustomerName,

            ServiceTotal =
                serviceTotal,

            TotalPaid =
                totalPaid,

            RemainingAmount =
                remainingAmount,

            Payments =
                payments,

            NewPayment =
                new CreatePaymentDto
                {
                    ServiceRecordId =
                        serviceRecordId,

                    PaymentDate =
                        DateTime.Now,

                    Amount =
                        remainingAmount
                }
        };
    }

    private void LoadPaymentMethods()
    {
        ViewBag.PaymentMethods =
            Enum.GetValues<PaymentMethod>()
                .Select(x =>
                    new SelectListItem
                    {
                        Value =
                            ((int)x).ToString(),

                        Text =
                            GetPaymentMethodText(x)
                    })
                .ToList();
    }

    private static string
        GetPaymentMethodText(
            PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.Cash =>
                "Nakit",

            PaymentMethod.CreditCard =>
                "Kredi Kartı",

            PaymentMethod.BankTransfer =>
                "Havale / EFT",

            PaymentMethod.Other =>
                "Diğer",

            _ => method.ToString()
        };
    }
}