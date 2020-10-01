
function deletecartitem(href) {
    var flyoutcartselector = AxiosCart.flyoutcartselector;
    var topcartselector = AxiosCart.topcartselector;
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
        document.querySelector(topcartselector).innerHTML = response.data.totalproducts;
    }).catch(function (xhr, ajaxOptions, thrownError) {
        console.log('Failed to retrieve Flyout Shopping Cart.');
    });
    return false;
}