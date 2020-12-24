using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Models
{
    public class Notification
    {
        //    {
        //"id": 12345,
        //"live_mode": true,
        //"type": "payment",
        //"date_created": "2015-03-25T10:04:58.396-04:00",
        //"application_id": 123123123,
        //"user_id": 44444,
        //"version": 1,
        //"api_version": "v1",
        //"action": "payment.created",
        //"data": {
        //    "id": "999999999"
        //}
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("live_mode")]
        public bool LiveMode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date_created")]
        public string DateCreated { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public NotificationData Data { get; set; }
    }

    public class NotificationData {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

}
