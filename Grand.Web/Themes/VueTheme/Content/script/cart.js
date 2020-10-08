function deleteitem(href) {
    axios({
        method: 'post',
        url: href,
    }).then(function (response) {
        var flyoutcart = response.data.flyoutshoppingcart;
        var cartChange = response.data.cart;
        var delButtons = document.querySelector('.remove-from-cart');
        document.querySelector('#ordersummarypagecart').innerHTML = cartChange;
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
        new Vue({
            el: '.checkout-buttons',
            mixins: [mix]
        });
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    })
}

function ChangeShoppingCart() {
    var form = document.getElementById('shopping-cart-form');
    var data = new FormData(form);
    axios({
        method: 'post',
        data: data,
        url: '/updatecart',
    }).then(function (response) {
        var cartChange = response.data.cart;
        document.querySelector('#ordersummarypagecart').innerHTML = cartChange;
        new Vue({
            el: '.checkout-buttons',
            mixins: [mix] 
        });
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    })
}