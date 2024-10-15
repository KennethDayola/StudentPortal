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

        // Determine if the active view is "Students" or "Subjects"
        let activeView = window.location.pathname.includes("Students") ? "Students" : "Subjects";

        // Use 'id' for Students and 'code' for Subjects
        const identifier = activeView === "Students" ? $(this).data("id") : $(this).data("code");

        // Perform the AJAX request to load the correct edit form based on the active view
        $.get("/" + activeView + "/Edit/" + identifier, function (data) {
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