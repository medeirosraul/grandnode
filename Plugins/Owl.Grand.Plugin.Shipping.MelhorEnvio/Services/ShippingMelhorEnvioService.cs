using Grand.Services.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Domain;
using RestSharp;
using Grand.Services.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Logging = Grand.Domain.Logging;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Services
{
    public partial class ShippingMelhorEnvioService
    {
        #region Constants
        private const string MELHOR_ENVIO_URL = "https://melhorenvio.com.br";
        private const string MELHOR_ENVIO_URL_SANDBOX = "https://sandbox.melhorenvio.com.br";
        private const string CEP_URL = "https://viacep.com.br/";
        #endregion

        #region Fields
        private readonly ShippingMelhorEnvioSettings _settings;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public ShippingMelhorEnvioService(ShippingMelhorEnvioSettings settings, ISettingService settingService, ILogger logger)
        {
            _settings = settings;
            _settingService = settingService;
            _logger = logger;
        }

        #endregion

        #region Methods
        public async Task<string> CalcShipping(MelhorEnvioShipment shipment)
        {
            var result = await PostRequest("/api/v2/me/shipment/calculate", shipment);

            _logger.InsertLog(Logging.LogLevel.Information, "Return of shipping calculation for " + shipment.To.PostalCode, result);

            return result;
        }

        public string GetCepInfo(string cep)
        {
            var client = new RestClient($"{CEP_URL}/ws/{cep}/json");
            var request = new RestRequest(Method.GET);

            var response = client.Execute(request);

            // Validate response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response.Content;
        }

        public async Task<string> PostRequest(string endpoint, object content)
        {
            var client = CreateClient(endpoint);
            var request = await CreateRequest(Method.POST);

            request.AddParameter("application/json", JsonConvert.SerializeObject(content), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            // Validate response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response.Content;
        }

        public async Task<RestRequest> CreateRequest(Method method)
        {
            // Validate Access Token
            if (_settings.AccessTokenExpiration < DateTime.Now)
                await RefreshToken();

            var request = new RestRequest(method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.AccessToken}");

            return request;
        }

        public RestClient CreateClient(string endpoint)
        {
            // Create base Url
            var baseUrl = _settings.IsSandbox ? MELHOR_ENVIO_URL_SANDBOX : MELHOR_ENVIO_URL;

            // Fix endpoint
            if (endpoint.First() != '/')
                endpoint = "/" + endpoint;

            var client = new RestClient($"{baseUrl}{endpoint}");

            return client;
        }

        public async Task RefreshToken()
        {
            var client = CreateClient("/oauth/token");
            client.Timeout = -1;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", _settings.RefreshToken);
            request.AddParameter("client_id", _settings.ClientId);
            request.AddParameter("client_secret",_settings.ClientSecret);

            IRestResponse response = client.Execute(request);

            // Validate response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new UnauthorizedAccessException("Acesso a API do Melhor Envio negado.");
            }

            // Save tokens
            var contentObject = JObject.Parse(response.Content);
            var accessToken = contentObject.Value<string>("access_token");
            var refreshToken = contentObject.Value<string>("refresh_token");
            var expiresIn = contentObject.Value<int>("expires_in");

            _settings.AccessToken = accessToken;
            _settings.RefreshToken = refreshToken;

            // Set expire date to 1 day before
            _settings.AccessTokenExpiration = DateTime.Now.AddSeconds(expiresIn).AddDays(-1);

            await _settingService.SaveSetting(_settings);
            await _settingService.ClearCache();
        }
        #endregion
    }

}
