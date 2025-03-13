// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelector(".account-btn").addEventListener("click", function () {
    document.querySelector(".dropdown-content").classList.toggle("show");
});

window.addEventListener("click", function (e) {
    if (!document.querySelector(".account-menu").contains(e.target)) {
        document.querySelector(".dropdown-content").classList.remove("show");
    }
});
document.addEventListener("DOMContentLoaded", function () {
    let menuButton = document.getElementById("menu-button");
    let dropdownMenu = document.getElementById("dropdown-menu");

    menuButton.addEventListener("click", function (event) {
        event.stopPropagation(); // Ngăn chặn sự kiện lan ra ngoài
        dropdownMenu.style.display = dropdownMenu.style.display === "flex" ? "none" : "flex";
    });

    document.addEventListener("click", function () {
        dropdownMenu.style.display = "none";
    });

    dropdownMenu.addEventListener("click", function (event) {
        event.stopPropagation(); // Để khi ấn vào menu nó không bị tắt
    });
});

