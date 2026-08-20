using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.CustomerDtos;
using FluentValidation;

namespace AutoService.Business.Validators.CustomerValidators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad Soyad boş bırakılamaz.")
            .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefon boş bırakılamaz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adres boş bırakılamaz.");
    }
}
