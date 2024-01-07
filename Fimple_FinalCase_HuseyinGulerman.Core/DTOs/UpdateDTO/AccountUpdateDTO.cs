using Fimple_FinalCase_HuseyinGulerman.Core.DTOs.CreateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.DTOs.UpdateDTO
{
    public class AccountUpdateDTO:AccountCreateDTO
    {
        public int Id { get; set; }
    }
}
