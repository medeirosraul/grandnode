var flycartMix = {
    props: {
        flycart: null,
        flycartitems: null,
        flycartindicator: null,
    },
    methods: {
        updateFly() {
            axios({
                baseURL: '/Component/Index?Name=FlyoutShoppingCart',
                method: 'get',
                data: null,
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'X-Response-View': 'Json'
                }
            }).then(response => (
                this.flycart = response.data,
                this.flycartitems = response.data.Items,
                this.flycartindicator = response.data.TotalProducts
            ))
        }
    }
}
var vm = new Vue({
    el: '#app',
    mixins: [flycartMix],
    data() {
        return {
            show: false,
            fluid: false,
            hover: false,
            active: false,
            NextDropdownVisible: false,
            value: 5,
            searchitems: null,
            searchcategories: null,
            searchmanufacturers: null,
            searchblog: null,
            searchproducts: null
        }
    },
    mounted() {
        if (localStorage.fluid == "true") this.fluid = "fluid";
        if (localStorage.fluid == "fluid") this.fluid = "fluid";
        if (localStorage.fluid == "") this.fluid = "false";
        window.addEventListener('scroll', this.handleScroll);
        this.isMobile();
        this.updateFly();
    },
    watch: {
        fluid(newName) {
            localStorage.fluid = newName;
        },
    },
    methods: {
        showModalBackInStock() {
            this.$refs['back-in-stock'].show()
        },
        validateBeforeSubmit(event) {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    event.srcElement.submit();
                    return
                } else {
                    if (document.querySelector('#PickUpInStore:checked')) {
                        event.srcElement.submit();
                        return
                    }
                    if (event.target.querySelectorAll('#pickup-new-address-form').length > 0) {
                        if (event.target.querySelector('#pickup-new-address-form').style['display'] === 'none') {
                            event.srcElement.submit();
                            return
                        }
                    }
                }
            });
        },
        validateBeforeClick(event) {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    var callFunction = event.srcElement.getAttribute('data-click');
                    eval(callFunction)
                    return
                }
            });
        },
        isMobile() {
            return (typeof window.orientation !== "undefined") || (navigator.userAgent.indexOf('IEMobile') !== -1);
        }
    }
});