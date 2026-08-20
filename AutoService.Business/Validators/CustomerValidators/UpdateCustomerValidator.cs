using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.CustomerDtos;
using FluentValidation;

namespace AutoService.Business.Validators.CustomerValidators;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.FullName)
    .NotEmpty().WithMessage("Ad Soyad boş bırakılamaz.")
    .MaximumLength(100);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Phone)
            .NotEmpty()
            .Length(10, 15);
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(250);
    }
}
