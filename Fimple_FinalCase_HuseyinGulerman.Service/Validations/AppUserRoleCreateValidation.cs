using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Validations
{
    public class AppUserRoleCreateValidation : AbstractValidator<AppUserRoleCreateDTO>
    {
        public AppUserRoleCreateValidation()
        {
            RuleFor(x => x.Name)
         .NotEmpty().WithMessage("Ad alanı boş olamaz.")
         .MaximumLength(10).WithMessage("Ad alanı en fazla 10 karakter olmalıdır.");
        }
    }
}
