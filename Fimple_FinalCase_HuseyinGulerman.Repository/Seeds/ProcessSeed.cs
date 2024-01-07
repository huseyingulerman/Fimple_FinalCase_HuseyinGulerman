using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Seeds
{
    public class ProcessSeed : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            //builder.HasData(new Process
            //{
            //   Id=1,
            //   AppUserId="a1bfbbf6-4dc5-4ddf-b6d5-23499b8b1954",
            //   Status=Status.Added,
            //   AmountSent=5,
            //   AccountId=1,
            //   OutgoingAccountNumber="10004546",
            //   ProcessType=ProcessType.SendingMoney,
            //   TransactionSuccessful=true,
            //   IsActive=true
            //});
        }
    }
}
