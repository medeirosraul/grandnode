using Grand.Core.Plugins;
using Grand.Services.Cms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Widgets
{
    public class MelhorEnvioProductEstimateWidget : BasePlugin, IWidgetPlugin
    {
        public void GetPublicViewComponent(string widgetZone, out string viewComponentName)
        {
            viewComponentName = "Owl.Grand.Plugin.Shipping.MelhorEnvio.Widgets.ProductEstimate";
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> {
                "productdetails_overview_bottom"
            };
        }
    }
}
