using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using Fimple_FinalCase_HuseyinGulerman.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public string Addresses { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
    }
}
