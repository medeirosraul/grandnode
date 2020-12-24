using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Domain
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MelhorEnvioShipment
    {
        [JsonProperty("from")]
        public MelhorEnvioShipmentAddress From { get; set; }

        [JsonProperty("to")]
        public MelhorEnvioShipmentAddress To { get; set; }

        [JsonProperty("products")]
        public IEnumerable<MelhorEnvioShipmentProduct> Products { get; set; }

        [JsonProperty("options")]
        public MelhorEnvioShipmentOptions Options { get; set; }

        [JsonProperty("services")]
        public string Services { get; set; }
    }
}
