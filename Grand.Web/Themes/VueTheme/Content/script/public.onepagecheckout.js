/*
** one page checkout
*/
var Checkout = {
    loadWaiting: false,
    failureUrl: false,

    init: function (failureUrl) {
        this.loadWaiting = false;
        this.failureUrl = failureUrl;
        //Accordion.disallowAccessToNextSections = true;
    },

    axiosFailure: function () {
        location = Checkout.failureUrl;
    },

    _disableEnableAll: function (element, isDisabled) {
        var descendants = element.querySelectorAll('*');
        descendants.forEach(function(d) {
            if (isDisabled) {
                d.setAttribute('disabled', 'disabled');
            } else {
                d.removeAttribute('disabled');
            }
        });

        if (isDisabled) {
                element.setAttribute('disabled', 'disabled');
            } else {
                element.removeAttribute('disabled');
            }
    },

    setLoadWaiting: function (step, keepDisabled) {
        if (step) {
            if (this.loadWaiting) {
                this.setLoadWaiting(false);
            }
            var container = document.querySelector('#' + step + '-buttons-container');
            container.classList.add('disabled');
            container.style.opacity = '0.5';
            this._disableEnableAll(container, true);
            document.querySelector('#' + step + '-please-wait').style.display = 'block';
        } else {
            if (this.loadWaiting) {
                var container = document.querySelector('#' + this.loadWaiting + '-buttons-container');
                var isDisabled = (keepDisabled ? true : false);
                if (!isDisabled) {
                    container.classList.remove('disabled');
                    container.style.opacity = '1';
                }
                this._disableEnableAll(container, isDisabled);
                document.querySelector('#' + this.loadWaiting + '-please-wait').style.display = 'none';
            }
        }
        this.loadWaiting = step;
    },

    gotoSection: function (section) {
        section = document.querySelector('#button-' + section);
        section.classList.add("allow");
    },

    back: function () {
        if (this.loadWaiting) return;
    },

    setStepResponse: function (response) {
        if (response.data.update_section.name) {
            if (response.data.goto_section == "shipping") {
                var Model = JSON.parse(response.data.update_section.html);
                new Vue({
                    created() {
                        this.updateShipping()
                    },
                    methods: {
                        updateShipping() {
                            vm.ShippingAllowPickUpInStore = Model.AllowPickUpInStore;
                            vm.ShippingAllowPickUpInStore = Model.AllowPickUpInStore,
                            vm.ShippingExistingAddresses = Model.ExistingAddresses,
                            vm.ShippingNewAddress = Model.NewAddress,
                            vm.ShippingNewAddressPreselected = Model.NewAddressPreselected,
                            vm.ShippingPickUpInStore = Model.PickUpInStore,
                            vm.ShippingPickUpInStoreOnly = Model.PickUpInStoreOnly,
                            vm.ShippingPickupPoints = Model.PickupPoints,
                            vm.ShippingWarnings = Model.Warnings,
                            vm.ShippingAddress = true;
                        }
                    }
                })
            }
            if (response.data.goto_section == "shipping_method") {
                var Model = JSON.parse(response.data.update_section.html);
                new Vue({
                    created() {
                        this.updateShippingMethod()
                    },
                    methods: {
                        updateShippingMethod() {
                            vm.NotifyCustomerAboutShippingFromMultipleLocations = Model.NotifyCustomerAboutShippingFromMultipleLocations;
                            vm.ShippingMethods = Model.ShippingMethods,
                            vm.ShippingMethodWarnings = Model.Warnings,
                            vm.ShippingMethod = true;
                        }
                    }
                })
            }
            if (response.data.goto_section == "payment_method") {
                var Model = JSON.parse(response.data.update_section.html);
                new Vue({
                    created() {
                        this.updateShippingMethod()
                    },
                    methods: {
                        updateShippingMethod() {
                            vm.DisplayRewardPoints = Model.DisplayRewardPoints;
                            vm.PaymentMethods = Model.PaymentMethods,
                            vm.RewardPointsAmount = Model.RewardPointsAmount,
                            vm.RewardPointsBalance = Model.RewardPointsBalance;
                            vm.RewardPointsEnoughToPayForOrder = Model.RewardPointsEnoughToPayForOrder;
                            vm.UseRewardPoints = Model.UseRewardPoints;
                            vm.PaymentMethod = true;
                        }
                    }
                })
            }
            if (response.data.goto_section == "payment_info") {
                var Model = JSON.parse(response.data.update_section.html);
                new Vue({
                    created() {
                        this.updatePaymentInfo();
                        this.updateTotals();
                    },
                    methods: {
                        updatePaymentInfo() {
                            vm.DisplayOrderTotals = Model.DisplayOrderTotals;
                            vm.PaymentViewComponentName = Model.PaymentViewComponentName,
                            vm.PaymentInfo = true;
                                axios({
                                    baseURL: '/Common/Component',
                                    method: 'get',
                                    params: { Name: Model.PaymentViewComponentName },
                                    data: null,
                                    headers: {
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    }
                                }).then(response => {
                                    var html = response.data;
                                    document.querySelector('.payment-info .info').innerHTML = html;
                                })
                        },
                        updateTotals() {
                            axios({
                                baseURL: '/Common/Component',
                                method: 'get',
                                params: { Name: 'OrderTotals' },
                                data: null,
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json'
                                }
                            }).then(response => {
                                vm.cart.DisplayTax = response.data.DisplayTax;
                                vm.cart.DisplayTaxRates = response.data.DisplayTaxRates;
                                vm.cart.GenericAttributes = response.data.GenericAttributes;
                                vm.cart.GiftCards = response.data.GiftCards;
                                vm.cart.IsEditable = response.data.IsEditable;
                                vm.cart.OrderTotal = response.data.OrderTotal;
                                vm.cart.OrderTotalDiscount = response.data.OrderTotalDiscount;
                                vm.cart.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee;
                                vm.cart.RedeemedRewardPoints = response.data.RedeemedRewardPoints;
                                vm.cart.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount;
                                vm.cart.RequiresShipping = response.data.RequiresShipping;
                                vm.cart.SelectedShippingMethod = response.data.SelectedShippingMethod;
                                vm.cart.Shipping = response.data.Shipping;
                                vm.cart.SubTotal = response.data.SubTotal;
                                vm.cart.SubTotalDiscount = response.data.SubTotalDiscount;
                                vm.cart.Tax = response.data.Tax;
                                vm.cart.TaxRates = response.data.TaxRates;
                                vm.cart.WillEarnRewardPoints = response.data.WillEarnRewardPoints;
                            });
                        },
                    }
                })
            }
            if (response.data.goto_section == "confirm_order") {
                var Model = JSON.parse(response.data.update_section.html);
                new Vue({
                    created() {
                        this.updateConfirmMethod();
                        this.updateTotals();
                    },
                    methods: {
                        updateConfirmMethod() {
                            vm.MinOrderTotalWarning = Model.MinOrderTotalWarning;
                            vm.TermsOfServiceOnOrderConfirmPage = Model.TermsOfServiceOnOrderConfirmPage,
                            vm.ConfirmWarnings = Model.ConfirmWarnings
                            vm.Confirm = true;
                            setTimeout(function () {
                                var c_back = document.getElementById('back-confirm_order').getAttribute('onclick');
                                document.getElementById('new-back-confirm_order').setAttribute('onclick', c_back);
                            }, 300);
                        },
                        updateTotals() {
                            axios({
                                baseURL: '/Common/Component',
                                method: 'get',
                                params: { Name: 'OrderTotals' },
                                data: null,
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json'
                                }
                            }).then(response => {
                                vm.cart.DisplayTax = response.data.DisplayTax;
                                vm.cart.DisplayTaxRates = response.data.DisplayTaxRates;
                                vm.cart.GenericAttributes = response.data.GenericAttributes;
                                vm.cart.GiftCards = response.data.GiftCards;
                                vm.cart.IsEditable = response.data.IsEditable;
                                vm.cart.OrderTotal = response.data.OrderTotal;
                                vm.cart.OrderTotalDiscount = response.data.OrderTotalDiscount;
                                vm.cart.PaymentMethodAdditionalFee = response.data.PaymentMethodAdditionalFee;
                                vm.cart.RedeemedRewardPoints = response.data.RedeemedRewardPoints;
                                vm.cart.RedeemedRewardPointsAmount = response.data.RedeemedRewardPointsAmount;
                                vm.cart.RequiresShipping = response.data.RequiresShipping;
                                vm.cart.SelectedShippingMethod = response.data.SelectedShippingMethod;
                                vm.cart.Shipping = response.data.Shipping;
                                vm.cart.SubTotal = response.data.SubTotal;
                                vm.cart.SubTotalDiscount = response.data.SubTotalDiscount;
                                vm.cart.Tax = response.data.Tax;
                                vm.cart.TaxRates = response.data.TaxRates;
                                vm.cart.WillEarnRewardPoints = response.data.WillEarnRewardPoints;
                            });
                        },
                    }
                })
            }
            //document.querySelector('#checkout-' + response.data.update_section.name + '-load').innerHTML = response.data.update_section.html;
            document.querySelector('#button-' + response.data.update_section.name).click();
        }
        if (response.data.allow_sections) {
            response.data.allow_sections.forEach(function (e) {
                document.querySelector('#button-' + e).classList.add('allow');
            });
        }
        
        if (document.querySelector("#billing-address-select")) {
            Billing.newAddress(!document.querySelector('#billing-address-select').value);
        }
        if (document.querySelector("#shipping-address-select")) {
            Shipping.newAddress(!document.querySelector('#shipping-address-select').value);
        }

        if (response.data.update_section) {
            Checkout.gotoSection(response.data.update_section.name);
            return true;
        }
        if (response.data.redirect) {
            location.href = response.data.redirect;
            return true;
        }
        return false;
    }
};





var Billing = {
    form: false,
    saveUrl: false,
    disableBillingAddressCheckoutStep: false,

    init: function (form, saveUrl, disableBillingAddressCheckoutStep) {
        this.form = form;
        this.saveUrl = saveUrl;
        this.disableBillingAddressCheckoutStep = disableBillingAddressCheckoutStep;
    },

    newAddress: function (isNew) {
        if (isNew) {
            this.resetSelectedAddress();
            document.querySelector('#billing-new-address-form').style.display = 'block';
        } else {
            document.querySelector('#billing-new-address-form').style.display = 'none';
        }
    },

    resetSelectedAddress: function () {
        var selectElement = document.querySelector('#billing-address-select');
        if (selectElement) {
            selectElement.value = '';
        }
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;

        Checkout.setLoadWaiting('billing');

        var form = document.querySelector(this.form);
        var data = new FormData(form);
        axios({
            url: this.saveUrl,
            method: 'post',
            data: data,
        }).then(function (response) {
            document.querySelector('#back-' + response.data.goto_section).setAttribute('onclick', 'document.querySelector("#button-billing").click()');
            document.querySelector('#opc-' + response.data.update_section.name).parentElement.classList.remove('active');
            this.Billing.nextStep(response);
        }).catch(function (error) {
            error.axiosFailure;
        }).then(function () {
            this.Billing.resetLoadWaiting();
        }); 
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    nextStep: function (response) {
        //ensure that response.wrong_billing_address is set
        //if not set, "true" is the default value
        if (typeof response.data.wrong_billing_address == 'undefined') {
            response.data.wrong_billing_address = false;
        }
        if (Billing.disableBillingAddressCheckoutStep) {
            if (response.data.wrong_billing_address) {
                document.getElementById('button-billing').classList.add('allow');
            } else {
                document.getElementById('button-billing').classList.remove('allow');
                if (document.getElementById('back-shipping_method')) {
                    document.getElementById('back-shipping_method').classList.add('disabled');
                }
            }
        }


        if (response.data.error) {
            if ((typeof response.data.message) == 'string') {
                alert(response.data.message);
            } else {
                alert(response.data.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};



var Shipping = {
    form: false,
    saveUrl: false,

    init: function (form, saveUrl) {
        this.form = form;
        this.saveUrl = saveUrl;
    },

    newAddress: function (isNew) {
        if (isNew) {
            this.resetSelectedAddress();
            document.querySelector('#shipping-new-address-form').style.display = 'block';
        } else {
            document.querySelector('#shipping-new-address-form').style.display = 'none';
        }
    },

    togglePickUpInStore: function (pickupInStoreInput) {
        if (pickupInStoreInput.checked) {
            document.querySelector('#shipping-addresses-form').style.display = 'none';
            document.querySelector('#pickup-points-form').style.display = 'block';
        }
        else {
            document.querySelector('#shipping-addresses-form').style.display = 'block';
            document.querySelector('#pickup-points-form').style.display = 'none';
        }
    },

    resetSelectedAddress: function () {
        var selectElement = document.querySelector('#shipping-address-select');
        if (selectElement) {
            selectElement.value = '';
        }
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;
        Checkout.setLoadWaiting('shipping');

        var form = document.querySelector(this.form);
        var data = new FormData(form);
        axios({
            url: this.saveUrl,
            method: 'post',
            data: data,
        }).then(function (response) {
            document.querySelector('#back-' + response.data.goto_section).setAttribute('onclick', 'document.querySelector("#button-shipping").click()');
            document.querySelector('#opc-' + response.data.update_section.name).parentElement.classList.remove('active');
            this.Shipping.nextStep(response);
        }).catch(function (error) {
            error.axiosFailure;
        }).then(function () {
            this.Billing.resetLoadWaiting();
        }); 
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    nextStep: function (response) {
        if (response.data.error) {
            if ((typeof response.data.message) == 'string') {
                alert(response.data.message);
            } else {
                alert(response.data.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};



var ShippingMethod = {
    form: false,
    saveUrl: false,

    init: function (form, saveUrl) {
        this.form = form;
        this.saveUrl = saveUrl;
    },

    validate: function() {
        var methods = document.getElementsByName('shippingoption');
        if (methods.length==0) {
            alert('Your order cannot be completed at this time as there is no shipping methods available for it. Please make necessary changes in your shipping address.');
            return false;
        }

        for (var i = 0; i< methods.length; i++) {
            if (methods[i].checked) {
                return true;
            }
        }
        alert('Please specify shipping method.');
        return false;
    },
    
    save: function () {
        if (Checkout.loadWaiting != false) return;
        
        if (this.validate()) {
            Checkout.setLoadWaiting('shipping-method');
        
            var form = document.querySelector(this.form);
            var data = new FormData(form);
            axios({
                url: this.saveUrl,
                method: 'post',
                data: data,
            }).then(function (response) {
                document.querySelector('#back-' + response.data.goto_section).setAttribute('onclick', 'document.querySelector("#button-shipping-method").click()');
                document.querySelector('#opc-' + response.data.update_section.name).parentElement.classList.remove('active');
                this.ShippingMethod.nextStep(response);
            }).catch(function (error) {
                error.axiosFailure;
            }).then(function () {
                this.ShippingMethod.resetLoadWaiting();
            }); 
        }
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    nextStep: function (response) {
        if (response.data.error) {
            if ((typeof response.data.message) == 'string') {
                alert(response.data.message);
            } else {
                alert(response.data.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};



var PaymentMethod = {
    form: false,
    saveUrl: false,

    init: function (form, saveUrl) {
        this.form = form;
        this.saveUrl = saveUrl;
    },

    toggleUseRewardPoints: function (useRewardPointsInput) {
        if (useRewardPointsInput.checked) {
            document.querySelector('#payment-method-block').style.display = 'none';
        }
        else {
            document.querySelector('#payment-method-block').style.display = 'block';
        }
    },

    validate: function () {
        var methods = document.getElementsByName('paymentmethod');
        if (methods.length == 0) {
            alert('Your order cannot be completed at this time as there is no payment methods available for it.');
            return false;
        }
        
        for (var i = 0; i < methods.length; i++) {
            if (methods[i].checked) {
                return true;
            }
        }
        alert('Please specify payment method.');
        return false;
    },
    
    save: function () {
        if (Checkout.loadWaiting != false) return;
        
        if (this.validate()) {
            Checkout.setLoadWaiting('payment-method');
            var form = document.querySelector(this.form);
            var data = new FormData(form);
            axios({
                url: this.saveUrl,
                method: 'post',
                data: data,
            }).then(function (response) {
                document.querySelector('#back-' + response.data.goto_section).setAttribute('onclick', 'document.querySelector("#button-payment-method").click()');
                document.querySelector('#opc-' + response.data.update_section.name).parentElement.classList.remove('active');
                this.PaymentMethod.nextStep(response);
            }).catch(function (error) {
                error.axiosFailure;
            }).then(function () {
                this.PaymentMethod.resetLoadWaiting();
            }); 
        }
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    nextStep: function (response) {
        if (response.data.error) {
            if ((typeof response.data.message) == 'string') {
                alert(response.data.message);
            } else {
                alert(response.data.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};



var PaymentInfo = {
    form: false,
    saveUrl: false,

    init: function (form, saveUrl) {
        this.form = form;
        this.saveUrl = saveUrl;
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;
        
        Checkout.setLoadWaiting('payment-info');
        var form = document.querySelector(this.form);
        var data = new FormData(form);
        axios({
            url: this.saveUrl,
            method: 'post',
            data: data,
        }).then(function (response) {
            document.querySelector('#back-' + response.data.goto_section).setAttribute('onclick', 'document.querySelector("#button-payment-info").click()');
            document.querySelector('#opc-' + response.data.update_section.name).parentElement.classList.remove('active');
            this.PaymentInfo.nextStep(response);
        }).catch(function (error) {
            error.axiosFailure;
        }).then(function () {
            this.PaymentInfo.resetLoadWaiting()
        }); 
    },

    resetLoadWaiting: function () {
        Checkout.setLoadWaiting(false);
    },

    nextStep: function (response) {
        if (response.data.error) {
            if ((typeof response.data.message) == 'string') {
                alert(response.data.message);
            } else {
                alert(response.data.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
};



var ConfirmOrder = {
    form: false,
    saveUrl: false,
    isSuccess: false,

    init: function (saveUrl, successUrl) {
        this.saveUrl = saveUrl;
        this.successUrl = successUrl;
    },

    save: function () {
        if (Checkout.loadWaiting != false) return;
        
        // terms of service
        var termOfServiceOk = true;
        if (termOfServiceOk) {
            Checkout.setLoadWaiting('confirm-order');
            axios({
                url: this.saveUrl,
                method: 'post',
            }).then(function (response) {
                this.ConfirmOrder.nextStep(response);
            }).catch(function (error) {
                error.axiosFailure;
            }).then(function () {
                this.ConfirmOrder.resetLoadWaiting()
            }); 
        } else {
            return false;
        }
    },
    
    resetLoadWaiting: function (transport) {
        Checkout.setLoadWaiting(false, ConfirmOrder.isSuccess);
    },

    nextStep: function (response) {
        if (response.data.error) {
            if ((typeof response.data.message) == 'string') {
                alert(response.data.message);
            } else {
                alert(response.data.message.join("\n"));
            }

            return false;
        }
        
        if (response.data.redirect) {
            ConfirmOrder.isSuccess = true;
            location.href = response.data.redirect;
            return;
        }
        if (response.data.success) {
            ConfirmOrder.isSuccess = true;
            window.location = ConfirmOrder.successUrl;
        }

        Checkout.setStepResponse(response);
    }
};  