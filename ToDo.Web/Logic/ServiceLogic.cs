using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ToDo.Web.Logic
{
    public static class ServiceLogic
    {
        public async static Task<T> GetAsync<T>(string serviceUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync(serviceUrl);

                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return default(T);
            }
        }

        public async static Task<T> PostAsync<T>(string serviceUrl, object item)
        {
            using (HttpClient client = new HttpClient())
            {;
                var result = await client.PostAsync(serviceUrl, new StringContent (JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));

                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return default(T);
            }
        }

        public async static Task<T> PutAsync<T>(string serviceUrl, object item)
        {
            using (HttpClient client = new HttpClient())
            {
                
                var result = await client.PutAsync(serviceUrl, new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"));

                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return default(T);
            }
        }
        public async static Task<T> DeleteAsync<T>(string serviceUrl)
        {
            using (HttpClient client = new HttpClient())
            {

                var result = await client.DeleteAsync(serviceUrl);

                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return default(T);
            }
        }
    }
}