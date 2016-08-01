using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrippismApi.TraveLayer;
using TrippismApi.TraveLayer.Hotel.Sabre;

namespace Trippism.TokenManagement
{
    public interface ITokenManager
    {
        void SetApiToken(IAsyncSabreAPICaller apiCaller, ICacheService cacheService);
        void SetSabreSoapApiToken(ISabreHotelSoapCaller apiCaller, ICacheService cacheService);
        void SaveTokenInCache(IAsyncSabreAPICaller apiCaller, ICacheService cacheService);
        void RefreshApiToken(ICacheService _cacheService, IAsyncSabreAPICaller _apiCaller);

    }
}
