using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Domain
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MelhorEnvioShipmentOptions
    {
        [JsonProperty("receipt")]
        public bool Receipt { get; set; }

        [JsonProperty("own_hand")]
        public bool OwnHand { get; set; }
    }
}
