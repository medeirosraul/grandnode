using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Owl.Grand.Plugin.Misc.Essentials.Models;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grand.Core;
using Grand.Services.Logging;
using Owl.Grand.Plugin.Misc.Essentials.Services;

namespace Owl.Grand.Plugin.Misc.Essentials.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class OwlEssentialsController : BaseShippingController
    {
        private readonly OwlEssentialsSettings _settings;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly MercadoLivreService _mercadoLivreService;

        public OwlEssentialsController(
            OwlEssentialsSettings settings,
            ISettingService settingService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            ILogger logger, MercadoLivreService mercadoLivreService)
        {
            _settings = settings;
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _logger = logger;
            _mercadoLivreService = mercadoLivreService;
        }

        public IActionResult Configure()
        {
            var model = new OwlEssentialsSettingsModel();
            model.IsSandbox = _settings.IsSandbox;

            return View("~/Plugins/Owl.Essentials/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Configure(OwlEssentialsSettingsModel model)
        {
            //save settings
            _settings.IsSandbox = model.IsSandbox;

            await _settingService.SaveSetting(_settings);
            await _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [Area("Admin")]
        [HttpGet("ExportProductsForMercadoLivre")]
        public async Task<IActionResult> GetProductsForMercadoLivre()
        {
            byte[] bytes = await _mercadoLivreService.ExportProductsToMercadoLivre();
            return File(bytes, "text/xls", "products.xlsx");
        }

    }
}
