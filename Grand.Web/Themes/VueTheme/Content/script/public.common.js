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