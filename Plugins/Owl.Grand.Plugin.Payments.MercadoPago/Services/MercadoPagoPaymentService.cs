using Grand.Domain.Orders;
using Grand.Services.Orders;
using MercadoPagoCore;
using MercadoPagoCore.Common;
using MercadoPagoCore.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Payments.MercadoPago.Services
{
    public class MercadoPagoPaymentService
    {
        private readonly MercadoPagoSettings _mercadoPagoSettings;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;

        public MercadoPagoPaymentService(MercadoPagoSettings mercadoPagoSettings, IOrderProcessingService orderProcessingService, IOrderService orderService)
        {
            _mercadoPagoSettings = mercadoPagoSettings;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;

            var accessToken = _mercadoPagoSettings.AccessToken;

            if (_mercadoPagoSettings.IsSandbox)
                accessToken = _mercadoPagoSettings.AccessTokenSandbox;

            if (string.IsNullOrEmpty(MercadoPagoSDK.AccessToken))
                MercadoPagoSDK.AccessToken = accessToken;
        }

        public ICollection<Payment> SearchPayments()
        {
            return Payment.All();
        }

        public async Task UpdatePayment(string id)
        {
            Payment payment = new Payment().Load(id);

            // Validate returned payment
            if (payment == null)
                throw new NullReferenceException("Invalid notification received. Payment not found at MercadoPago.");

            // Get referenced Order
            var order = await _orderService.GetOrderById(payment.ExternalReference);

            if(order == null)
                throw new NullReferenceException("Invalid notification received. Order not found.");

            switch (payment.Status)
            {
                case PaymentStatus.pending:
                    break;
                case PaymentStatus.authorized:
                    await UpdatePaymentAuthorized(order);
                    break;
                case PaymentStatus.approved:
                    await UpdatePaymentApproved(order);
                    break;
                default:
                    break;
            }
        }

        public async Task UpdatePaymentAuthorized(Order order)
        {
            // Mark as authorized
            if (!_orderProcessingService.CanMarkOrderAsAuthorized(order)) return;
            await _orderProcessingService.MarkAsAuthorized(order);

            // Insert Order Note
            await _orderService.InsertOrderNote(new OrderNote {
                Note = "Payment authorized.",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow,
                OrderId = order.Id,
            });
        }

        public async Task UpdatePaymentApproved(Order order)
        {
            // Mark as paid
            if (!await _orderProcessingService.CanMarkOrderAsPaid(order)) return;

            await _orderProcessingService.MarkOrderAsPaid(order);

            // Insert Order Note
            await _orderService.InsertOrderNote(new OrderNote {
                Note = "Payment approved.",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow,
                OrderId = order.Id,
            });
        }
    }
}
