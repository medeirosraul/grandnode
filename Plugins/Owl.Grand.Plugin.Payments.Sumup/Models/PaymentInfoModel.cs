using Grand.Core.ModelBinding;
using Grand.Core.Models;
using Grand.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.Sumup.Models
{
    public class PaymentInfoModel: BaseModel
    {
        public PaymentInfoModel()
        {
        }

        public SumupSettings SumupSettings { get; set; }
        public string CheckoutReference { get; set; }
        public string CheckoutId { get; set; }
    }
}
