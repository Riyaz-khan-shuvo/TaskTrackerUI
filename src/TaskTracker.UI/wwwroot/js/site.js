$(document).ready(function () {
    // Desktop: open dropdown on hover
    // if ($(window).width() > 768) {
    $('.nav-item.dropdown').hover(
        function () {
            $(this).addClass('show');
            $(this).find('.dropdown-menu').addClass('show');
        },
        function () {
            $(this).removeClass('show');
            $(this).find('.dropdown-menu').removeClass('show');
        }
    );


    var drawer = $("#drawerMenu").kendoResponsivePanel({
        orientation: "left",
        breakpoint: 768,
        autoClose: true
    }).data("kendoResponsivePanel");

    $("#menuToggle").on("click", function () {
        drawer.toggle();
    });

    // Mobile submenu toggle
    $('#drawerMenu ul.k-menu > li > a').on('click', function (e) {
        e.preventDefault();
        var parentLi = $(this).parent('li');

        // Close other open submenus
        parentLi.siblings('li').removeClass('open').find('ul').slideUp(200);

        // Toggle current submenu
        parentLi.toggleClass('open');
        parentLi.find('ul').first().slideToggle(200);
    });

    // Optional: close drawer when clicking a submenu item
    $('#drawerMenu ul.k-menu li ul li a').on('click', function () {
        drawer.close();
    });
    $('#drawerClose').on('click', function () {
        drawer.close();
    });
});