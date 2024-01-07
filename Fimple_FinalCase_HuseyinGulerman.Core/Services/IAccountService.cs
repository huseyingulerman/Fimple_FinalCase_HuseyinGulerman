using Fimple_FinalCase_HuseyinGulerman.Core.DTOs;
using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Services
{
    public interface IAccountService : IService<Account, AccountCreateDTO, AccountDTO>
    {
        Task<IAppResult<AccountDTO>> AddAccountAsync(AccountCreateDTO accountCreateDTO,string identificationNumber);
        Task<Account> GetAccountByExpAsync(Expression<Func<Account, bool>> exp);
    }
}
