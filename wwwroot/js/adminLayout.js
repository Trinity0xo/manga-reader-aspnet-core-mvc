const sideBar = $(".side-bar");
const sideBarToggler = $("#sideBarToggler");
const overlay = $(".overlay");
const body = $("body");

sideBarToggler.on("click", function () {
  if ($(window).width() > 768) {
    sideBar.toggleClass("collapsed");
  }
});

$(document).on("click", function (e) {
  if (!$(e.target).closest(".side-bar").length) {
    if ($(window).width() <= 768) {
      sideBar.removeClass("show");
      overlay.css("display", "none");
      // body.css("position", "unset");
    }
  }

  if ($(e.target).closest("#sideBarToggler").length) {
    if ($(window).width() <= 768) {
      sideBar.addClass("show");
      overlay.css("display", "block");
      // body.css("position", "fixed");
    }
  }
});
