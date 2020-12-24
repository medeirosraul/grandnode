using Grand.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Owl.Grand.Plugin.Payments.PagSeguro.Models
{
    public class PagSeguroSettingsModel
    {
        [GrandResourceDisplayName("Ambiente sandbox")]
        public bool IsSandbox { get; set; }

        [GrandResourceDisplayName("Chave Pública")]

        public string PublicKey { get; set; } = string.Empty;
    }
}
