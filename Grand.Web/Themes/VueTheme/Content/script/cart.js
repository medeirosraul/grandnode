function deleteitem(href) {
    axios({
        method: 'post',
        url: href,
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
        window.location.reload();
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
        //new Vue({
        //    el: '.static-cart',
        //    data: {
        //        template: null
        //    },
        //    render: function (createElement) {
        //        if (!this.template) {

        //        } else {
        //            return this.template();
        //        }
        //    },
        //    mounted() {
        //        var self = this;
        //        self.template = Vue.compile(cartChange).render;
        //    }
        //});
        window.location.reload();
    }).catch(function (error) {
        alert('Failed to retrieve Shopping Cart Page.');
    })
}
//setTimeout(function () {
//    var button = document.querySelectorAll(".changeshoppingcartitem");
//    button.forEach(function () {
//        this.addEventListener("click", function (e) {
//            var self = this;
//            var href = self.querySelectorAll('.custom-control-input').getAttribute('data-href');
//            axios({
//                method: 'post',
//                data: data,
//                url: href,
//            }).then(function (response) {
//                var cartChange = response.data.cart;
//                //new Vue({
//                //    el: '.static-cart',
//                //    data: {
//                //        template: null
//                //    },
//                //    render: function (createElement) {
//                //        if (!this.template) {

//                //        } else {
//                //            return this.template();
//                //        }
//                //    },
//                //    mounted() {
//                //        var self = this;
//                //        self.template = Vue.compile(cartChange).render;
//                //    }
//                //});
//                window.location.reload();
//            }).catch(function (error) {
//                alert('Failed to retrieve Shopping Cart Page.');
//            })
//        }, false);
//    });
//}, 300);