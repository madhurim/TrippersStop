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
        String ClientId { set; }
        String ClientSecret { set; }
        String Accept { set; }
        String ContentType { set; }


        Task<String> Get(string Method);        
        Task<String> Post(string Method,string Body);
        //Put
        //Delete
       
    }
}