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