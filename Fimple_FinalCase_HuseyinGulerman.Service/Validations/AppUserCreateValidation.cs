using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Validations
{
    public class AppUserCreateValidation : AbstractValidator<AppUserCreateDTO>
    {
        public AppUserCreateValidation()
        {
            RuleFor(x => x.FirstName)
          .NotEmpty().WithMessage("Ad alanı boş olamaz.")
          .MaximumLength(50).WithMessage("Ad alanı en fazla 50 karakter olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Soyad alanı en fazla 50 karakter olmalıdır.");

            RuleFor(x => x.IdentificationNumber)
                .NotEmpty().WithMessage("Kimlik numarası alanı boş olamaz.")
                .Length(11).WithMessage("Kimlik numarası 11 karakter olmalıdır.")
                .Matches("^[0-9]+$").WithMessage("Kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.Addresses)
                .NotEmpty().WithMessage("Adres alanı boş olamaz.")
                .MaximumLength(255).WithMessage("Adres alanı en fazla 255 karakter olmalıdır.");

            RuleFor(x => x.DateOfBirth)
             .NotNull().WithMessage("Doğum tarihi alanı boş olamaz.")
             .Must(BeAValidDate).WithMessage("Geçerli bir tarih giriniz.")
             .Must(BeAtLeast18YearsOld).WithMessage("Kullanıcının en az 18 yaşında olması gerekmektedir.");
        }
        private bool BeAValidDate(DateTime? date)
        {
            return date.HasValue && date.Value < DateTime.Now;
        }

        private bool BeAtLeast18YearsOld(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue)
            {
                var age = DateTime.Now.Year - dateOfBirth.Value.Year;

                if (DateTime.Now.DayOfYear < dateOfBirth.Value.DayOfYear)
                {
                    age--;
                }

                return age >= 18;
            }

            return false;
        }
    }
}
