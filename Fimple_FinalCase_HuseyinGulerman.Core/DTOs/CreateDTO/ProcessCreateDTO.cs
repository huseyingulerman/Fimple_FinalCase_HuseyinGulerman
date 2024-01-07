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
    public class ProcessCreateDTO
    {

        public int Id { get; set; } = 0;
        [JsonIgnore]
        public ProcessStatus ProcessStatus { get; set; }
        public string OutgoingAccountNumber { get; set; }
        public double AmountSent { get; set; }
        public int AccountId { get; set; }
        [JsonIgnore]
        public string? AppUserId { get; set; }
        public bool IsActive { get; set; }
        public ProcessType ProcessType { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
