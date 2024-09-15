// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('keydown', function (event) {
    if (event.ctrlKey && (event.key === '=' || event.key === '-' || event.key === '0')) {
        event.preventDefault();
    }
});

// Prevent zoom with mouse wheel + Ctrl on desktop
window.addEventListener('wheel', function (event) {
    if (event.ctrlKey) {
        event.preventDefault();
    }
}, { passive: false });