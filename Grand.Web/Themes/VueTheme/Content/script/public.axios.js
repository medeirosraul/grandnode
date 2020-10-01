
/*
** axios cart implementation
*/
var AxiosCart = {
    loadWaiting: false,
    topcartselector: '',
    topwishlistselector: '',
    flyoutcartselector: '',

    init: function (topcartselector, topwishlistselector, flyoutcartselector) {
        this.loadWaiting = false;
        this.topcartselector = topcartselector;
        this.topwishlistselector = topwishlistselector;
        this.flyoutcartselector = flyoutcartselector;
    },

    setLoadWaiting: function (display) {
        //displayAxiosLoading(display);
        this.loadWaiting = display;
    },

    quickview_product: function (quickviewurl) {
        this.setLoadWaiting(true);
        axios({
            url: quickviewurl,
            success: this.success_process,
            complete: this.resetLoadWaiting,
            error: this.axiosFailure
        })
    },

    //add a product to the cart/wishlist from the catalog pages
    addproducttocart_catalog: function (urladd, showqty, productid) {
        if (showqty.toLowerCase() == 'true') {
            var qty = document.querySelector('#addtocart_' + productid + '_EnteredQuantity').value();
            if (urladd.indexOf("forceredirection") != -1) {
                urladd += '&quantity=' + qty;
            }
            else {
                urladd += '?quantity=' + qty;
            }
        }
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        axios({
            url: urladd,
            method: 'post',
        }).then(function (response) {
            this.AxiosCart.success_process(response);
        }).catch(function (error) {
            error.axiosFailure;
        }).then(function (response) {
            this.AxiosCart.resetLoadWaiting(response);
        });  
    },

    //add a product to the cart/wishlist from the product details page
    addproducttocart_details: function (urladd, formselector) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);
        axios({
            url: urladd,
            data: formselector.serialize(),
            method: 'post',
        }).then(function (response) {
            this.AxiosCart.success_process(response);
        }).catch(function (error) {
            error.axiosFailure
        }).then(function () {
            this.AxiosCart.resetLoadWaiting(response);
        });  
    },

    //add bid
    addbid: function (urladd, formselector) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);
        axios({
            url: urladd,
            data: formselector.serialize(),
            method: 'post'
        }).then(function (response) {
            this.AxiosCart.success_process(response);
        }).catch(function (error) {
            error.axiosFailure;
        }).then(function () {
            this.AxiosCart.resetLoadWaiting(response);
        });  
    },
    //add a product to compare list
    addproducttocomparelist: function (urladd) {
        if (this.loadWaiting != false) {
            return;
        }
        this.setLoadWaiting(true);

        axios({
            url: urladd,
            method: 'post'
        }).then(function (response) {
            this.AxiosCart.success_process(response);
        }).catch(function (error) {
            error.axiosFailure;
        }).then(function () {
            this.AxiosCart.resetLoadWaiting(response);
        });  
    },

    success_process: function (response) {
        if (response.data.updatetopcartsectionhtml) {
            document.querySelector(AxiosCart.topcartselector).innerHTML = response.data.updatetopcartsectionhtml;
        }
        if (response.data.updatetopwishlistsectionhtml) {
            document.querySelector(AxiosCart.topwishlistselector).innerHTML = response.data.updatetopwishlistsectionhtml;
        }
        if (response.data.updateflyoutcartsectionhtml) {
            var flyoutcart = response.data.updateflyoutcartsectionhtml;
            new Vue({
                el: '.flyout-cart',
                data: {
                    template: null
                },
                render: function (createElement) {
                    if (!this.template) {
                        return createElement('b-icon', {
                            attrs: { icon: 'cart-check', animation: 'cylon', variant: 'success' }
                        });
                    } else {
                        return this.template();
                    }
                },
                mounted() {
                    var self = this;
                    setTimeout(function () {
                        self.template = Vue.compile(flyoutcart).render;
                    }, 1000);
                }
            });
        }
        if (response.data.comparemessage) {
            if (response.data.success == true) {
                displayBarNotification(response.data.comparemessage, 'success', 3500);
            }
            else {
                displayBarNotification(response.data.comparemessage, 'error', 3500);
            }
            return false;
        }
        if (response.data.product) {
            if (response.data.success == true) {
                $("#ModalQuickView .product-quickview").remove();
                displayPopupQuickView(response.data.innerHTML);
            }
        }
        if (response.data.message) {
            //display notification
            if (response.data.success == true) {
                //success
                $("#ModalQuickView .close").click();
                displayPopupAddToCart(response.data.innerHTML);

                if (response.data.refreshreservation == true) {
                    var param = "";
                    if ($("#parameterDropdown").val() != null) {
                        param = $("#parameterDropdown").val();
                    }

                    Reservation.fillAvailableDates(Reservation.currentYear, Reservation.currentMonth, param, true);
                }

            }
            else {
                //error
                displayBarNotification(response.data.message, 'error', 3500);
            }
            return false;
        }
        if (response.data.redirect) {
            location.href = response.data.redirect;
            return true;
        }
        return false;
    },

    resetLoadWaiting: function () {
        AxiosCart.setLoadWaiting(false);
    },

    axiosFailure: function () {
        alert('Failed to add the product. Please refresh the page and try one more time.');
    }
};