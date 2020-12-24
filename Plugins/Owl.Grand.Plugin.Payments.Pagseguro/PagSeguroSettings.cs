using Grand.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.PagSeguro
{
    public class PagSeguroSettings: ISettings
    {
        public bool IsSandbox { get; set; }
        public string PublicKey { get; set; } = string.Empty;
    }
}
