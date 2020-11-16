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
            vm.cart.ButtonPaymentMethodViewComponentNames = newcart.ButtonPaymentMethodViewComponentNames,
            vm.cart.CheckoutAttributeInfo = newcart.CheckoutAttributeInfo,
            vm.cart.CheckoutAttributes = newcart.CheckoutAttributes,
            vm.cart.DiscountBox = newcart.DiscountBox,
            vm.cart.DisplayTaxShippingInfo = newcart.DisplayTaxShippingInfo,
            vm.cart.GiftCardBox = newcart.GiftCardBox,
            vm.cart.IsAllowOnHold = newcart.IsAllowOnHold,
            vm.cart.IsGuest = newcart.IsGuest,
            vm.cart.Items = newcart.Items,
            vm.cart.MinOrderSubtotalWarning = newcart.MinOrderSubtotalWarning,
            vm.cart.OnePageCheckoutEnabled = newcart.OnePageCheckoutEnabled,
            vm.cart.OrderReviewData = newcart.OrderReviewData,
            vm.cart.ShowCheckoutAsGuestButton = newcart.ShowCheckoutAsGuestButton,
            vm.cart.ShowProductImages = newcart.ShowProductImages,
            vm.cart.ShowSku = newcart.ShowSku,
            vm.cart.TermsOfServiceOnOrderConfirmPage = newcart.TermsOfServiceOnOrderConfirmPage,
            vm.cart.TermsOfServiceOnShoppingCartPage = newcart.TermsOfServiceOnShoppingCartPage,
            vm.cart.TermsOfServicePopup = newcart.TermsOfServicePopup,
            vm.cart.Warnings = newcart.Warnings,
            vm.cart.DiscountBox = newcart.DiscountBox
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    }).then(function () {
        axios({
            baseURL: '/vue/component',
            method: 'get',
            params: { component: 'OrderTotals' },
        }).then(response => (
            vm.cart.DisplayTax = response.data.DisplayTax,
            vm.cart.DisplayTaxRates = response.data.DisplayTaxRates,
            vm.cart.GiftCards = response.data.GiftCards,
            vm.cart.OrderTotal = response.data.OrderTotal,
            vm.cart.OrderTotalDiscount = response.data.OrderTotalDiscount,
            vm.cart.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee,
            vm.cart.RedeemedRewardPoints = response.data.RedeemedRewardPoints,
            vm.cart.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount,
            vm.cart.RequiresShipping = response.data.RequiresShipping,
            vm.cart.SelectedShippingMethod = response.data.SelectedShippingMethod,
            vm.cart.Shipping = response.data.Shipping,
            vm.cart.SubTotal = response.data.SubTotal,
            vm.cart.SubTotalDiscount = response.data.SubTotalDiscount,
            vm.cart.Tax = response.data.Tax,
            vm.cart.TaxRates = response.data.TaxRates,
            vm.cart.WillEarnRewardPoints = response.data.WillEarnRewardPoints
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
        vm.cart.ButtonPaymentMethodViewComponentNames = newcart.ButtonPaymentMethodViewComponentNames,
            vm.cart.CheckoutAttributeInfo = newcart.CheckoutAttributeInfo,
            vm.cart.CheckoutAttributes = newcart.CheckoutAttributes,
            vm.cart.DiscountBox = newcart.DiscountBox,
            vm.cart.DisplayTaxShippingInfo = newcart.DisplayTaxShippingInfo,
            vm.cart.GiftCardBox = newcart.GiftCardBox,
            vm.cart.IsAllowOnHold = newcart.IsAllowOnHold,
            vm.cart.IsGuest = newcart.IsGuest,
            vm.cart.Items = newcart.Items,
            vm.cart.MinOrderSubtotalWarning = newcart.MinOrderSubtotalWarning,
            vm.cart.OnePageCheckoutEnabled = newcart.OnePageCheckoutEnabled,
            vm.cart.OrderReviewData = newcart.OrderReviewData,
            vm.cart.ShowCheckoutAsGuestButton = newcart.ShowCheckoutAsGuestButton,
            vm.cart.ShowProductImages = newcart.ShowProductImages,
            vm.cart.ShowSku = newcart.ShowSku,
            vm.cart.TermsOfServiceOnOrderConfirmPage = newcart.TermsOfServiceOnOrderConfirmPage,
            vm.cart.TermsOfServiceOnShoppingCartPage = newcart.TermsOfServiceOnShoppingCartPage,
            vm.cart.TermsOfServicePopup = newcart.TermsOfServicePopup,
            vm.cart.Warnings = newcart.Warnings,
            vm.cart.DiscountBox = newcart.DiscountBox
        checkoutAttributeChange();
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    }).then(function () {
        axios({
            baseURL: '/vue/component',
            method: 'get',
            params: { component: 'OrderTotals' },
        }).then(response => (
            vm.cart.DisplayTax = response.data.DisplayTax,
            vm.cart.DisplayTaxRates = response.data.DisplayTaxRates,
            vm.cart.GiftCards = response.data.GiftCards,
            vm.cart.OrderTotal = response.data.OrderTotal,
            vm.cart.OrderTotalDiscount = response.data.OrderTotalDiscount,
            vm.cart.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee,
            vm.cart.RedeemedRewardPoints = response.data.RedeemedRewardPoints,
            vm.cart.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount,
            vm.cart.RequiresShipping = response.data.RequiresShipping,
            vm.cart.SelectedShippingMethod = response.data.SelectedShippingMethod,
            vm.cart.Shipping = response.data.Shipping,
            vm.cart.SubTotal = response.data.SubTotal,
            vm.cart.SubTotalDiscount = response.data.SubTotalDiscount,
            vm.cart.Tax = response.data.Tax,
            vm.cart.TaxRates = response.data.TaxRates,
            vm.cart.WillEarnRewardPoints = response.data.WillEarnRewardPoints
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
            vm.cart.CheckoutAttributeInfo = newcart.CheckoutAttributeInfo,
            vm.cart.CheckoutAttributes = newcart.CheckoutAttributes,
            vm.cart.DiscountBox = newcart.DiscountBox,
            vm.cart.DisplayTaxShippingInfo = newcart.DisplayTaxShippingInfo,
            vm.cart.GiftCardBox = newcart.GiftCardBox,
            vm.cart.IsAllowOnHold = newcart.IsAllowOnHold,
            vm.cart.IsGuest = newcart.IsGuest,
            vm.cart.Items = newcart.Items,
            vm.cart.MinOrderSubtotalWarning = newcart.MinOrderSubtotalWarning,
            vm.cart.OnePageCheckoutEnabled = newcart.OnePageCheckoutEnabled,
            vm.cart.OrderReviewData = newcart.OrderReviewData,
            vm.cart.ShowCheckoutAsGuestButton = newcart.ShowCheckoutAsGuestButton,
            vm.cart.ShowProductImages = newcart.ShowProductImages,
            vm.cart.ShowSku = newcart.ShowSku,
            vm.cart.TermsOfServiceOnOrderConfirmPage = newcart.TermsOfServiceOnOrderConfirmPage,
            vm.cart.TermsOfServiceOnShoppingCartPage = newcart.TermsOfServiceOnShoppingCartPage,
            vm.cart.TermsOfServicePopup = newcart.TermsOfServicePopup,
            vm.cart.Warnings = newcart.Warnings,
            vm.cart.DiscountBox = newcart.DiscountBox
    }).catch(function (error) {
        alert(error);
    }).then(function () {
        axios({
            baseURL: '/vue/component',
            method: 'get',
            params: { component: 'OrderTotals' },
        }).then(response => (
            vm.cart.DisplayTax = response.data.DisplayTax,
            vm.cart.DisplayTaxRates = response.data.DisplayTaxRates,
            vm.cart.GiftCards = response.data.GiftCards,
            vm.cart.OrderTotal = response.data.OrderTotal,
            vm.cart.OrderTotalDiscount = response.data.OrderTotalDiscount,
            vm.cart.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee,
            vm.cart.RedeemedRewardPoints = response.data.RedeemedRewardPoints,
            vm.cart.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount,
            vm.cart.RequiresShipping = response.data.RequiresShipping,
            vm.cart.SelectedShippingMethod = response.data.SelectedShippingMethod,
            vm.cart.Shipping = response.data.Shipping,
            vm.cart.SubTotal = response.data.SubTotal,
            vm.cart.SubTotalDiscount = response.data.SubTotalDiscount,
            vm.cart.Tax = response.data.Tax,
            vm.cart.TaxRates = response.data.TaxRates,
            vm.cart.WillEarnRewardPoints = response.data.WillEarnRewardPoints
        ))
    });
}