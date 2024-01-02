﻿using Fimple_FinalCase_HuseyinGulerman.Core.Repositories;
using Fimple_FinalCase_HuseyinGulerman.Core.UnitOfWork;
using Fimple_FinalCase_HuseyinGulerman.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        IGenericRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new GenericRepository<T>(_context);
        }
    }
}
