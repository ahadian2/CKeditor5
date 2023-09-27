/*loader*/
window.addEventListener("load", () => {
    const loader = document.querySelector(".Loader");
    loader.classList.add("Loader-Hidden");
    loader.addEventListener("transitioned", () => {
        document.body.removeChild("Loader");
    })
});
//end loader
$(document).ready(function () {
    /*navbar-right*/
    $('.NavbarRight ul li:has(ul)').append('<i class="bi bi-caret-down-fill hassub"></i>');
    $(".hassub").on("click", function () {
        $(this).parent().children("ul").toggle("slow");

        if ($(this).hasClass('TrueSub')) {
            $(this).removeClass("TrueSub");
            $(this).addClass("FalseSub");
        } else {
            $(this).removeClass("FalseSub");
            $(this).addClass("TrueSub");
        }
    });
    /*end navbar-right*/
    /*navbar resize*/
    $(document).ready(function () {
        $(window).resize(function () {
            if ($(window).width() >= 991.98) {
                $('.NavbarRight').removeClass("NavbarRight-close");
                $('.NavbarRight').removeClass("NavbarRight-tablet");
            } else {
                $('.NavbarRight').addClass("NavbarRight-close");
                $('.NavbarRight').removeClass("NavbarRight-tablet");
            }
        }).resize();
    });
    /*end navbar resize*/
    /*btn navbar open close*/
    $(".icon-Menu").on("click", function () {
        var wd = $(window).width();
        if (wd >= 991.98) {
            if ($('.NavbarRight').hasClass('NavbarRight-close')) {
                $('.NavbarRight').removeClass("NavbarRight-close");
                $('.NavbarRight').addClass("col-lg-3");
                $('.NavbarRight').addClass(" col-xl-2");
                $('.NavbarRight').removeClass(" NavbarRight-tablet");
            } else {
                $('.NavbarRight').addClass("NavbarRight-close");
                $('.NavbarRight').removeClass("col-lg-3");
                $('.NavbarRight').removeClass(" col-xl-2");
                $('.NavbarRight').removeClass(" NavbarRight-tablet");
            }
        } else {
            if ($('.NavbarRight').hasClass('NavbarRight-tablet')) {
                $('.NavbarRight').removeClass("NavbarRight-tablet");
                $('.NavbarRight').addClass("NavbarRight-close");

            } else {
                $('.NavbarRight').addClass("NavbarRight-tablet");
                $('.NavbarRight').removeClass("NavbarRight-close");
            }
        }
    });
    /*end btn navbar open close*/
    /*open close search box*/
    $(".search-navbar-open").on("click", function () {
        $(".search-navbar").toggle("fast");
        $("#search").focus();
    });
    $(".search-navbar-close").on("click", function () {
        $(".search-navbar").toggle("fast");
    });
    /*end open close search box*/
    /*footer*/
    $("footer").on("click", function () {
        if ($(this).hasClass('footer-close')) {
            $(this).removeClass('footer-close');
        } else {
            $(this).addClass('footer-close');
        }
    });
    /*end footer*/
    /*night mode*/
    if (typeof $.cookie('tehranit_nightmode') === 'undefined') {
        //no cookie
        $.cookie("tehranit_nightmode", "light", { expires: 7, path: '/' });

        $('link[href*="AdminDark.css"]')[0].disabled = true;
        $('link[href*="Admin.css"]')[0].disabled = false;

    } else {
        //have cookie
        var nightmode = $.cookie("tehranit_nightmode");
        if (nightmode == "light") {
            $(".txt-night").text("حالت روز");
            $(".icon-night").removeClass("bi-moon-fill");
            $(".icon-night").addClass("bi-brightness-high-fill");

            $('link[href*="AdminDark.css"]')[0].disabled = true;
            $('link[href*="Admin.css"]')[0].disabled = false;

            $("#night-mode-switch").attr('checked', false);
        } else {
            $(".txt-night").text("حالت شب");
            $(".icon-night").addClass("bi-moon-fill");
            $(".icon-night").removeClass("bi-brightness-high-fill");

            $('link[href*="AdminDark.css"]')[0].disabled = false;
            $('link[href*="Admin.css"]')[0].disabled = true;

            $("#night-mode-switch").attr('checked', true);
        }
    }
    $("#night-mode-switch").on("click", function () {
        $.removeCookie("tehranit_nightmode");
        if ($("#night-mode-switch").is(':checked')) {
            $(".txt-night").text("حالت شب");
            $(".icon-night").addClass("bi-moon-fill");
            $(".icon-night").removeClass("bi-brightness-high-fill");

            $('link[href*="AdminDark.css"]')[0].disabled = false;
            $('link[href*="Admin.css"]')[0].disabled = true;


            $.cookie("tehranit_nightmode", "dark", { expires: 7, path: '/' });
        }
        else {
            $(".txt-night").text("حالت روز");
            $(".icon-night").removeClass("bi-moon-fill");
            $(".icon-night").addClass("bi-brightness-high-fill");

            $('link[href*="AdminDark.css"]')[0].disabled = true;
            $('link[href*="Admin.css"]')[0].disabled = false;

            $.cookie("tehranit_nightmode", "light", { expires: 7, path: '/' });
        }
    });
    /*end night mode*/




});