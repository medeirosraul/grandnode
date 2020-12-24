
using Grand.Domain.Configuration;
using System;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio
{
    public class ShippingMelhorEnvioSettings : ISettings
    {
        public bool IsSandbox { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string PostalCodeFrom { get; set; }
    }
}