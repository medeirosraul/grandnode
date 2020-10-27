// fire if first slide has set full width

if (document.querySelector('#GrandCarousel .carousel-item').getAttribute('data-width') == 'full') {
    var mainContainer = document.querySelector(".main-container");
    var GrandCarousel = document.getElementById("GrandCarousel");
    var parentDiv = mainContainer.parentNode;

    parentDiv.insertBefore(GrandCarousel, mainContainer);
}