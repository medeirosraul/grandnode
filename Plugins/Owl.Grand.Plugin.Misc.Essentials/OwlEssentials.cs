using Grand.Core;
using Grand.Core.Plugins;
using Grand.Services.Common;

namespace Owl.Grand.Plugin.Misc.Essentials
{
    public class OwlEssentials : BasePlugin, IMiscPlugin
    {
        private readonly IWebHelper _webHelper;

        public OwlEssentials(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/OwlEssentials/Configure";
        }
    }
}
