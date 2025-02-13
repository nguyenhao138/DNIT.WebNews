jQuery(document).ready(function ($) {
  // Slick slider for content-feature
  $(".content-feauture .caia-post-list-widget .main-posts").slick({
    arrows: false,
    infinite: true,
    dots: false,
    speed: 600,
    autoplay: true,
    autoplaySpeed: 5000,
    pauseOnHover: false,
    pauseOnFocus: false,
    slidesToShow: 1,
    slidesToScroll: 1,
  });

  // CountUp animation for content-number
  $(".content-number .widget_text .widgettitle span").countUp();

  // For mobile (width < 960)
  if ($(window).width() < 960) {
    var head = $(".site-header");
    var list = $("#responsive-menu");
    var menu = $("#click-menu");

    $(window).scroll(function () {
      if ($(this).scrollTop() > 80) {
        head.addClass("f-head");
        list.addClass("f-list");
        menu.addClass("f-menu");
      } else {
        head.removeClass("f-head");
        list.removeClass("f-list");
        menu.removeClass("f-menu");
      }
    });

    $(".site-header .widget-area .caia_code_widget").click(function () {
      $(".site-header .widget-area .widget_search").fadeIn();
    });
  } else {
    // For desktop (width >= 960)
    $(".timkiem a").click(function () {
      $(".site-header .widget-area .widget_search").fadeIn();
    });

    $(".site-header .widget-area .widget_search").click(function () {
      $(this).fadeOut();
    });

    $(".site-header .widget-area .widget_search .search-form").click(function (
      e
    ) {
      e.stopPropagation();
    });

    var head = $(".site-header");
    $(window).scroll(function () {
      if ($(this).scrollTop() > 120) {
        head.addClass("f-head");
      } else {
        head.removeClass("f-head");
      }
    });

    $(".content-client .widget_text .gallery").slick({
      arrows: true,
      infinite: true,
      dots: false,
      speed: 600,
      autoplay: true,
      autoplaySpeed: 5000,
      pauseOnHover: false,
      pauseOnFocus: false,
      slidesToShow: 6,
      slidesToScroll: 1,
      responsive: [
        {
          breakpoint: 961,
          settings: {
            slidesToShow: 4,
          },
        },
        {
          breakpoint: 801,
          settings: {
            slidesToShow: 3,
          },
        },
        {
          breakpoint: 601,
          settings: {
            slidesToShow: 2,
          },
        },
      ],
    });
  }

  // Smooth scroll for anchor links
  $("a[href*=\\#]:not([href=\\#])").click(function () {
    if (
      location.pathname.replace("/^//", "") ==
        this.pathname.replace("/^//", "") &&
      location.hostname == this.hostname
    ) {
      var target = $(this.hash);
      target = target.length ? target : $("[name=" + this.hash.slice(1) + "]");
      if (target.length) {
        $("html,body").animate(
          {
            scrollTop: target.offset().top - 50,
          },
          500
        );
        return false;
      }
    }
  });

  // Comment form validation
  $(".comment-form #submit").click(function () {
    var comment_name = $(".comment-form #author").val();
    var comment_phone = $(".comment-form .comment-form-phone #phone").val();
    var comment_content = $(".comment-form #comment").val();

    if (!comment_name) {
      alert("Bạn chưa nhập họ và tên!");
      return false;
    } else if (!comment_phone) {
      alert("Bạn chưa nhập số điện thoại!");
      return false;
    } else if (!comment_content) {
      alert("Bạn chưa nhập nội dung!");
      return false;
    } else {
      $(".content-notice").show();
      setTimeout(function () {
        $("#commentform").submit();
      }, 2000);
    }
  });

  // Back to top button functionality
  $("#backtotop").click(function () {
    var offset_top = jQuery("h1").offset();
    var top_it = typeof offset_top === "undefined" ? 0 : offset_top.top;
    top_it = top_it > 500 ? 0 : top_it;
    top_it = top_it > 30 ? top_it - 30 : top_it;
    $("body,html").animate({ scrollTop: top_it }, "slow");
  });

  var last_st = 0;
  $(window).scroll(function () {
    var st = $(this).scrollTop();
    if (st < last_st) {
      if (st > 500) {
        $("#backtotop").fadeIn();
      } else {
        $("#backtotop").fadeOut();
      }
    } else {
      $("#backtotop").hide();
    }
    last_st = st;
  });

  // Cookie functions (set and get cookies)
  function setCookie(e, t, o) {
    if (0 != o) {
      var i = new Date();
      i.setTime(i.getTime() + 24 * o * 60 * 60 * 1e3);
      var n = "expires=" + i.toUTCString();
      document.cookie = e + "=" + t + ";" + n + ";path=/";
    } else document.cookie = e + "=" + t + ";path=/";
  }
  function getCookie(e) {
    for (
      var t = e + "=", o = document.cookie.split(";"), i = 0;
      i < o.length;
      i++
    ) {
      for (var n = o[i]; " " == n.charAt(0); ) n = n.substring(1);
      if (0 == n.indexOf(t)) return n.substring(t.length, n.length);
    }
    return "";
  }

  // YouTube embedding on click to play
  (function () {
    var youtube = document.querySelectorAll(".youtube");
    for (var i = 0; i < youtube.length; i++) {
      var source =
        "https://img.youtube.com/vi/" +
        youtube[i].dataset.embed +
        "/hqdefault.jpg";
      var image = new Image();
      image.src = source;
      image.alt = "Youtube Video";
      image.addEventListener(
        "load",
        (function () {
          youtube[i].appendChild(image);
        })(i)
      );
      youtube[i].addEventListener("click", function () {
        var iframe = document.createElement("iframe");
        iframe.setAttribute("frameborder", "0");
        iframe.setAttribute("allowfullscreen", "");
        iframe.setAttribute(
          "src",
          "https://www.youtube.com/embed/" +
            this.dataset.embed +
            "?rel=0&showinfo=0&autoplay=1"
        );
        this.innerHTML = "";
        this.appendChild(iframe);
      });
    }
  })();

  
});
