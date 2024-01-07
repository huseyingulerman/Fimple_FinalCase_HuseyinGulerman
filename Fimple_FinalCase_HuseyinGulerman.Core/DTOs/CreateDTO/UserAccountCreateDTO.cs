using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO
{
    public class UserAccountCreateDTO
    {
        public string Name { get; set; }
        public AccountType AccountType { get; set; }

        public double AccountBalance { get; set; }
    }
}
