 using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Net; 
 using System.Net.Http; 
 using System.Net.Http.Headers; 
 using System.Runtime.CompilerServices; 
 using System.Text; 
 using System.Web.Http; 
 using System.Web.Http.Controllers; 
 using System.Web.Http.Filters; 
 using TrippismApi.TraveLayer; 
 
 
 
 
 
 
 namespace Trippism.APIExtention.Filters 
 { 
     public class TrippismCache : ActionFilterAttribute 
     { 
         int _duration; 
         string _key; 
         string TrippismKey = "Trippism.Sabre."; 
         public TrippismCache(int duration, string key) 
         { 
             if (duration <= 0) 
                 throw new InvalidOperationException("Invalid Duration"); 
             if (string.IsNullOrWhiteSpace(key)) 
                 throw new InvalidOperationException("Invalid Key"); 
             _key = TrippismKey+key; 
             _duration = duration; 
         } 
 
 
         private string GetQueryString(HttpActionContext actionContext) 
         { 
             var query = actionContext.Request.GetQueryNameValuePairs(); 
             int count = query.Count(); 
             if (query != null) 
             { 
                 StringBuilder sb = new StringBuilder();                 
                 foreach (var item in query) 
                 { 
                     count--; 
                     sb.Append(string.Join("_", item.Key, item.Value)); 
                     if (count != 0) 
                         sb.Append("."); 
                 } 
                 return "." + sb.ToString(); 
             } 
             return string.Empty; 
         } 
 
 
         private Action<HttpActionExecutedContext> Callback { set; get; } 
 
         public override void OnActionExecuting(HttpActionContext actionContext) 
         { 
             if (actionContext == null) 
             { 
                 throw new ArgumentNullException("actionExecutedContext"); 
             } 
             var Cache = GlobalConfiguration.Configuration.DependencyResolver 
             .GetService(typeof(ICacheService)) as ICacheService; 
             string cachedValue = Cache != null ? Cache.GetByKey<string>(_key + GetQueryString(actionContext) ) as string : null; 
             if (cachedValue != null) 
             { 
                 actionContext.Response = actionContext.Request.CreateResponse(); 
                 actionContext.Response.Content = new StringContent(cachedValue); 
                 actionContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue(Cache.GetByKey<string>(_key+ GetQueryString(actionContext) + "+ContentType").ToString()); 
                 return; 
             } 
             Callback = (actionExecutedContext) => 
             { 
                 //var output = actionExecutedContext.Result.Content.ReadAsStringAsync().Result; 
                 if (actionExecutedContext.Response != null && actionExecutedContext.Response.StatusCode==HttpStatusCode.OK) 
                 { 
                     var output = actionExecutedContext.Response.Content.ReadAsStringAsync().Result; 
                     if (Cache != null) 
                     { 
                         Cache.Save<string>(_key + GetQueryString(actionContext), output, _duration); 
                         //Cache.Save<string>(_key + "+ContentType", actionExecutedContext.Result.Content.Headers.ContentType.MediaType, DateTimeOffset.UtcNow.AddSeconds(_duration)); 
                         Cache.Save<string>(_key + GetQueryString(actionContext) + "+ContentType", actionExecutedContext.Response.Content.Headers.ContentType.MediaType, _duration); 
                     } 
                 }               
             }; 
         } 
 
 
         public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) 
         { 
             if (actionExecutedContext == null) 
             { 
                 throw new ArgumentNullException("actionExecutedContext"); 
             } 
             Callback(actionExecutedContext); 
         } 
     } 
 } 
