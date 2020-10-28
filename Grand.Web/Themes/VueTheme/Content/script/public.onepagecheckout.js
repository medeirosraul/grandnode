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
        var m_title = document.getElementById('confirm-data-modal').getAttribute('data-title');
        var m_body = document.getElementById('confirm-data-modal').getAttribute('data-body');
        var m_terms = document.getElementById('confirm-data-modal').getAttribute('data-terms');
        var m_link = document.getElementById('confirm-data-modal').getAttribute('data-link');
        var m_linkname = document.getElementById('confirm-data-modal').getAttribute('data-linkname');
        var c_back = document.getElementById('back-confirm_order').getAttribute('onclick');
        new Vue({
            el: '.order-summary-content',
            methods: {
                    showMsgBoxOne() {
                        const h = this.$createElement

                    const titleVNode = h('div', { domProps: { innerHTML: '<h5>' + m_title + '</h5>' } })
                    const messageVNode = h('div', { domProps: { innerHTML: '' + m_terms + ' <a href="' + m_link + '" target="popup" onclick="window.open(' + m_link + ')">' + m_linkname + '</a>' } })

                        this.$bvModal.msgBoxConfirm([messageVNode], {
                            title: [titleVNode],
                            centered: true,
                            size: 'md',
                            okVariant: 'info',
                            okTitle: 'Ok',
                            cancelTitle: 'Cancel',
                            cancelVariant: 'danger',
                            footerClass: 'p-2',
                            hideHeaderClose: false,
                        })
                            .then(value => {
                                this.boxOne = value
                                if (value == true) {
                                    ConfirmOrder.save()
                                }
                            })
                            .catch(err => {

                            })
                    },
            }
        });    
        new Vue({
            el: '.section.new-shipping-address',
            methods: {
                showMsgBoxOne() {
                    const h = this.$createElement

                    const titleVNode = h('div', { domProps: { innerHTML: '<h5>' + m_title + '</h5>' } })
                    const messageVNode = h('div', { domProps: { innerHTML: '' + m_terms + ' <a href="' + m_link + '" target="popup" onclick="window.open(' + m_link + ')">' + m_linkname + '</a>' } })

                    this.$bvModal.msgBoxConfirm([messageVNode], {
                        title: [titleVNode],
                        centered: true,
                        size: 'md',
                        okVariant: 'info',
                        okTitle: 'Ok',
                        cancelTitle: 'Cancel',
                        cancelVariant: 'danger',
                        footerClass: 'p-2',
                        hideHeaderClose: false,
                    })
                        .then(value => {
                            this.boxOne = value
                            if (value == true) {
                                ConfirmOrder.save()
                            }
                        })
                        .catch(err => {

                        })
                },
            }
        });
        document.getElementById('new-back-confirm_order').setAttribute('onclick', c_back);
    },

    back: function () {
        if (this.loadWaiting) return;
    },

    setStepResponse: function (response) {
        if (response.data.update_section.name) {
            document.querySelector('#checkout-' + response.data.update_section.name + '-load').innerHTML = response.data.update_section.html;
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
            response.wrong_billing_address = false;
        }
        if (Billing.disableBillingAddressCheckoutStep) {
            console.log(response.data)
            if (response.data.wrong_billing_address) {
                Accordion.showSection('#opc-billing');
            } else {
                Accordion.hideSection('#opc-billing');
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