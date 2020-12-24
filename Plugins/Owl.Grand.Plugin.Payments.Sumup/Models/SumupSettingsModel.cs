using Grand.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Owl.Grand.Plugin.Payments.Sumup.Models
{
    public class SumupSettingsModel
    {
        [GrandResourceDisplayName("Sandbox")]
        public bool IsSandbox { get; set; }

        [GrandResourceDisplayName("Client Id")]
        public string ClientId { get; set; } = string.Empty;

        [GrandResourceDisplayName("Client Secret")]
        public string ClientSecret { get; set; } = string.Empty;
    }
}
