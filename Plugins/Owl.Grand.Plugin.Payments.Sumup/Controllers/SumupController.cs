using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using Owl.Grand.Plugin.Payments.Sumup.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.Sumup.Controllers
{
    public class SumupController: BasePaymentController
    {
        #region fields
        private readonly SumupSettings _settings;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region constructor
        public SumupController(ISettingService settingService, ILocalizationService localizationService, SumupSettings settings)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _settings = settings;
        }

        #endregion

        #region methods
        [AuthorizeAdmin]
        [Area("Admin")]
        public IActionResult Configure()
        {
            var model = new SumupSettingsModel() 
            {
                IsSandbox = _settings.IsSandbox,
                ClientId = _settings.ClientId,
                ClientSecret = _settings.ClientSecret
            };

            return View("~/Plugins/Owl.Grand.Plugin.Payments.Sumup/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area("Admin")]
        public IActionResult Configure(SumupSettingsModel model)
        {
            _settings.IsSandbox = model.IsSandbox;
            _settings.ClientId = model.ClientId;
            _settings.ClientSecret = model.ClientSecret;

            _settingService.SaveSetting(_settings);
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
        #endregion
    }
}
