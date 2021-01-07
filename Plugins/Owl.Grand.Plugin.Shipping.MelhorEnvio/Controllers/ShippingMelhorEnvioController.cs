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
using Grand.Services.Orders;
using Grand.Domain.Orders;
using System.Linq;
using Grand.Domain.Shipping;
using Grand.Services.Common;
using Grand.Domain.Customers;
using System.Collections.Generic;
using Grand.Services.Shipping;
using MediatR;
using Grand.Domain.Common;
using Grand.Services.Catalog;
using static Grand.Services.Shipping.GetShippingOptionRequest;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Controllers
{
    
    public class ShippingMelhorEnvioController : BaseShippingController
    {
        private readonly ShippingMelhorEnvioSettings _shippingMelhorEnvioSettings;
        private readonly MelhorEnvioShippingComputationMethod _shippingComputationMethod;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IShippingService _shippingService;
        private readonly IMediator _mediator;
        private readonly IProductService _productService;
        public ShippingMelhorEnvioController(
            ShippingMelhorEnvioSettings shippingMelhorEnvioSettings,
            ISettingService settingService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            ILogger logger,
            MelhorEnvioShippingComputationMethod shippingComputationMethod,
            IShoppingCartService shoppingCartService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            IShippingService shippingService, IMediator mediator, 
            IProductService productService)
        {
            _shippingMelhorEnvioSettings = shippingMelhorEnvioSettings;
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _logger = logger;
            _shippingComputationMethod = shippingComputationMethod;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
            _workContext = workContext;
            _genericAttributeService = genericAttributeService;
            _shippingService = shippingService;
            _mediator = mediator;
            _productService = productService;
        }

        [AuthorizeAdmin]
        [Area("Admin")]
        public IActionResult Configure()
        {
            var model = new ShippingMelhorEnvioSettingsModel();

            model.IsSandbox = _shippingMelhorEnvioSettings.IsSandbox;
            model.ClientId = _shippingMelhorEnvioSettings.ClientId;
            model.ClientSecret = _shippingMelhorEnvioSettings.ClientSecret;
            model.PostalCodeFrom = _shippingMelhorEnvioSettings.PostalCodeFrom;
            model.CombineShippingOver = _shippingMelhorEnvioSettings.CombineShippingOver;
            model.FreeShippingOver = _shippingMelhorEnvioSettings.FreeShippingOver;
            model.FreeShippingStates = _shippingMelhorEnvioSettings.FreeShippingStates;
            model.RedirectUrl = $"{_webHelper.GetStoreLocation()}Admin/ShippingMelhorEnvio/Callback";

            return View("~/Plugins/Shipping.MelhorEnvio/Views/Configure.cshtml", model);
        }

        [AuthorizeAdmin]
        [Area("Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Configure(ShippingMelhorEnvioSettingsModel model)
        {
            //save settings
            _shippingMelhorEnvioSettings.IsSandbox = model.IsSandbox;
            _shippingMelhorEnvioSettings.ClientId = model.ClientId;
            _shippingMelhorEnvioSettings.ClientSecret = model.ClientSecret;
            _shippingMelhorEnvioSettings.PostalCodeFrom = model.PostalCodeFrom;
            _shippingMelhorEnvioSettings.CombineShippingOver = model.CombineShippingOver;
            _shippingMelhorEnvioSettings.FreeShippingOver = model.FreeShippingOver;
            _shippingMelhorEnvioSettings.FreeShippingStates = model.FreeShippingStates;

            await _settingService.SaveSetting(_shippingMelhorEnvioSettings);
            await _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [AuthorizeAdmin]
        [Area("Admin")]
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

        public async Task<IActionResult> SaveShippingMethod([FromForm]string name, [FromForm]string cep)
        {
            //validation
            var cart = _shoppingCartService.GetShoppingCart(_storeContext.CurrentStore.Id, ShoppingCartType.ShoppingCart, ShoppingCartType.Auctions);

            var customer = _workContext.CurrentCustomer;
            var store = _storeContext.CurrentStore;

            //clear shipping option XML/Description
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.ShippingOptionAttributeXml, "", store.Id);
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.ShippingOptionAttributeDescription, "", store.Id);

            //find it
            //performance optimization. try cache first
            var shippingOptions = await customer.GetAttribute<List<ShippingOption>>(_genericAttributeService, SystemCustomerAttributeNames.OfferedShippingOptions, store.Id);
            if (shippingOptions == null || shippingOptions.Count == 0)
            {
                var address = new Address { ZipPostalCode = cep };
                //not found? let's load them using shipping service
                shippingOptions = (await _shippingService
                    .GetShippingOptions(customer, cart, address, "Shipping.MelhorEnvio", store))
                    .ShippingOptions
                    .ToList();
            }
            else
            {
                //loaded cached results. let's filter result by a chosen shipping rate computation method
                shippingOptions = shippingOptions.Where(so => so.ShippingRateComputationMethodSystemName.Equals("Shipping.MelhorEnvio", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var shippingOption = shippingOptions
                .Find(so => !String.IsNullOrEmpty(so.Name) && so.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            //save
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.SelectedShippingOption, shippingOption, store.Id);

            return Ok();
        }

        public async Task<IActionResult> EstimateProductShippingValue([FromForm]string productId, [FromForm]int quantity, [FromForm]string cep)
        {
            var request = new GetShippingOptionRequest {
                ZipPostalCodeFrom = _shippingMelhorEnvioSettings.PostalCodeFrom,
                ShippingAddress = new Address {
                    ZipPostalCode = cep
                },
                Items = new List<PackageItem> {
                    new PackageItem(new ShoppingCartItem {
                        ProductId = productId
                    }, quantity)
                }
            };

            var response = await _shippingComputationMethod.GetShippingOptions(request);

            if (response.Errors.Any()) return Content("erro");

            var result = new List<MelhorEnvioShippingOption>();

            foreach(var option in response.ShippingOptions)
            {
                result.Add(new MelhorEnvioShippingOption {
                    ShippingRateComputationMethodSystemName = option.ShippingRateComputationMethodSystemName,
                    Name = option.Name,
                    Description = option.Description,
                    Rate = option.Rate
                });
            }

            return Json(result);
        }
    }
}
