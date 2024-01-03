using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Seeds
{
    public class AppRoleSeed : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole
            {

                Id="5b64a78a-4442-4e92-a019-4f7bfb29ac52",
                Name="admin",
                NormalizedName="ADMIN"
            },

            new IdentityRole
            {

                Id="6b64a78a-4442-4e92-a019-4f7bfb29ac52",
                Name="user",
                NormalizedName="USER"
            },
            new IdentityRole
            {

                Id="7b64a78a-4442-4e92-a019-4f7bfb29ac52",
                Name="auditor",
                NormalizedName="AUDITOR"
            }
            );
        }
    }
}
