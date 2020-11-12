function deleteitem(href) {
    axios({
        method: 'post',
        url: href,
    }).then(function (response) {
        var cartall = response.data.cart;
        var newcart = JSON.parse(cartall);
        var flyoutcart = response.data.flyoutshoppingcart;
        var newfly = JSON.parse(flyoutcart);
        vm.flycart = newfly;
        vm.flycartitems = newfly.Items;
        vm.flycartindicator = newfly.TotalProducts;
            vm.ButtonPaymentMethodViewComponentNames = newcart.ButtonPaymentMethodViewComponentNames,
            vm.CheckoutAttributeInfo = newcart.CheckoutAttributeInfo,
            vm.CheckoutAttributes = newcart.CheckoutAttributes,
            vm.DiscountBox = newcart.DiscountBox,
            vm.DisplayTaxShippingInfo = newcart.DisplayTaxShippingInfo,
            vm.GiftCardBox = newcart.GiftCardBox,
            vm.IsAllowOnHold = newcart.IsAllowOnHold,
            vm.IsGuest = newcart.IsGuest,
            vm.Items = newcart.Items,
            vm.MinOrderSubtotalWarning = newcart.MinOrderSubtotalWarning,
            vm.OnePageCheckoutEnabled = newcart.OnePageCheckoutEnabled,
            vm.OrderReviewData = newcart.OrderReviewData,
            vm.ShowCheckoutAsGuestButton = newcart.ShowCheckoutAsGuestButton,
            vm.ShowProductImages = newcart.ShowProductImages,
            vm.ShowSku = newcart.ShowSku,
            vm.TermsOfServiceOnOrderConfirmPage = newcart.TermsOfServiceOnOrderConfirmPage,
            vm.TermsOfServiceOnShoppingCartPage = newcart.TermsOfServiceOnShoppingCartPage,
            vm.TermsOfServicePopup = newcart.TermsOfServicePopup,
            vm.Warnings = newcart.Warnings,
            vm.DiscountBox = newcart.DiscountBox
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    }).then(function () {
        axios({
            baseURL: '/vue/component',
            method: 'get',
            params: { component: 'OrderTotals' },
        }).then(response => (
            vm.DisplayTax = response.data.DisplayTax,
            vm.DisplayTaxRates = response.data.DisplayTaxRates,
            vm.GiftCards = response.data.GiftCards,
            vm.OrderTotal = response.data.OrderTotal,
            vm.OrderTotalDiscount = response.data.OrderTotalDiscount,
            vm.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee,
            vm.RedeemedRewardPoints = response.data.RedeemedRewardPoints,
            vm.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount,
            vm.RequiresShipping = response.data.RequiresShipping,
            vm.SelectedShippingMethod = response.data.SelectedShippingMethod,
            vm.Shipping = response.data.Shipping,
            vm.SubTotal = response.data.SubTotal,
            vm.SubTotalDiscount = response.data.SubTotalDiscount,
            vm.Tax = response.data.Tax,
            vm.TaxRates = response.data.TaxRates,
            vm.WillEarnRewardPoints = response.data.WillEarnRewardPoints
        ))
    });
}

function ChangeShoppingCart(e) {
    var href = e.getAttribute('data-href');
    var form = document.getElementById('shopping-cart-form');
    var data = new FormData(form);
    axios({
        method: 'post',
        data: data,
        url: href,
    }).then(function (response) {
        var cartall = response.data.cart;
        var newcart = JSON.parse(cartall);
        vm.ButtonPaymentMethodViewComponentNames = newcart.ButtonPaymentMethodViewComponentNames,
            vm.CheckoutAttributeInfo = newcart.CheckoutAttributeInfo,
            vm.CheckoutAttributes = newcart.CheckoutAttributes,
            vm.DiscountBox = newcart.DiscountBox,
            vm.DisplayTaxShippingInfo = newcart.DisplayTaxShippingInfo,
            vm.GiftCardBox = newcart.GiftCardBox,
            vm.IsAllowOnHold = newcart.IsAllowOnHold,
            vm.IsGuest = newcart.IsGuest,
            vm.Items = newcart.Items,
            vm.MinOrderSubtotalWarning = newcart.MinOrderSubtotalWarning,
            vm.OnePageCheckoutEnabled = newcart.OnePageCheckoutEnabled,
            vm.OrderReviewData = newcart.OrderReviewData,
            vm.ShowCheckoutAsGuestButton = newcart.ShowCheckoutAsGuestButton,
            vm.ShowProductImages = newcart.ShowProductImages,
            vm.ShowSku = newcart.ShowSku,
            vm.TermsOfServiceOnOrderConfirmPage = newcart.TermsOfServiceOnOrderConfirmPage,
            vm.TermsOfServiceOnShoppingCartPage = newcart.TermsOfServiceOnShoppingCartPage,
            vm.TermsOfServicePopup = newcart.TermsOfServicePopup,
            vm.Warnings = newcart.Warnings,
            vm.DiscountBox = newcart.DiscountBox
        checkoutAttributeChange();
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    }).then(function () {
        axios({
            baseURL: '/vue/component',
            method: 'get',
            params: { component: 'OrderTotals' },
        }).then(response => (
            vm.DisplayTax = response.data.DisplayTax,
            vm.DisplayTaxRates = response.data.DisplayTaxRates,
            vm.GiftCards = response.data.GiftCards,
            vm.OrderTotal = response.data.OrderTotal,
            vm.OrderTotalDiscount = response.data.OrderTotalDiscount,
            vm.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee,
            vm.RedeemedRewardPoints = response.data.RedeemedRewardPoints,
            vm.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount,
            vm.RequiresShipping = response.data.RequiresShipping,
            vm.SelectedShippingMethod = response.data.SelectedShippingMethod,
            vm.Shipping = response.data.Shipping,
            vm.SubTotal = response.data.SubTotal,
            vm.SubTotalDiscount = response.data.SubTotalDiscount,
            vm.Tax = response.data.Tax,
            vm.TaxRates = response.data.TaxRates,
            vm.WillEarnRewardPoints = response.data.WillEarnRewardPoints
        ))
    });
}

function removeGiftCard(e) {
    var href = e.getAttribute('data-href');
    addAntiForgeryToken();
    var bodyFormData = new FormData();
    bodyFormData.append('__RequestVerificationToken', document.querySelector('input[name=__RequestVerificationToken]').value);
    axios({
        method: 'post',
        url: href,
        data: bodyFormData,
        headers: { 'Content-Type': 'multipart/form-data' },
    }).then(function (response) {
        var cartall = response.data.cart;
        var newcart = JSON.parse(cartall);
        vm.ButtonPaymentMethodViewComponentNames = newcart.ButtonPaymentMethodViewComponentNames,
            vm.CheckoutAttributeInfo = newcart.CheckoutAttributeInfo,
            vm.CheckoutAttributes = newcart.CheckoutAttributes,
            vm.DiscountBox = newcart.DiscountBox,
            vm.DisplayTaxShippingInfo = newcart.DisplayTaxShippingInfo,
            vm.GiftCardBox = newcart.GiftCardBox,
            vm.IsAllowOnHold = newcart.IsAllowOnHold,
            vm.IsGuest = newcart.IsGuest,
            vm.Items = newcart.Items,
            vm.MinOrderSubtotalWarning = newcart.MinOrderSubtotalWarning,
            vm.OnePageCheckoutEnabled = newcart.OnePageCheckoutEnabled,
            vm.OrderReviewData = newcart.OrderReviewData,
            vm.ShowCheckoutAsGuestButton = newcart.ShowCheckoutAsGuestButton,
            vm.ShowProductImages = newcart.ShowProductImages,
            vm.ShowSku = newcart.ShowSku,
            vm.TermsOfServiceOnOrderConfirmPage = newcart.TermsOfServiceOnOrderConfirmPage,
            vm.TermsOfServiceOnShoppingCartPage = newcart.TermsOfServiceOnShoppingCartPage,
            vm.TermsOfServicePopup = newcart.TermsOfServicePopup,
            vm.Warnings = newcart.Warnings,
            vm.DiscountBox = newcart.DiscountBox
    }).catch(function (error) {
        alert(error);
    }).then(function () {
        axios({
            baseURL: '/vue/component',
            method: 'get',
            params: { component: 'OrderTotals' },
        }).then(response => (
            vm.DisplayTax = response.data.DisplayTax,
            vm.DisplayTaxRates = response.data.DisplayTaxRates,
            vm.GiftCards = response.data.GiftCards,
            vm.OrderTotal = response.data.OrderTotal,
            vm.OrderTotalDiscount = response.data.OrderTotalDiscount,
            vm.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee,
            vm.RedeemedRewardPoints = response.data.RedeemedRewardPoints,
            vm.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount,
            vm.RequiresShipping = response.data.RequiresShipping,
            vm.SelectedShippingMethod = response.data.SelectedShippingMethod,
            vm.Shipping = response.data.Shipping,
            vm.SubTotal = response.data.SubTotal,
            vm.SubTotalDiscount = response.data.SubTotalDiscount,
            vm.Tax = response.data.Tax,
            vm.TaxRates = response.data.TaxRates,
            vm.WillEarnRewardPoints = response.data.WillEarnRewardPoints
        ))
    });
}