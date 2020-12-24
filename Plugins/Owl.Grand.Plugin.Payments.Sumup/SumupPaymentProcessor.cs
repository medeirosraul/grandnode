using Grand.Core;
using Grand.Core.Plugins;
using Grand.Domain.Orders;
using Grand.Services.Payments;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Payments.Sumup
{
    public class SumupPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly IWebHelper _webHelper;

        public SumupPaymentProcessor(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/Sumup/Configure";
        }

        public Task<CancelRecurringPaymentResult> CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CanRePostProcessPayment(Order order)
        {
            throw new System.NotImplementedException();
        }

        public Task<CapturePaymentResult> Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<decimal> GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(0m);
        }

        public async Task<ProcessPaymentRequest> GetPaymentInfo(IFormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest {
                CustomValues = new Dictionary<string, object> {
                    { "CheckoutReference", form["CheckoutReference"] }
                },
                CreditCardNumber = form["CardNumber"],
                CreditCardExpireMonth = int.Parse(form["ExpireMonth"]),
                CreditCardExpireYear = int.Parse(form["ExpireYear"]),
                CreditCardCvv2 = form["CardCode"]
            };

            return await Task.FromResult(paymentInfo);
        }
        public Task<ProcessPaymentResult> ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new System.NotImplementedException();
        }
        public Task PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public void GetPublicViewComponent(out string viewComponentName)
        {
            viewComponentName = "SumupPaymentInfo";
        }

        public Task<bool> HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(false);
        }

        public Task<string> PaymentMethodDescription()
        {
            return Task.FromResult("Pagar com Sumup");
        }



        public Task<ProcessPaymentResult> ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<RefundPaymentResult> Refund(RefundPaymentRequest refundPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SkipPaymentInfo()
        {
            return Task.FromResult(false);
        }

        public Task<bool> SupportCapture()
        {
            return Task.FromResult(false);
        }

        public Task<bool> SupportPartiallyRefund()
        {
            return Task.FromResult(false);
        }

        public Task<bool> SupportRefund()
        {
            return Task.FromResult(false); ;
        }

        public Task<bool> SupportVoid()
        {
            return Task.FromResult(false);
        }

        public Task<IList<string>> ValidatePaymentForm(IFormCollection form)
        {
            throw new System.NotImplementedException();
        }

        public Task<VoidPaymentResult> Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
