// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function checkScreenWidth() {
    if (window.innerWidth < 1024) {
        window.location.href = "/Home/Unsupported";
    }
}

window.onload = checkScreenWidth;
window.onresize = checkScreenWidth;
