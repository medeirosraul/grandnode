using Grand.Core.ModelBinding;
using Grand.Core.Models;
using Grand.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Models
{
    public class PaymentInfoModel: BaseModel
    {
        public PaymentInfoModel()
        {

        }

        public MercadoPagoSettings MercadoPagoSettings { get; set; }
        public string PreferenceId { get; set; }
        public decimal Amount { get; set; }
        public string InitPoint { get; set; }
    }
}
