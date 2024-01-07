using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Repositories
{
    public interface IAccountRepository:IGenericRepository<Account>
    {
        Task<Account> GetAccountByExpAsync(Expression<Func<Account, bool>> exp);
    }
}
