function revealHiddenSection() {
    const hiddenSection = document.querySelector('.hidden-section');
    hiddenSection.style.height = hiddenSection.scrollHeight + 'px';

    const studInfoBox = document.querySelector('.stud-info-box');
    studInfoBox.style.transform = `translate(-50%,-55%)`;
}

$(document).ready(function () {
    $('#idNumber').on('input', function () {
        const idNumber = $(this).val();

        $.ajax({
            url: '/Enrollment/ValidateStudentId',
            type: 'GET',
            data: { Id: idNumber },
            success: function (response) {
                if (response.success) {
                    $('#idNumberMessage').text(response.message)
                        .css('color', '#28a745'); // Success color
                } else {
                    // ID number is invalid
                    $('#idNumberMessage').text(response.message)
                        .css('color', '#dc3545'); // Error color
                }
            },
            error: function () {
                // AJAX request failed
                $('#idNumberMessage').text('An error occurred while validating the ID.')
                    .css('color', '#dc3545'); // Error color
            }
        });
    });
});