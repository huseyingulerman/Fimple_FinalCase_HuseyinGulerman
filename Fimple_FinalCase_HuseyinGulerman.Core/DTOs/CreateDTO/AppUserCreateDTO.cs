using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO
{
    public class AppUserCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string IdentificationNumber { get; set; }
        public string Addresses { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
