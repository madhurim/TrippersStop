﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippismApi.TraveLayer
{
    public interface ICacheService
    {
        bool Save<T>(string key, T keyData);

        bool Save<T>(string key, T keyData,double expireInMin);
        T GetByKey<T>(string key);

        bool Expire(string key);
        bool IsConnected();
    }
}
