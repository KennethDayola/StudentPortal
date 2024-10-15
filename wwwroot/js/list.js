document.querySelectorAll('.btn-link').forEach(function (button) {
    button.addEventListener('click', function () {
        this.parentElement.classList.toggle('show');
    });
});
document.querySelectorAll('.nav-ic, .nav-ic-selected').forEach(function (div) {
    div.addEventListener('click', function () {
        window.location.href = this.getAttribute('data-href');
    });
});
$('.middle-initial').each(function () {
    const middleInitial = $(this).text().trim();
    if (middleInitial === '�') {
        $(this).text('-');
    }
});
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $(".dropdown-item").click(function (e) {
        e.preventDefault();

        $(this).closest(".dropdown").removeClass("show");

        const studentId = $(this).data("id");

        // Perform the AJAX request to load the edit form
        $.get("/Students/Edit/" + studentId, function (data) {
            $("#editContent").html(data);  // Populate the modal with the edit form

            // Show the modal and apply animation
            $("#editModal").show();
            $(".modal-content").css("animation", "slideIn 0.5s forwards");
        });
    });

    $(".close").click(function () {
        $(".modal-content").css("animation", "slideOut 0.75s forwards");

        setTimeout(function () {
            $("#editModal").hide();
        }, 750);
    });

    $(window).click(function (event) {
        if ($(event.target).is("#editModal")) {
            $(".modal-content").css("animation", "slideOut 0.75s forwards");
            setTimeout(function () {
                $("#editModal").hide();
            }, 750);
        }
    });
});