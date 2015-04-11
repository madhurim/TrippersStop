using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrippersStop.TraveLayer
{
    public interface IAsyncSabreAPICaller : IAPIAsyncCaller
    {
        Task<String> GetToken();
    }
}
