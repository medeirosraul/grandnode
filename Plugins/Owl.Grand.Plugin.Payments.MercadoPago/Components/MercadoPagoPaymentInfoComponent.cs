using Grand.Core;
using Grand.Domain.Orders;
using Grand.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Owl.Grand.Plugin.Payments.MercadoPago.Models;
using Owl.Grand.Plugin.Payments.MercadoPago.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MercadoPagoCore;
using MercadoPagoCore.Resources;
using MercadoPagoCore.Common;
using MercadoPagoCore.DataStructures.Preference;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Components
{
    [ViewComponent(Name = "MercadoPagoPaymentInfo")]
    public class MercadoPagoPaymentInfoComponent : ViewComponent
    {
        public MercadoPagoPaymentInfoComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("~/Plugins/Owl.Grand.Plugin.Payments.MercadoPago/Views/PaymentInfo.cshtml"));
        }
    }
}
