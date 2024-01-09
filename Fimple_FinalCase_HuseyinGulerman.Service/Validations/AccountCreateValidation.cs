using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Validations
{
    public class AccountCreateValidation:AbstractValidator<AccountCreateDTO>
    {
        public AccountCreateValidation()
        {
            RuleFor(x => x.Name)
   .NotEmpty().WithMessage("Ad alanı boş olamaz.")
   .MaximumLength(50).WithMessage("Ad maksimum 50 karakter olmalıdır.");
            RuleFor(x => x.AccountNumber)
         .Cascade(CascadeMode.StopOnFirstFailure)
   .Must(z => z.Length == 12 && z.All(char.IsDigit))
   .WithMessage("Hesap numarası alanı 12 basamaklı olmalı ve sadece sayı içermelidir.");


            RuleFor(x => x.AccountBalance)
           .GreaterThanOrEqualTo(10)
           .WithMessage("Hesap bakiyesi 10 TL'den az olamaz.");

            RuleFor(x => x.AccountBalance)
                .Must(x => x.ToString().All(char.IsDigit))
                .WithMessage("Hesap bakiyesi sadece sayı içermelidir.");



            RuleFor(x => x.AppUserId)
           .NotEmpty().WithMessage("UserId alanı boş geçilemez.");


            RuleFor(x => x.AccountType).NotEmpty().WithMessage("Hesap tipi alanı boş geçilemez.");
           
        }
    }
}
