using Grand.Core.ModelBinding;
using Grand.Core.Models;
using Grand.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.PagSeguro.Models
{
    public class PaymentInfoModel: BaseModel
    {
        public PaymentInfoModel()
        {
            ExpireMonths = new List<SelectListItem>();
            ExpireYears = new List<SelectListItem>();
        }

        public PagSeguroSettings PagSeguroSettings { get; set; }

        [GrandResourceDisplayName("Payment.SelectCreditCard")]
        public string CreditCardType { get; set; }

        [GrandResourceDisplayName("Payment.CardholderName")]

        public string CardholderName { get; set; }

        [GrandResourceDisplayName("Payment.CardNumber")]
        public string CardNumber { get; set; }

        [GrandResourceDisplayName("Payment.ExpirationDate")]
        public string ExpireMonth { get; set; }

        [GrandResourceDisplayName("Payment.ExpirationDate")]
        public string ExpireYear { get; set; }

        public IList<SelectListItem> ExpireMonths { get; set; }

        public IList<SelectListItem> ExpireYears { get; set; }

        [GrandResourceDisplayName("Payment.CardCode")]
        public string CardCode { get; set; }

        public string CardCryptography { get; set; }

        public string Errors { get; set; }
    }
}
