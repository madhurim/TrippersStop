﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippismApi.TraveLayer
{
    public interface IAsyncSabreAPICaller : IAPIAsyncCaller
    {
        Task<String> GetToken(string url);
        String SabreTokenKey { get;  }
        String SabreTokenExpireKey { get; }
        String LongTermToken { get; set; }
        String TokenExpireIn { get; set; }

        String Authorization { set; }

        String ClientSecret { set; }
    }
}
