using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Seeds
{
    public class AccountSeed /*: IEntityTypeConfiguration<Account>*/
    {
        //public void Configure(EntityTypeBuilder<Account> builder)
        //{
        //    builder.HasData(new Account()
        //    {
        //        Id = 1,
        //        Name ="vadeli",
        //        AccountTypeId = 1,
        //        AppUserId="00c92277-f154-49d4-9765-9be7357a04bd",
        //        IsActive = true,
        //        AccountNumber="10008224",
        //        AccountBalance=100,
        //        OneTimeTransactionLimit=10,
        //        DailyTransactionLimit=20,
        //        Status=Status.Added


        //    });
            //builder.HasData(new Account(new AppUser
            //{
            //    Id="10c92277-f154-49d4-9765-9be7357a04bd",
            //    Email="huseyingulerman.1997@gmail.com",
            //    FirstName="Hüseyin",
            //    LastName="Gülerman",
            //    IsActive=true,
            //    Status=Status.Added,
            //    DateOfBirth=DateTime.UtcNow,
            //    PasswordHash="123456",
            //    Addresses="Üsküdar",
            //    IdentificationNumber="44572008224"
            //}),1,0);
        }
    }

