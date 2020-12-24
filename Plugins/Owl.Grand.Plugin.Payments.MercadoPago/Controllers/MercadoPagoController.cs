using Grand.Domain.Logging;
using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Logging;
using MercadoPagoCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Owl.Grand.Plugin.Payments.MercadoPago.Models;
using Owl.Grand.Plugin.Payments.MercadoPago.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Controllers
{
    public class MercadoPagoController: BasePaymentController
    {
        #region fields
        private readonly MercadoPagoSettings _settings;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly MercadoPagoPaymentService _paymentService;
        private readonly ILogger _logger;

        #endregion

        #region constructor
        public MercadoPagoController(ISettingService settingService, ILocalizationService localizationService, MercadoPagoSettings settings, MercadoPagoPaymentService paymentService, ILogger logger)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _settings = settings;
            _paymentService = paymentService;

            var accessToken = _settings.AccessToken;

            if (_settings.IsSandbox)
                accessToken = _settings.AccessTokenSandbox;

            if (string.IsNullOrEmpty(MercadoPagoSDK.AccessToken))
                MercadoPagoSDK.AccessToken = accessToken;

            _logger = logger;
        }

        #endregion

        #region methods
        [AuthorizeAdmin]
        [Area("Admin")]
        public IActionResult Configure()
        {
            var model = new MercadoPagoSettingsModel() 
            {
                IsSandbox = _settings.IsSandbox,
                PublicKey = _settings.PublicKey,
                AccessToken = _settings.AccessToken,
                PublicKeySandbox = _settings.PublicKeySandbox,
                AccessTokenSandbox = _settings.AccessTokenSandbox
            };

            return View("~/Plugins/Owl.Grand.Plugin.Payments.MercadoPago/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area("Admin")]
        public IActionResult Configure(MercadoPagoSettingsModel model)
        {
            _settings.IsSandbox = model.IsSandbox;
            _settings.PublicKey = model.PublicKey;
            _settings.AccessToken = model.AccessToken;
            _settings.PublicKeySandbox = model.PublicKeySandbox;
            _settings.AccessTokenSandbox = model.AccessTokenSandbox;

            _settingService.SaveSetting(_settings);
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        public async Task<IActionResult> Payments()
        {
            return await Task.FromResult(Json(_paymentService.SearchPayments()));
        }

        [HttpPost]
        public async Task<IActionResult> Notifications([FromBody] Notification notification, [FromQuery(Name ="data.id")] string dataId)
        {
            await _logger.InsertLog(LogLevel.Information, $"MercadoPago Notification received. Query DataId = {dataId}. ", JsonConvert.SerializeObject(notification));

            if (notification == null && string.IsNullOrEmpty(dataId))
                return BadRequest();
            

            string id;
            if (notification?.Data == null || string.IsNullOrEmpty(notification.Data.Id))
                id = dataId;
            else
                id = notification.Data.Id;

            await _paymentService.UpdatePayment(id);

            return await Task.FromResult(Ok());
        }

        public IActionResult Redirect([FromQuery] string redirectUrl)
        {
            ViewData["redirectUrl"] = redirectUrl;
            return View("~/Plugins/Owl.Grand.Plugin.Payments.MercadoPago/Views/Redirect.cshtml");
        }
        #endregion
    }
}
