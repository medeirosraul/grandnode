using Grand.Core;
using Grand.Core.Plugins;
using Grand.Domain.Orders;
using GrandPayments = Grand.Domain.Payments;
using Grand.Services.Payments;
using MercadoPagoCore;
using MercadoPagoCore.Common;
using MercadoPagoCore.DataStructures.Preference;
using MercadoPagoCore.Resources;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grand.Services.Logging;

namespace Owl.Grand.Plugin.Payments.MercadoPago
{
    public class MercadoPagoPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly IWebHelper _webHelper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly MercadoPagoSettings _mercadoPagoSettings;
        private readonly ILogger _logger;

        public MercadoPagoPaymentProcessor(IWebHelper webHelper, MercadoPagoSettings mercadoPagoSettings, IHttpContextAccessor contextAccessor, ILogger logger)
        {
            _webHelper = webHelper;
            _mercadoPagoSettings = mercadoPagoSettings;
            _contextAccessor = contextAccessor;
            _logger = logger;

            if (string.IsNullOrEmpty(MercadoPagoSDK.AccessToken))
                MercadoPagoSDK.AccessToken = _mercadoPagoSettings.AccessToken;
        }

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/MercadoPago/Configure";
        }

        public Task<CancelRecurringPaymentResult> CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CanRePostProcessPayment(Order order)
        {
            if(order.PaymentStatus == GrandPayments.PaymentStatus.Pending)
                return Task.FromResult(true);

            return Task.FromResult(false);
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
            var paymentInfo = new ProcessPaymentRequest();
            return await Task.FromResult(paymentInfo);
        }

        public async Task<ProcessPaymentResult> ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return await Task.FromResult(new ProcessPaymentResult());
        }

        public async Task PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            // Cria um objeto de preferência
            Preference preference = new Preference {
                ExternalReference = postProcessPaymentRequest.Order.Id,
                BackUrls = new BackUrls {
                    Success = $"{_webHelper.GetStoreLocation()}checkout/completed",
                    Pending = $"{_webHelper.GetStoreLocation()}checkout/completed",
                    Failure = $"{_webHelper.GetStoreLocation()}checkout/completed"
                },
                AutoReturn = AutoReturnType.approved,
                //NotificationUrl = $"{_webHelper.GetStoreLocation()}MercadoPago/Notifications"
            };

            // Cria um item na preferência
            preference.Items.Add(
              new Item() {
                  Title = "Corujinha Presentes",
                  Quantity = 1,
                  CurrencyId = CurrencyId.BRL,
                  UnitPrice = postProcessPaymentRequest.Order.OrderTotal
              }
            );

            preference.Save();

            var initPoint = preference.InitPoint;
            _logger.Information("Payment requested in " + initPoint);
            if (_mercadoPagoSettings.IsSandbox)
            {
                initPoint = preference.SandboxInitPoint;
                _logger.Information("Payment requested in SANDBOX " + initPoint);
            }
            _contextAccessor.HttpContext.Response.Redirect(initPoint);
            return;
        }

        public void GetPublicViewComponent(out string viewComponentName)
        {
            viewComponentName = "MercadoPagoPaymentInfo";
        }

        public Task<bool> HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(false);
        }

        public Task<string> PaymentMethodDescription()
        {
            return Task.FromResult("Pagar com MercadoPago");
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

        public async Task<IList<string>> ValidatePaymentForm(IFormCollection form)
        {
            return await Task.FromResult(new List<string>());
        }

        public Task<VoidPaymentResult> Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
