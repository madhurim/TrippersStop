using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippersStop.TraveLayer
{
    public interface IDBService
    {
        bool Save<T>(string key, T keyData);

        bool Save<T>(string key, T keyData,double expireInMin);
        T GetByKey<T>(string key);
    }
}
