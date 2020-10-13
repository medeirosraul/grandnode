function deletecartitem(href) {
    axios({
        method: "post",
        url: href
    }).then(function (response) {
        var flyoutcart = response.data.flyoutshoppingcart;
        new Vue({
            el: '.flyout-cart',
            data: {
                template: null
            },
            render: function (createElement) {
                if (!this.template) {
                    
                } else {
                    return this.template();
                }
            },
            mounted() {
                var self = this;
                self.template = Vue.compile(flyoutcart).render;
            }
        });
    }).catch(function (xhr, ajaxOptions, thrownError) {
        console.log('Failed to retrieve Flyout Shopping Cart.');
    });
    return false;
}

function displayPopupAddToCart(html) {
    document.querySelector('.modal-place').innerHTML = html;
        new Vue({
            el: '#ModalAddToCart',
            data: {
                template: null,
            },
            render: function (createElement) {
                if (!this.template) {
                    return createElement('b-overlay', {
                        attrs: { show: 'true' }
                    });
                } else {
                    return this.template();
                }
            },
            methods: {
                showModal() {
                    this.$refs['ModalAddToCart'].show()
                }
            },
            mounted() {
                var self = this;
                self.template = Vue.compile(html).render;
            },
            updated: function () {
                this.showModal();
            }
        });
}

function displayPopupQuickView(html) {
    document.querySelector('.modal-place').innerHTML = html;
    new Vue({
        el: '#ModalQuickView',
        data: {
            template: null,
        },
        render: function (createElement) {
            if (!this.template) {
                return createElement('b-overlay', {
                    attrs: { show: 'true' }
                });
            } else {
                return this.template();
            }
        },
        methods: {
            showModal() {
                this.$refs['ModalQuickView'].show()
            }
        },
        mounted() {
            var self = this;
            self.template = Vue.compile(html).render;
        },
        updated: function () {
            this.showModal();
        }
    });
}


function displayBarNotification(message, messagetype, timeout) {
    new Vue({
        el: "#app",
        methods: {
            toast() {
                if (messagetype == 'error') {
                    this.$bvToast.toast(message, {
                        title: messagetype,
                        variant: 'danger',
                        autoHideDelay: timeout,
                    })
                } else {
                    this.$bvToast.toast(message, {
                        title: messagetype,
                        variant: 'info',
                        autoHideDelay: timeout,
                    })
                }
            }
        },
        mounted: function () {
            this.toast();
        }
    });
}

// CSRF (XSRF) security
function addAntiForgeryToken(data) {
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = document.querySelector('input[name=__RequestVerificationToken]');
    if (tokenInput) {
        data.__RequestVerificationToken = tokenInput.value;
    }
    return data;
};

function newsletter_subscribe(subscribe) {
    var subscribeProgress = document.getElementById("subscribe-loading-progress");
    subscribeProgress.style.display = "block";
    var postData = {
        subscribe: subscribe,
        email: document.getElementById("newsletter-email").value
    };
    var href = document.getElementById("newsletterbox").getAttribute('data-href');
    axios({
        url: href,
        params: postData,
        method: 'post',
    }).then(function (response) {
        subscribeProgress.style.display = "none";
        document.getElementById("newsletter-result-block").innerHTML = response.data.Result;
        console.log(response);
        if (response.data.Success) {
            document.querySelector('.newsletter-inputs .input-group').style.display = "none";
            document.getElementById('newsletter-result-block').classList.add("d-block").style.bottom = "unset";
            if (data.response.Showcategories) {
                document.getElementById('#nc_modal_form').innerHTML = response.data.ResultCategory;
                window.setTimeout(function () {
                    //document.querySelector('.nc-action-form').magnificPopup('open');
                }, 100);
            }
        } else {
            window.setTimeout(function () {
                document.getElementById('newsletter-result-block').style.display = "none"
            }, 2000);
        }
    }).catch(function (error) {
        subscribeProgress.style.display = "none";
    })
}
window.onload = function () {
    var el = document.getElementById('newsletter-subscribe-button');
    el.onclick = function () {
        var allowToUnsubscribe = document.getElementById("newsletterbox").getAttribute('data-allowtounsubscribe').toLowerCase();
        if (allowToUnsubscribe == 'true') {
            if (document.getElementById('newsletter_subscribe').checked) {
                newsletter_subscribe('true');
            }
            else {
                newsletter_subscribe('false');
            }
        }
        else {
            newsletter_subscribe('true');
        }
    };
    document.getElementById("newsletter-email").addEventListener("keyup", function (event) {
        if (event.keyCode == 13) {
            document.getElementById("newsletter-subscribe-button").click();
        }
    });
}