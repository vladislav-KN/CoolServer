using CoolServer.Properties;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoolServer.MessageTransfer
{
    public class RequestApi<T,K>
    {
 
        static HttpClient client { 
            get {
 
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:44359/api/");
                return httpClient;
            }  
                
        }  
        public static async Task<Tuple<T, HttpStatusCode, ProblemDetails, string>> Get(string request, string token = "")
        {
            T result = default(T);
            if(!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.GetAsync(request);
            Tuple<T, HttpStatusCode, ProblemDetails, string> tuple = null;
            ProblemDetails problem = new ProblemDetails();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = response.Content.ReadFromJsonAsync<T>(null, CancellationToken.None).Result;
                if (response.Headers.Contains("Token"))
                {
                    token = response.Headers.GetValues("Token").First();
                }
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails, string>(result, response.StatusCode, problem, token);
            }
            else 
            {   
                problem = response.Content.ReadFromJsonAsync<ProblemDetails>(null, CancellationToken.None).Result;
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails, string>(result, HttpStatusCode.OK, problem, token);
            }
            return tuple;

        }
        public static async Task<Tuple<T, HttpStatusCode, ProblemDetails, string>> Put(K data,string request, string token = "")
        {
            T result = default(T);
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.PutAsJsonAsync(
                request, data);
            Tuple<T, HttpStatusCode, ProblemDetails, string> tuple = null;
            ProblemDetails problem = new ProblemDetails();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result = await response.Content.ReadFromJsonAsync<T>(null, CancellationToken.None);
                    if (response.Headers.Contains("Token"))
                    {
                        token = response.Headers.GetValues("Token").First();
                    }
                    tuple = new Tuple<T, HttpStatusCode, ProblemDetails,string>(result, response.StatusCode, problem,token);
                    break;
                case HttpStatusCode.BadRequest:
                    problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(null, CancellationToken.None);
                    tuple = new Tuple<T, HttpStatusCode, ProblemDetails, string>(result, HttpStatusCode.OK, problem, token);
                    break;
            }
            return tuple;

        }
        public static async Task<Tuple<HttpStatusCode, ProblemDetails>> Delete(string request, string token = "")
        {
            Tuple<HttpStatusCode, ProblemDetails> tuple = null;
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.DeleteAsync(request);
            ProblemDetails problem = new ProblemDetails();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    tuple = new Tuple<HttpStatusCode, ProblemDetails>(response.StatusCode, problem);
                    break;
                case HttpStatusCode.BadRequest:
                    problem = response.Content.ReadFromJsonAsync<ProblemDetails>(null, CancellationToken.None).Result;
                    tuple = new Tuple<HttpStatusCode, ProblemDetails>(response.StatusCode, problem);
                    break;
            }
            return tuple;
        }
        public static async Task<Tuple<T, HttpStatusCode, ProblemDetails>> Post(K data, string request, string token = "")
        {
            T result = default(T);
            
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Add("Token", token);
            HttpResponseMessage response = await client.PostAsJsonAsync(request, data);
            Tuple<T, HttpStatusCode, ProblemDetails> tuple = null;
            ProblemDetails problem = new ProblemDetails();
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result = response.Content.ReadFromJsonAsync<T>(null, CancellationToken.None).Result;
                    tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
                    break;
                case HttpStatusCode.BadRequest:
                    problem = response.Content.ReadFromJsonAsync<ProblemDetails>(null,CancellationToken.None).Result;
                    tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, HttpStatusCode.OK, problem);
                    break;
            }
            return tuple;
        }
    }
}
