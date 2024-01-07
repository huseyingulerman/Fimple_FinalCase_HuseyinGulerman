using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Process> Processes { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumber { get; set; }
        public double AccountBalance { get; set; }
        public double DailyTransactionLimit { get; set; }
        public double OneTimeTransactionLimit { get; set; }

        public Account()
        {
            Processes= new HashSet<Process>();
            DailyTransactionLimit = 2000;
            OneTimeTransactionLimit = 1000;
            IsActive = true;
        }
        public Account(string name, string FirstName, string LastName, string appUserId, AccountType accountType = AccountType.DepositAccount, double accountBalance = 0)
        {
            Name = name;
            AppUserId = appUserId;
            AccountType = accountType;
            AccountBalance = accountBalance;

        }

    }
}
