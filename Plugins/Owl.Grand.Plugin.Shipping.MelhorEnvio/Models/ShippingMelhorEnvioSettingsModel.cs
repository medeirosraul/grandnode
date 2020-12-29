using Grand.Core.ModelBinding;
using Grand.Core.Models;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Models
{
    public class ShippingMelhorEnvioSettingsModel : BaseModel
    {
        [GrandResourceDisplayName("Ambiente Sandbox")]
        public bool IsSandbox { get; set; }

        [GrandResourceDisplayName("Client ID")]
        public string ClientId { get; set; }

        [GrandResourceDisplayName("Client Secret")]
        public string ClientSecret { get; set; }

        [GrandResourceDisplayName("CEP de origem")]
        public string PostalCodeFrom { get; set; }

        [GrandResourceDisplayName("Combinar envio a partir de X")]
        public decimal CombineShippingOver { get; set; }

        [GrandResourceDisplayName("Frete grátis a partir de X")]
        public decimal FreeShippingOver { get; set; }

        [GrandResourceDisplayName("Estados com frete grátis")]
        public string FreeShippingStates { get; set; }

        public string RedirectUrl { get; set; }
    }
}