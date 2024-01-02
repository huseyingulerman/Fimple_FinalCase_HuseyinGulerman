using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Seeds
{
    public class AppUserSeed : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasData(new AppUser
            {
                Id="00c92277-f154-49d4-9765-9be7357a04bd",
                Email="huseyingulerman.1997@gmail.com",
                FirstName="Hüseyin",
                LastName="Gülerman",
                IsActive=true,
                Status=Status.Added,
                DateOfBirth=DateTime.UtcNow,
                PasswordHash="123456",
                UserName="huseying",
                Addresses="Üsküdar"
                

            });
        }
    }
}
