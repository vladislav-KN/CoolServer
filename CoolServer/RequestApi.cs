﻿using CoolServer.Properties;
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
 
        static HttpClient cln { 
            get {
 
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:44359/api/");
                return httpClient;
            }  
                
        }  
        public static async Task<Tuple<T, HttpStatusCode, ProblemDetails>> Get(string request, string token = "")
        {
            HttpClient client = cln;
            T result = default(T);
            if(!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(request);
            Tuple<T, HttpStatusCode, ProblemDetails> tuple = null;
            ProblemDetails problem = new ProblemDetails();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<T>(null, CancellationToken.None);
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
            }
            else 
            {   
                problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(null, CancellationToken.None);
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
            }
            return tuple;

        }
        
        public static async Task<Tuple<T, HttpStatusCode, ProblemDetails>> Put(K data,string request, string token = "")
        {
            HttpClient client = cln;
            T result = default(T);
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PutAsJsonAsync(
                request, data);
            Tuple<T, HttpStatusCode, ProblemDetails> tuple = null;
            ProblemDetails problem = new ProblemDetails();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<T>(null, CancellationToken.None);
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
            }
            else
            {
                problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(null, CancellationToken.None);
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
            }
            return tuple;

        }
        public static async Task<Tuple<HttpStatusCode, ProblemDetails>> Delete(string request, string token = "")
        {
            HttpClient client = cln;
            Tuple<HttpStatusCode, ProblemDetails> tuple = null;
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.DeleteAsync(request);
            ProblemDetails problem = new ProblemDetails();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                tuple = new Tuple<HttpStatusCode, ProblemDetails>(response.StatusCode, problem);
            }
            else
            {
                problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(null, CancellationToken.None);
                tuple = new Tuple<HttpStatusCode, ProblemDetails>(response.StatusCode, problem);
            }
 
            return tuple;
        }
        public static async Task<Tuple<T, HttpStatusCode, ProblemDetails>> Post(K data, string request, string token = "")
        {
            HttpClient client = cln;
            T result = default(T);
            
            if (!(string.IsNullOrWhiteSpace(token) && string.IsNullOrEmpty(token)))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsJsonAsync(request, data);
            Tuple<T, HttpStatusCode, ProblemDetails> tuple = null;
            ProblemDetails problem = new ProblemDetails();
            if(response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<T>(null, CancellationToken.None);
                tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
            }        
            else{ 
                    problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(null,CancellationToken.None);
                    tuple = new Tuple<T, HttpStatusCode, ProblemDetails>(result, response.StatusCode, problem);
            }
            return tuple;
        }
    }
}
