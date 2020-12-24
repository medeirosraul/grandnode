using Grand.Core.ModelBinding;
using Grand.Core.Models;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Models
{
    public class ShippingMelhorEnvioListModel : BaseModel
    {
        [GrandResourceDisplayName("Plugins.Shipping.MelhorEnvio.Fields.LimitMethodsToCreated")]
        public bool LimitMethodsToCreated { get; set; }
    }
}