using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Response;
using ServiceStack.Text;
using System.Net.Http.Headers;
using TrippismApi.TraveLayer;

namespace TrippismTests
{
    public class LoadTestSample
    {
        public async Task LoadGenerator()
        {
            List<Task<APIResponse>> loadTestTasks = new List<Task<APIResponse>>();

            //let's create n number of requests , in this case 100
            int load = 100;
            //sabre api URL
            string apiURL = "http://url";
            Task<APIResponse> taskToAdd = TestAPI(apiURL);

            for (int n = 1; n <= load; n++)
            {
                loadTestTasks.Add(taskToAdd);
            }

            var responses = await Task.WhenAll<APIResponse>(loadTestTasks);

            foreach( APIResponse response in responses)
            {
                Console.WriteLine(response.Response);
            }
       

            return;
        }
        public async Task<string> testMethod()
        {
            string hello = "Hello World";
            string apiURL = "http://url";

            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            
            var byteArray = await client.GetByteArrayAsync(apiURL);
            await  Task.Delay(100);
            return hello;
        }
        public async Task<APIResponse> TestAPI(string Method)
        {
            SabreAPICaller apiCaller = new SabreAPICaller();
            string token = await apiCaller.GetToken(Method);
            APIResponse resp = await apiCaller.Get(Method);

            return resp;

        } 
    }
}
