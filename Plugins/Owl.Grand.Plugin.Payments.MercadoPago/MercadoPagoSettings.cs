using Grand.Domain.Configuration;
using MercadoPagoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.MercadoPago
{
    public class MercadoPagoSettings: ISettings
    {
        public bool IsSandbox { get; set; }
        public string PublicKey { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;

        public string PublicKeySandbox { get; set; } = string.Empty;
        public string AccessTokenSandbox { get; set; } = string.Empty;

        public MercadoPagoSettings()
        {
            
        }
    }
}
