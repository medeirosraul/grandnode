using Newtonsoft.Json;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Domain
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MelhorEnvioShipmentProduct
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("weight")]
        public decimal Weight { get; set; }

        [JsonProperty("insurance_value")]
        public decimal InsuranceValue { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

    }
}
