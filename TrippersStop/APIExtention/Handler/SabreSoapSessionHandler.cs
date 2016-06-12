using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TrippismApi.TraveLayer;
using TrippismApi.TraveLayer.Hotel.Sabre;

namespace Trippism.APIExtention.Handler
{
    public class SabreSoapSessionHandler : DelegatingHandler
    {
        private readonly Container container;
        public SabreSoapSessionHandler(Container container)
        {
            this.container = container;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.ToString().Contains("api/sabre/hotels"))
            {
                CreateSabreSoapSession(request);
            }         
            return base.SendAsync(request, cancellationToken);
        }

        private void CreateSabreSoapSession(HttpRequestMessage request)
        {
            request.GetDependencyScope();
            var iCacheService = this.container.GetInstance<ICacheService>();
            var iSabreHotelSoapCaller = this.container.GetInstance<ISabreHotelSoapCaller>();
            TrippismApi.ApiHelper.CreateSabreSoapTokenPool(iSabreHotelSoapCaller, iCacheService);
        }
    }
}