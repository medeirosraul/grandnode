using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Models
{
    public class CheckoutRequestModel
    {
        [JsonProperty("checkout_reference")]
        public string CheckoutReference { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("pay_to_email")]
        public string PayToEmail { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }
    }
}
