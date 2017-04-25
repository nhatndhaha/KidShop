$(document).ready(function () {
    // Home slider
    $("#sk-home-slider").owlCarousel({
        slideSpeed: 300,
        paginationSpeed: 400,
        pagination: false,
        singleItem: true
    });

    // Left sidebar products slider
    $("#sellingProducts-slider").owlCarousel({
        slideSpeed: 300,
        paginationSpeed: 400,
        items: 1,
        itemsDesktop: false,
        itemsDesktopSmall: false,
        itemsTablet: false,
        itemsTabletSmall: false,
        itemsMobile: false,
        pagination: false,
        autoPlay: true
    });

    if (window.innerWidth < 992) {
        $("#sk-parallax-carousel").addClass('owl-carousel owl-theme');
        $("#sk-parallax-carousel").owlCarousel({
            slideSpeed: 300,
            paginationSpeed: 400,
            items: 1,
            pagination: false,
            autoPlay: true
        });
    }

    if ((window.innerWidth > 767) && (window.innerWidth < 992)) {
        $("#sk-parallax-carousel").addClass('owl-carousel owl-theme');
        $("#sk-parallax-carousel .owl-item .sk-parallax-item").removeClass('col-md-6 col-sm-6');
        $("#sk-parallax-carousel").owlCarousel({
            slideSpeed: 300,
            paginationSpeed: 400,
            items: 2,
            itemsDesktop: false,
            itemsDesktopSmall: false,
            itemsTablet: false,
            itemsTabletSmall: false,
            itemsMobile: false,
            pagination: false,
            autoPlay: true
        });
    }

    // Custom nav for Left sidebar products slider
    var owl = $("#sellingProducts-slider");

    owl.owlCarousel();

    // Custom Navigation Events
    $(".customNavigation .next").click(function () {
        owl.trigger('owl.next');
    });
    $(".customNavigation .prev").click(function () {
        owl.trigger('owl.prev');
    });

    // Custom nav for products care slider
    var owl2 = $("#product-care");

    owl2.owlCarousel();

    // Custom Navigation Events
    $(".productCareNavigation .next").click(function () {
        owl2.trigger('owl.next');
    });
    $(".productCareNavigation .prev").click(function () {
        owl2.trigger('owl.prev');
    });

    // #discounts-product
    $("#discounts-product").owlCarousel({
        slideSpeed: 300,
        paginationSpeed: 400,
        pagination: false,
        items: 1,
        itemsDesktop: false,
        itemsDesktopSmall: false,
        itemsTablet: false,
        itemsTabletSmall: false,
        itemsMobile: false,
        autoPlay: true
    });
    // #discounts-product
    $("#product-care").owlCarousel({
        slideSpeed: 300,
        paginationSpeed: 400,
        items: 5,
        pagination: false,
        autoPlay: true
    });

    // Search box in menu
    $('#sk-button').click(function () {
        $('.sk-search-input').toggleClass("sk-search-show");
    });

    var sync1 = $("#sync1");
    var sync2 = $("#sync2");

    sync1.owlCarousel({
        singleItem: true,
        slideSpeed: 1000,
        navigation: true,
        pagination: false,
        afterAction: syncPosition,
        responsiveRefreshRate: 200,
    });

    sync2.owlCarousel({
        items: 5,
        itemsDesktop: [1199, 10],
        itemsDesktopSmall: [979, 10],
        itemsTablet: [768, 8],
        itemsMobile: [479, 4],
        pagination: false,
        responsiveRefreshRate: 100,
        afterInit: function (el) {
            el.find(".owl-item").eq(0).addClass("synced");
        }
    });

    function syncPosition(el) {
        var current = this.currentItem;
        $("#sync2")
			.find(".owl-item")
			.removeClass("synced")
			.eq(current)
			.addClass("synced")
        if ($("#sync2").data("owlCarousel") !== undefined) {
            center(current)
        }
    }

    $("#sync2").on("click", ".owl-item", function (e) {
        e.preventDefault();
        var number = $(this).data("owlItem");
        sync1.trigger("owl.goTo", number);
    });

    function center(number) {
        var sync2visible = sync2.data("owlCarousel").owl.visibleItems;
        var num = number;
        var found = false;
        for (var i in sync2visible) {
            if (num === sync2visible[i]) {
                var found = true;
            }
        }

        if (found === false) {
            if (num > sync2visible[sync2visible.length - 1]) {
                sync2.trigger("owl.goTo", num - sync2visible.length + 2)
            } else {
                if (num - 1 === -1) {
                    num = 0;
                }
                sync2.trigger("owl.goTo", num);
            }
        } else if (num === sync2visible[sync2visible.length - 1]) {
            sync2.trigger("owl.goTo", sync2visible[1])
        } else if (num === sync2visible[0]) {
            sync2.trigger("owl.goTo", num - 1)
        }

    }
});

