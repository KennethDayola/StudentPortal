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
    $('.search-input').on('input', function () {
        const searchValue = $(this).val().toLowerCase();
        if (searchValue === "") {
            $('.table tbody tr').show();
            return;
        }

        $('.table tbody tr').each(function () {
            const key = $(this).find('td:nth-child(2)').text().toLowerCase();
            if (key.startsWith(searchValue)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    $('.reset').on('click', function () {
        $('.search-input').val('');
        $('.table tbody tr').show();
    });

    $('[data-toggle="tooltip"]').tooltip();

    $(".dropdown-item").click(function (e) {
        e.preventDefault();

        $(this).closest(".dropdown").removeClass("show");

        let activeView;

        if (window.location.pathname.includes("Students")) {
            activeView = "Students";
        } else if (window.location.pathname.includes("Subjects")) {
            activeView = "Subjects";
        } else if (window.location.pathname.includes("SubjectSchedules")) {
            activeView = "SubjectSchedules";
        } else if (window.location.pathname.includes("Enrollment")) {
            activeView = "Enrollment";
        }

        const id = $(this).data("id");

        $.get("/" + activeView + "/Edit/" + id, function (data) {
            $("#editContent").html(data); 

            $("#editModal").show();
            $(".edit-modal-content").css("animation", "slideIn 0.5s forwards");
        });
    });

    $(".edit-modal-content .close").click(function () {
        $(".edit-modal-content").css("animation", "slideOut 0.75s forwards");

        setTimeout(function () {
            $("#editModal").hide();
        }, 750);
    });

    $(window).click(function (event) {
        if ($(event.target).is("#editModal")) {
            $(".edit-modal-content").css("animation", "slideOut 0.75s forwards");
            setTimeout(function () {
                $("#editModal").hide();
            }, 750);
        }
    });
});