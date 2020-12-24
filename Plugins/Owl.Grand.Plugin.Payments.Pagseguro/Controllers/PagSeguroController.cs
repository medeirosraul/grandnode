using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using Owl.Grand.Plugin.Payments.PagSeguro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.PagSeguro.Controllers
{
    public class PagSeguroController: BasePaymentController
    {
        #region fields
        private readonly PagSeguroSettings _settings;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region constructor
        public PagSeguroController(ISettingService settingService, ILocalizationService localizationService, PagSeguroSettings settings)
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
            var model = new PagSeguroSettingsModel() 
            {
                IsSandbox = _settings.IsSandbox,
                PublicKey = _settings.PublicKey
            };

            return View("~/Plugins/Owl.Grand.Plugin.Payments.PagSeguro/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area("Admin")]
        public IActionResult Configure(PagSeguroSettingsModel model)
        {
            _settings.IsSandbox = model.IsSandbox;
            _settings.PublicKey = model.PublicKey;

            _settingService.SaveSetting(_settings);
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
        #endregion
    }
}
