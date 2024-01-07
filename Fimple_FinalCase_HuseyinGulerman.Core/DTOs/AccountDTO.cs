using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AppUserId { get; set; }

        public AccountType AccountType { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountNumber { get; set; }
        public double AccountBalance { get; set; }
        public double DailyTransactionLimit { get; set; }
        public double OneTimeTransactionLimit { get; set; }
    }
}
