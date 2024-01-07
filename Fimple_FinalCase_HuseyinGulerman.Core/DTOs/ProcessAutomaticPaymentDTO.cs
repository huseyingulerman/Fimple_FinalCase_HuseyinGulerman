using Fimple_FinalCase_HuseyinGulerman.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs
{
    public class ProcessAutomaticPaymentDTO
    {
        public int Id { get; set; }
        public string OutgoingAccountNumber { get; set; }
        public double AmountSent { get; set; }
        public ProcessStatus ProcessStatus { get; set; }
        public string AppUserId { get; set; }
        public string ProcessTypeName { get; set; }
        public ProcessType ProcessType { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
