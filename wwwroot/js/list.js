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
$('.middle-name').each(function () {
    const middleName = $(this).text().trim();
    if (middleName === '�') {
        $(this).text('-');
    }
});

$(document).ready(function () {
    //$("#searchEDP").on("keyup", function() {
    //    var value = $(this).val().toLowerCase(); // Get the input value

    //    // Loop through all table rows
    //    $("table tbody tr").filter(function() {
    //        // Find the second column (EDP Code) and toggle visibility based on its text
    //        $(this).toggle($(this).find("td:nth-child(2)").text().toLowerCase().indexOf(value) > -1);
    //    });
    //});

    $('[data-toggle="tooltip"]').tooltip();

    $(".dropdown-item").click(function (e) {
        e.preventDefault();

        $(this).closest(".dropdown").removeClass("show");

        // Determine if the active view is "Students" or "Subjects"
        let activeView;

        if (window.location.pathname.includes("Students")) {
            activeView = "Students";
        } else if (window.location.pathname.includes("Subjects")) {
            activeView = "Subjects";
        } else if (window.location.pathname.includes("SubjectSchedules")) {
            activeView = "SubjectSchedules";
        }

        // Use 'id' for Students and 'code' for Subjects
        const id = $(this).data("id");

        // Perform the AJAX request to load the correct edit form based on the active view
        $.get("/" + activeView + "/Edit/" + id, function (data) {
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