using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Validations
{
    public class ProcessAutomaticPaymentCreateValidation : AbstractValidator<ProcessAutomaticPaymentCreateDTO>
    {
        public ProcessAutomaticPaymentCreateValidation()
        {
            RuleFor(x => x.OutgoingAccountNumber)
         .Cascade(CascadeMode.StopOnFirstFailure)
   .Must(z => z.Length == 12 && z.All(char.IsDigit))
   .WithMessage("Hesap numarası alanı 12 basamaklı olmalı ve sadece sayı içermelidir.");


            RuleFor(x => x.AmountSent)
           .GreaterThanOrEqualTo(1)
           .WithMessage("Gönderdiğiniz tutar 1 TL'den az olamaz.");

            RuleFor(x => x.AmountSent)
                .Must(x => x.ToString().All(char.IsDigit))
                .WithMessage("Gönderdiğiniz tutar sadece sayı içermelidir.");



            RuleFor(x => x.AccountId)
           .NotEmpty().WithMessage("AccountId alanı boş geçilemez.");


            RuleFor(x => x.PaymentDate)
           .Must(BeInTheFuture).WithMessage("Ödeme tarihi günümüzden ileri bir tarih olmalıdır.")
           .Must(BeWithinOneMonth).WithMessage("Ödeme tarihi en fazla 1 ay sonrasında olmalıdır.");
        }
        private bool BeInTheFuture(DateTime paymentDate)
        {
            return paymentDate > DateTime.Now;
        }

        private bool BeWithinOneMonth(DateTime paymentDate)
        {
            return paymentDate <= DateTime.Now.AddMonths(1);
        }
    }
}
