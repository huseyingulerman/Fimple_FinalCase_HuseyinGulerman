using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Entities
{
    public class Process:BaseEntity
    {
        public string OutgoingAccountNumber { get; set; }
        public double AmountSent { get; set; }
        public ProcessStatus ProcessStatus { get; set; } 
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        [ForeignKey(nameof(AccountId))]
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public ProcessType  ProcessType { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
