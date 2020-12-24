using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Models;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Grand.Core;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;
using Grand.Services.Logging;
using Logging = Grand.Domain.Logging;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    [PermissionAuthorize(PermissionSystemName.ShippingSettings)]
    public class ShippingMelhorEnvioController : BaseShippingController
    {
        private readonly ShippingMelhorEnvioSettings _shippingMelhorEnvioSettings;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;

        public ShippingMelhorEnvioController(
            ShippingMelhorEnvioSettings shippingMelhorEnvioSettings,
            ISettingService settingService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            ILogger logger)
        {
            _shippingMelhorEnvioSettings = shippingMelhorEnvioSettings;
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _logger = logger;
        }

        public IActionResult Configure()
        {
            var model = new ShippingMelhorEnvioSettingsModel();

            model.IsSandbox = _shippingMelhorEnvioSettings.IsSandbox;
            model.ClientId = _shippingMelhorEnvioSettings.ClientId;
            model.ClientSecret = _shippingMelhorEnvioSettings.ClientSecret;
            model.PostalCodeFrom = _shippingMelhorEnvioSettings.PostalCodeFrom;
            model.RedirectUrl = $"{_webHelper.GetStoreLocation()}Admin/ShippingMelhorEnvio/Callback";

            return View("~/Plugins/Shipping.MelhorEnvio/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Configure(ShippingMelhorEnvioSettingsModel model)
        {
            //save settings
            _shippingMelhorEnvioSettings.IsSandbox = model.IsSandbox;
            _shippingMelhorEnvioSettings.ClientId = model.ClientId;
            _shippingMelhorEnvioSettings.ClientSecret = model.ClientSecret;
            _shippingMelhorEnvioSettings.PostalCodeFrom = model.PostalCodeFrom;

            await _settingService.SaveSetting(_shippingMelhorEnvioSettings);
            await _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [AuthorizeAdmin]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string error)
        {
            // Validate parameters
            if (!string.IsNullOrEmpty(error))
                ErrorNotification($"Houve um erro na solicitação: {error}.");
            else if(string.IsNullOrEmpty(code))
                ErrorNotification("O parâmetro 'code' não foi passado.");

            // Get Access Token
            var melhorEnvioUrl = _shippingMelhorEnvioSettings.IsSandbox ? "https://sandbox.melhorenvio.com.br" : "https://melhorenvio.com.br";

            var client = new RestClient($"{melhorEnvioUrl}/oauth/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddHeader("Accept", "application/json");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_id", _shippingMelhorEnvioSettings.ClientId);
            request.AddParameter("client_secret", _shippingMelhorEnvioSettings.ClientSecret);
            request.AddParameter("redirect_uri", $"{_webHelper.GetStoreLocation(true)}Admin/ShippingMelhorEnvio/Callback");
            request.AddParameter("code", code);

            IRestResponse response = client.Execute(request);

            // Validate response
            if(response.StatusCode != HttpStatusCode.OK)
            {
                var debugMessage = $" - ClientId: {_shippingMelhorEnvioSettings.ClientId}, RedirectUrl = {_webHelper.GetStoreLocation(true)}Admin/ShippingMelhorEnvio/Callback";
                await _logger.InsertLog(Logging.LogLevel.Error,"Melhor Envio: " + response.StatusDescription, response.Content + debugMessage);
                ErrorNotification($"Não foi possível requisitar o Access Token: {response.StatusDescription}.");
                return Configure();
            }

            // Save tokens
            var contentObject = JObject.Parse(response.Content);
            var accessToken = contentObject.Value<string>("access_token");
            var refreshToken = contentObject.Value<string>("refresh_token");
            var expiresIn = contentObject.Value<int>("expires_in");

            _shippingMelhorEnvioSettings.AccessToken = accessToken;
            _shippingMelhorEnvioSettings.RefreshToken = refreshToken;

            // Set expire date to 1 day before
            _shippingMelhorEnvioSettings.AccessTokenExpiration = DateTime.Now.AddSeconds(expiresIn).AddDays(-1);

            await _settingService.SaveSetting(_shippingMelhorEnvioSettings);
            await _settingService.ClearCache();

            SuccessNotification("Access Token atualizado.");
            return Configure();
        }

    }
}
