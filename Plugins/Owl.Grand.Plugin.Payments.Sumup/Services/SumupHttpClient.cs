using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owl.Grand.Plugin.Payments.Sumup.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Payments.Sumup.Services
{
    public class SumupHttpClient
    {
        private readonly SumupSettings _sumupSettings;
        public HttpClient Client { get; set; }

        public SumupHttpClient(SumupSettings sumupSettings)
        {
            _sumupSettings = sumupSettings;

            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://api.sumup.com");
        }

        public async Task<string> GetAccessToken()
        {
            var data = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _sumupSettings.ClientId),
                new KeyValuePair<string, string>("client_secret", _sumupSettings.ClientSecret)
            };

            var content = new FormUrlEncodedContent(data);
            var result = await Client.PostAsync("token", content);
            var response = await result.Content.ReadAsStringAsync();
            var token = JObject.Parse(response)["access_token"].ToString();

            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return token;
        }

        public async Task<string> CreateCheckout(CheckoutRequestModel model)
        {
            await GetAccessToken();

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = await Client.PostAsync("v0.1/checkouts", content);

            if(result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return null;
            }

            var response = await result.Content.ReadAsStringAsync();
            return JObject.Parse(response)["access_token"].ToString();
        }
     }
}
