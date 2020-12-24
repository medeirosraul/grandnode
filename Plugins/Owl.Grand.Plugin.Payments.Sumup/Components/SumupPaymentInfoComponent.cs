using Grand.Core;
using Grand.Domain.Orders;
using Grand.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Owl.Grand.Plugin.Payments.Sumup.Models;
using Owl.Grand.Plugin.Payments.Sumup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Payments.Sumup.Components
{
    [ViewComponent(Name = "SumupPaymentInfo")]
    public class SumupPaymentInfoComponent : ViewComponent
    {
        private readonly SumupSettings _pagSeguroSettings;
        private readonly SumupHttpClient _sumupHttpClient; 
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;

        public SumupPaymentInfoComponent(SumupSettings pagSeguroSettings, SumupHttpClient sumupHttpClient, IOrderTotalCalculationService orderTotalCalculationService, IShoppingCartService shoppingCartService, IStoreContext storeContext, IOrderService orderService)
        {
            _pagSeguroSettings = pagSeguroSettings;
            _sumupHttpClient = sumupHttpClient;
            _orderTotalCalculationService = orderTotalCalculationService;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
#if DEBUG
            _pagSeguroSettings.IsSandbox = true;
#endif
            var cart = _shoppingCartService.GetShoppingCart(_storeContext.CurrentStore.Id, ShoppingCartType.ShoppingCart);
            var orderTotal = (await _orderTotalCalculationService.GetShoppingCartTotal(cart)).shoppingCartTotal;

            var checkout = new CheckoutRequestModel { 
                CheckoutReference = Guid.NewGuid().ToString(),
                Amount = Convert.ToSingle(orderTotal),
                Currency = "BRL",
                Description = "Corujinha Presentes",
                PayToEmail = "medeirosraul@outlook.com"
            };

            var checkoutId = await _sumupHttpClient.CreateCheckout(checkout);
            var model = new PaymentInfoModel {
                CheckoutId = checkoutId,
                CheckoutReference = checkout.CheckoutReference
            };

            return View("~/Plugins/Owl.Grand.Plugin.Payments.Sumup/Views/PaymentInfo.cshtml", model);
        }
    }
}
