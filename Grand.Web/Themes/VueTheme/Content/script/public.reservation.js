var Reservation = {
    availableDates: [],
    availableDatesFrom: [],
    availableDatesTo: [],
    currentMonth: -1,
    currentYear: -1,
    currentMonthFrom: -1,
    currentYearFrom: -1,
    currentMonthTo: -1,
    currentYearTo: -1,
    _parameter: "",
    noReservationsMessage: "",
    productId: "",
    ajaxUrl: "",
    ajaxUrl2: "",
    startDate: "",
    startDateMonth: "",
    startDateYear: "",

    init: function init(startDate, startDateYear, startDateMonth, noReservationsMessage, ajaxUrl, productId, ajaxUrl2) {
        this.noReservationsMessage = noReservationsMessage;
        this.ajaxUrl = ajaxUrl;
        this.ajaxUrl2 = ajaxUrl2;
        this.productId = productId;
        this.startDate = startDate;
        this.startDateMonth = startDateMonth;
        this.startDateYear = startDateYear;

        if (document.getElementById("reservationDatepicker") != null) {
            this.fillAvailableDates(startDateYear, startDateMonth, Reservation._parameter, false);
        }

        if (document.getElementById("reservationDatepickerFrom") != null) {
            this.fillAvailableDatesFrom(startDateYear, startDateMonth);
        }

        if (document.getElementById("reservationDatepickerTo") != null) {
            this.fillAvailableDatesTo(startDateYear, startDateMonth);
        }
        var defdate = new Date(this.startDate);

        //$("#reservationDatepicker").datepicker({
        //    onSelect: this.onDatePickerDateChange,
        //    firstDay: 1,
        //    beforeShowDay: this.daysToMark,
        //    onChangeMonthYear: function (year, month, inst) {
        //        $("#hoursDiv").html('');
        //        Reservation.fillAvailableDates(year, month, Reservation._parameter, false);
        //        Reservation.onDatePickerDateChange();
        //    },
        //    defaultDate: defdate
        //});
        var reservationDatepicker = new Pikaday({
            field: document.getElementById('reservationDatepicker'),
            onSelect: this.onDatePickerDateChange,
            disableDayFn: this.daysToMark,
            format: 'D/M/YYYY',
            toString(date, format) {
                // you should do formatting based on the passed format,
                // but we will just return 'D/M/YYYY' for simplicity
                const day = date.getDate();
                const month = date.getMonth() + 1;
                const year = date.getFullYear();
                return `${day}/${month}/${year}`;
            },
            parse(dateString, format) {
                // dateString is the result of `toString` method
                const parts = dateString.split('/');
                const day = parseInt(parts[0], 10);
                const month = parseInt(parts[1], 10) - 1;
                const year = parseInt(parts[2], 10);
                return new Date(year, month, day);
            },
            onDraw: function (date) {
                var year = date.calendars[0].year.toString();
                var month = date.calendars[0].month.toString();
                if (document.getElementById("hoursDiv") != null) {
                    document.getElementById("hoursDiv").innerHTML = '';
                }
                Reservation.fillAvailableDates(year, month, Reservation._parameter, false);
                Reservation.onDatePickerDateChange();
            },
            firstDay: 1,
            defaultDate: defdate
        });

        //$("#reservationDatepickerFrom").datepicker({
        //    firstDay: 1,
        //    defaultDate: defdate,
        //    onSelect: this.onDatePickerSelect,
        //    beforeShowDay: this.daysToMarkFrom,
        //    onChangeMonthYear: function (year, month, inst) {
        //        Reservation.fillAvailableDatesFrom(year, month);
        //    },
        //});

        var reservationDatepickerFrom = new Pikaday({
            field: document.getElementById('reservationDatepickerFrom'),
            onSelect: this.onDatePickerSelect,
            disableDayFn: this.daysToMarkFrom,
            format: 'M/D/YYYY',
            toString(date, format) {
                // you should do formatting based on the passed format,
                // but we will just return 'D/M/YYYY' for simplicity
                const day = date.getDate();
                const month = date.getMonth() + 1;
                const year = date.getFullYear();
                return `${month}/${day}/${year}`;
            },
            parse(dateString, format) {
                // dateString is the result of `toString` method
                const parts = dateString.split('/');
                const day = parseInt(parts[0], 10);
                const month = parseInt(parts[1], 10) - 1;
                const year = parseInt(parts[2], 10);
                return new Date(year, month, day);
            },
            onDraw: function (date) {
                var year = date.calendars[0].year.toString();
                var month = date.calendars[0].month.toString();
                Reservation.fillAvailableDatesFrom(year, month);
            },
            firstDay: 1,
            defaultDate: defdate
        });

        //$("#reservationDatepickerTo").datepicker({
        //    firstDay: 1,
        //    defaultDate: defdate,
        //    onSelect: this.onDatePickerSelect,
        //    beforeShowDay: this.daysToMarkTo,
        //    onChangeMonthYear: function (year, month, inst) {
        //        Reservation.fillAvailableDatesTo(year, month);
        //    },
        //});
        var reservationDatepickerTo = new Pikaday({
            field: document.getElementById('reservationDatepickerTo'),
            onSelect: this.onDatePickerSelect,
            disableDayFn: this.daysToMarkTo,
            format: 'M/D/YYYY',
            toString(date, format) {
                // you should do formatting based on the passed format,
                // but we will just return 'D/M/YYYY' for simplicity
                const day = date.getDate();
                const month = date.getMonth() + 1;
                const year = date.getFullYear();
                return `${month}/${day}/${year}`;
            },
            parse(dateString, format) {
                // dateString is the result of `toString` method
                const parts = dateString.split('/');
                const day = parseInt(parts[0], 10);
                const month = parseInt(parts[1], 10) - 1;
                const year = parseInt(parts[2], 10);
                return new Date(year, month, day);
            },
            onDraw: function (date) {
                var year = date.calendars[0].year.toString();
                var month = date.calendars[0].month.toString();
                Reservation.fillAvailableDatesFrom(year, month);
            },
            firstDay: 1,
            defaultDate: defdate
        });

        this.onDatePickerDateChange();
        var dropdown = document.getElementById("parameterDropdown");
        if (dropdown != null) {
            document.querySelector("#parameterDropdown").addEventListener('change', function () {
                Reservation.fillAvailableDates(Reservation.currentYear, Reservation.currentMonth, this.value, true);
            });
        }
    },

    reload: function init(startDate, startDateYear, startDateMonth) {
        this.startDate = startDate;
        this.startDateMonth = startDateMonth;
        this.startDateYear = startDateYear;

        this.fillAvailableDates(startDateYear, startDateMonth, Reservation._parameter, false);

        //$("#reservationDatepicker").datepicker({
        //    onSelect: this.onDatePickerDateChange,
        //    firstDay: 1,
        //    beforeShowDay: this.daysToMark,
        //    onChangeMonthYear: function (year, month, inst) {
        //        if (document.getElementById("hoursDiv") != null) {
        //            document.getElementById("hoursDiv").innerHTML = '';
        //        }
        //        Reservation.fillAvailableDates(year, month, Reservation._parameter, false);
        //    },
        //    defaultDate: this.startDate
        //});

        var reservationDatepickerFromRe = new Pikaday({
            field: document.getElementById('reservationDatepicker'),
            onSelect: this.onDatePickerDateChange,
            disableDayFn: this.daysToMark,
            format: 'M/D/YYYY',
            toString(date, format) {
                // you should do formatting based on the passed format,
                // but we will just return 'D/M/YYYY' for simplicity
                const day = date.getDate();
                const month = date.getMonth() + 1;
                const year = date.getFullYear();
                return `${month}/${day}/${year}`;
            },
            parse(dateString, format) {
                // dateString is the result of `toString` method
                const parts = dateString.split('/');
                const day = parseInt(parts[0], 10);
                const month = parseInt(parts[1], 10) - 1;
                const year = parseInt(parts[2], 10);
                return new Date(year, month, day);
            },
            onDraw: function (year, month, inst) {
                if (document.getElementById("hoursDiv") != null) {
                    document.getElementById("hoursDiv").innerHTML = '';
                }
                Reservation.fillAvailableDates(year, month, Reservation._parameter, false);
            },
            firstDay: 1,
            defaultDate: this.startDate
        });

        this.onDatePickerDateChange();
    },

    daysToMark: function daysToMark(date) {
        for (i = 0; i < Reservation.availableDates.length; i++) {
            var splitResults = Reservation.availableDates[i].Date.split("-");
            var year = splitResults[0];
            var month = splitResults[1];
            var day = splitResults[2].substring(0, 2);
            if (date.getYear() + 1900 == year && date.getMonth() + 1 == month && date.getDate() == day) {
                return false;
            }
        }

        return true;
    },

    daysToMarkTo: function daysToMark(date) {
        for (i = 0; i < Reservation.availableDatesTo.length; i++) {
            var splitResults = Reservation.availableDatesTo[i].Date.split("-");
            var year = splitResults[0];
            var month = splitResults[1];
            var day = splitResults[2].substring(0, 2);

            if (date.getYear() + 1900 == year && date.getMonth() + 1 == month && date.getDate() == day) {
                return false
            }
        }

        return true;
    },

    daysToMarkFrom: function daysToMark(date) {
        for (i = 0; i < Reservation.availableDatesFrom.length; i++) {
            var splitResults = Reservation.availableDatesFrom[i].Date.split("-");
            var year = splitResults[0];
            var month = splitResults[1];
            var day = splitResults[2].substring(0, 2);
            if (date.getYear() + 1900 == year && date.getMonth() + 1 == month && date.getDate() == day) {
                return false;
            }
        }

        return true;
    },

    onDatePickerDateChange: function onDatePickerDateChange() {
        if (document.querySelector("#reservationDatepicker") != null) {
            var selected = document.querySelector("#reservationDatepicker").value;
        } else {
            var selected = null;
        }
        if (selected != null) {
            var selectedSplitResults = selected.split("/");
            var selectedDay = selectedSplitResults[1];
            var selectedMonth = selectedSplitResults[0];
            var selectedYear = selectedSplitResults[2];

            document.querySelector("#hoursDiv").innerHTML = '';
            for (i = 0; i < Reservation.availableDates.length; i++) {
                var splitResults = Reservation.availableDates[i].Date.split("-");
                var year = splitResults[0];
                var month = splitResults[1];
                var day = splitResults[2].substring(0, 2);

                if (selectedYear == year && selectedMonth == month && selectedDay == day) {
                    document.querySelector("#hoursDiv").appendChild("<div class='custom-control custom-radio mx-1'><input class='custom-control-input' type='radio' id='Reservation_" + Reservation.availableDates[i].Id + "' name='Reservation' value='" + Reservation.availableDates[i].Id + "' /><label class='custom-control-label' for='Reservation_" + Reservation.availableDates[i].Id + "'>" + Reservation.availableDates[i].Date.substring(11, 16) + "</label></div>");
                }
            }

            if (Reservation.availableDates.length == 0) {
                var label = document.createElement('label');
                label.innerHTML = Reservation.noReservationsMessage;
                document.querySelector("#hoursDiv").appendChild(label);
            }
        }
    },

    fillAvailableDates: function fillAvailableDates(year, month, parameter, reload) {
        var postData = {
            productId: Reservation.productId,
            month: month,
            year: year,
            parameter: parameter
        };

        addAntiForgeryToken(postData);

        axios({
            url: Reservation.ajaxUrl,
            method: 'post',
            data: postData,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(response => {
            Reservation.currentMonth = month;
            Reservation.currentYear = year;
            Reservation.availableDates = response.data;
            if (reload) {
                Reservation._parameter = parameter;
                //$("#reservationDatepicker").datepicker("destroy");
                Reservation.reload(new Date(year, month - 1, 1), Reservation.currentYear, Reservation.currentMonth);
                //$("#reservationDatepicker").datepicker("refresh");
            }
        }).catch(function (error) {
            alert(error)
        })
        //$.ajax({
        //    cache: false,
        //    type: "POST",
        //    url: Reservation.ajaxUrl,
        //    dataType: 'json',
        //    data: postData,
        //    async: false
        //}).done(function (data) {
        //    Reservation.currentMonth = month;
        //    Reservation.currentYear = year;
        //    Reservation.availableDates = data;
        //    if (reload) {
        //        Reservation._parameter = parameter;
        //        $("#reservationDatepicker").datepicker("destroy");
        //        Reservation.reload(new Date(year, month - 1, 1), Reservation.currentYear, Reservation.currentMonth);
        //        $("#reservationDatepicker").datepicker("refresh");
        //    }
        //}).fail(function () {
        //    alert("Error");
        //});


    },

    fillAvailableDatesFrom: function fillAvailableDatesFrom(year, month) {
        var postData = {
            productId: Reservation.productId,
            month: month,
            year: year,
            parameter: null
        };
        addAntiForgeryToken(postData);

        axios({
            url: Reservation.ajaxUrl,
            method: 'post',
            params: postData,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(response => {
            Reservation.currentMonthFrom = month;
            Reservation.currentYearFrom = year;
            Reservation.availableDatesFrom = Reservation.availableDatesFrom.concat(response.data);
            return true;
        }).catch(function (error) {
            alert(error)
        })
    },

    fillAvailableDatesTo: function fillAvailableDatesTo(year, month) {
        var postData = {
            productId: Reservation.productId,
            month: month,
            year: year,
            parameter: null
        };

        addAntiForgeryToken(postData);

        axios({
            url: Reservation.ajaxUrl,
            method: 'post',
            params: postData,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }).then(response => {
            Reservation.currentMonthTo = month;
            Reservation.currentYearTo = year;
            Reservation.availableDatesTo = Reservation.availableDatesTo.concat(response.data);
        }).catch(function (error) {
            alert(error)
        })
    },

    onDatePickerSelect: function onDatePickerSelect() {
        var form = document.querySelector('#product-details-form');
        var data = new FormData(form);
        axios({
            url: Reservation.ajaxUrl2,
            method: 'post',
            data: data,
        }).then(response => {
            console.log(response);
            if (response.data.sku) {
                document.querySelector("#sku-" + Reservation.productId).innerText = response.data.sku;
            }
            if (response.data.mpn) {
                document.querySelector("#mpn-" + Reservation.productId).innerText = response.data.mpn;
            }
            if (response.data.gtin) {
                document.querySelector("#gtin-" + Reservation.productId).innerText = response.data.gtin;
            }
            if (response.data.price) {
                document.querySelector(".price-value-" + Reservation.productId).innerText = response.data.price;
            }
        })
    }
}
