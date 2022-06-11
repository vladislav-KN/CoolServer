using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CoolServer.MessageTransfer
{
    public class RequestApi<T,K>
    {
        static HttpClient client = new HttpClient();
        public static async Task<T> Get(string request, string token = "")
        {
            T result = default(T);
            if(!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content.ReadAsStringAsync()
                                                .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });                
                result = JsonConvert.DeserializeObject<T>(jsonString);
            }
            client.DefaultRequestHeaders.Remove("Token");
            return result;

        }
        public static async Task<T> Put(K data,string request, string token = "")
        {
            T result;
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.PutAsJsonAsync(
                request, data);
            response.EnsureSuccessStatusCode();

            string jsonString = response.Content.ReadAsStringAsync()
                                               .Result
                                              .Replace("\\", "")
                                              .Trim(new char[1] { '"' });
            result = JsonConvert.DeserializeObject<T>(jsonString);
            client.DefaultRequestHeaders.Remove("Token");
            return result;

        }
        public static async Task<HttpStatusCode> Delete(string request, string token = "")
        {
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.DeleteAsync(request);
            client.DefaultRequestHeaders.Remove("Token");
            return response.StatusCode;
        }
        public static async Task<Uri> Post(K data, string request, string token = "")
        {
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.PostAsJsonAsync(request, data);
            response.EnsureSuccessStatusCode();
            client.DefaultRequestHeaders.Remove("Token");
            return response.Headers.Location;
        }
    }
}
