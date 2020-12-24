using Grand.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.Sumup
{
    public class SumupSettings: ISettings
    {
        public bool IsSandbox { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
