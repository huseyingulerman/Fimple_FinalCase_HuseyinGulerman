﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimple_FinalCase_HuseyinGulerman.Core.Result.Abstract
{
    public interface IAppResult
    {
        int StatusCode { get; }
        List<string> Errors { get; }
    }
    public interface IAppResult<out T > : IAppResult
    {
        T Data { get; }
    }
}
