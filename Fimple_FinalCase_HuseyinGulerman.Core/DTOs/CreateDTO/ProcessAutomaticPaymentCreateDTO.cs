using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO
{
    public class ProcessAutomaticPaymentCreateDTO
    {
        public string OutgoingAccountNumber { get; set; }
        public double AmountSent { get; set; }
        public int AccountId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
