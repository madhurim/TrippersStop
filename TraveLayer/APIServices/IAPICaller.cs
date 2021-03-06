﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippismApi.TraveLayer
{
    public interface IAPIAsyncCaller
    {
        Uri BaseAPIUri { get; set; }
        String ClientId { set; }
       
        String Accept { set; }
        String ContentType { set; }
        //String Accept-Encoding { set; }
        
        Task<APIResponse> Get(string Method);
        Task<APIResponse> Post(string Method, string Body);

     
        
        //Task<String> Put(string Method, string Body);
        //Task<String> Delete(string Method);
      
       
    }
}