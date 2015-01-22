using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace TrippersStop.Services
{
    public interface IAPIAsyncCaller
    {
        Uri BaseAPIUri { get; set; }
        Uri TokenUri { get; set; }
        String ClientId { set; }
        String ClientSecret { set; }


        Task<String> Get(string Token, string accessUri);
        Task<String> GetToken();
        Task<String> Post<T>(string Token, string accessUri ,T value);
       
    }
}