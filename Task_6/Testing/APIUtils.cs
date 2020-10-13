using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Test.Framework.Logging;
namespace Task_6.Testing
{
    public static class APIUtils
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static HttpStatusCode StatusCode = 0;
        public static string MediaType = String.Empty;
        public static long? ContentLenght = 0;        

        public static void Initialization(string uri, string mediaType, string userAgent) 
        {
            httpClient.BaseAddress = new Uri(uri);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(mediaType));
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        public static async Task<Queue<T>> GetQueueData<T>(string rout)
        {
            try
            {                
                HttpResponseMessage streamTask = await httpClient.GetAsync(rout);
                StatusCode = streamTask.StatusCode;
                MediaType = streamTask.Content.Headers.ContentType.MediaType;
                ContentLenght = streamTask.Content.Headers.ContentLength;
                return await JsonSerializer.DeserializeAsync<Queue<T>>(await streamTask.Content.ReadAsStreamAsync());
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during getting data{typeof(T)} on rout {rout}.");
                return null;
            }
        }

        public static async Task<T> GetDataItem<T>(string rout, string itemId)
            where T : class
        {
            string fullRout = $"{rout}/{itemId}";
            try 
            {
                HttpResponseMessage streamTask = await httpClient.GetAsync(fullRout);
                StatusCode = streamTask.StatusCode;
                MediaType = streamTask.Content.Headers.ContentType.MediaType;
                ContentLenght = streamTask.Content.Headers.ContentLength;
                return await JsonSerializer.DeserializeAsync<T>(await streamTask.Content.ReadAsStreamAsync());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during getting data{typeof(T)} on rout {fullRout}.");
                return null;
            }
        }

        public static async Task<T> CreatePostDataItem<T>(string rout, T item)
            where T : class
        {
            try 
            { 
                var json = JsonSerializer.Serialize<T>(item);
                var data = new StringContent(json,Encoding.UTF8, "application/json");
                HttpResponseMessage streamTask = await httpClient.PostAsync(rout,data);
                StatusCode = streamTask.StatusCode;
                MediaType = streamTask.Content.Headers.ContentType.MediaType;
                ContentLenght = streamTask.Content.Headers.ContentLength;
                return await JsonSerializer.DeserializeAsync<T>(await streamTask.Content.ReadAsStreamAsync());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during sending post request data{typeof(T)} on rout {rout}.");
                return null;
            }
        }
    }
}
