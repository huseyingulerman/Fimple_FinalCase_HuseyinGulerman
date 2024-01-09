using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Validations
{
    public class ProcessCreateValidation : AbstractValidator<ProcessCreateDTO>
    {
        public ProcessCreateValidation()
        {
            RuleFor(x => x.OutgoingAccountNumber)
          .Cascade(CascadeMode.StopOnFirstFailure)
    .Must(z => z.Length == 12 && z.All(char.IsDigit))
    .WithMessage("Giden hesap numarası alanı 12 basamaklı olmalı ve sadece sayı içermelidir.");
            RuleFor(x => x.AmountSent)
  .NotEmpty().WithMessage("Tutar boş geçilemez.");

            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Hesap alanı boş olamaz.");

            RuleFor(x => x.ProcessType)
           .NotEmpty().WithMessage("İşlem tipi boş olamaz.");
          
   
        }
    }
}
