using Grand.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Models
{
    public class MercadoPagoSettingsModel
    {
        [GrandResourceDisplayName("Sandbox")]
        public bool IsSandbox { get; set; }

        [GrandResourceDisplayName("Public Key")]
        public string PublicKey { get; set; } = string.Empty;

        [GrandResourceDisplayName("Access Token")]
        public string AccessToken { get; set; } = string.Empty;


        [GrandResourceDisplayName("Public Key Sandbox")]
        public string PublicKeySandbox { get; set; } = string.Empty;


        [GrandResourceDisplayName("Access Token Sandbox")]
        public string AccessTokenSandbox { get; set; } = string.Empty;
    }
}
