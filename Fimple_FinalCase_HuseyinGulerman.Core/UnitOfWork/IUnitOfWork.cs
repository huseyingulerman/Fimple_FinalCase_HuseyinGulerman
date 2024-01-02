using Fimple_FinalCase_HuseyinGulerman.Core.Interfaces;
using Fimple_FinalCase_HuseyinGulerman.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
        IGenericRepository<T> GetRepository<T>() where T : class, IEntity;
    }
}
