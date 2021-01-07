using Grand.Services.Commands.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Widgets.Components
{
    [ViewComponent(Name = "Owl.Grand.Plugin.Shipping.MelhorEnvio.Widgets.ProductEstimate")]
    public class ProductEstimateComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            return View("/Plugins/Shipping.MelhorEnvio/Widgets/Views/EstimateShipping.cshtml");
        }
    }
}
