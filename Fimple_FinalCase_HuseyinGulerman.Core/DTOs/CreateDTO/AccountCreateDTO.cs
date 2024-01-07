using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO
{
    public class AccountCreateDTO
    {
    
        public string Name { get; set; }
        public string AppUserId { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumber { get; set; }
        public double AccountBalance { get; set; }
     
    }
}

