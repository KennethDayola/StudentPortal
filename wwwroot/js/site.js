// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const headerElement = document.querySelector('header');
    const containerElement = document.querySelector('.container');

    function adjustContainerPosition() {
        var headerHeight = headerElement.offsetHeight;
        containerElement.style.top = headerHeight + 'px'; // Set top position based on header height
    }

    adjustContainerPosition();

    window.addEventListener('resize', adjustContainerPosition);
});
