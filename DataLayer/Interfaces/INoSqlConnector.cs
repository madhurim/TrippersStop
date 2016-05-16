using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataLayer
{
    interface INoSqlConnector<T>
    {
        T connect();
    }
}
