using AutoMapper;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Service.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<AppUser, AppUserCreateDTO>().ReverseMap();
            CreateMap<IdentityRole, AppUserRoleCreateDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, AccountCreateDTO>().ReverseMap();
            CreateMap<AccountDTO, AccountCreateDTO>().ReverseMap();
            CreateMap<AccountDTO, AccountUpdateDTO>().ReverseMap();
            CreateMap<Account, AccountUpdateDTO>().ReverseMap();
            CreateMap<AccountCreateDTO, UserAccountCreateDTO>().ReverseMap();
            CreateMap<Process, ProcessCreateDTO>().ReverseMap();
            CreateMap<ProcessDTO, ProcessCreateDTO>().ReverseMap();
            CreateMap<Process, ProcessAutomaticPaymentCreateDTO>().ReverseMap();
            CreateMap<ProcessCreateDTO, ProcessAutomaticPaymentCreateDTO>().ReverseMap();
            CreateMap<ProcessDTO, ProcessAutomaticPaymentCreateDTO>().ReverseMap();



         
            CreateMap<Process, ProcessDTO>().ReverseMap()
          .ForMember(dest => dest.ProcessType, opt => opt.MapFrom(src =>src.ProcessTypeName.ToString()));

        }
    }
}
