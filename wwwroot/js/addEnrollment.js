let isHidden = true;
function toggleHiddenSection(show) {
    const hiddenSection = document.querySelector('.hidden-section');
    const studInfoBox = document.querySelector('.stud-info-box');
    if (show) {
        isHidden = false;
        hiddenSection.style.height = hiddenSection.scrollHeight + 'px';
        studInfoBox.style.transform = `translate(-50%,-55%)`;
    }
    else {
        isHidden = true;
        hiddenSection.style.height = 0;
        studInfoBox.style.transform = `translate(-50%,-69%)`;
    }
}

let isIdValidated = false;
function validateAndSubmit() {
    const idNumber = $('#idNumber').val();
    const loader = $('.loader');
    const idNumberMessage = $('#idNumberMessage');

    idNumberMessage.hide();
    loader.css('display', 'flex');

    $.ajax({
        url: '/Enrollment/ValidateStudentId', 
        type: 'GET',
        data: { Id: idNumber },
        success: function (response) {
            if (response.success) {
                isIdValidated = true; 

                $('#studentName').val(response.dataObject.name);
                $('#studentCourse').val(response.dataObject.course);
                $('#studentYear').val(response.dataObject.year);

                idNumberMessage.text(response.message).css('color', '#28a745');
                toggleHiddenSection(true); 
            } else {
                idNumberMessage.text(response.message).css('color', '#dc3545');
            }
        },
        error: function () {
            idNumberMessage.text('An error occurred while validating the ID.').css('color', '#dc3545');
        },
        complete: function () {
            loader.hide();
            idNumberMessage.show();
        }
    });
}
$(document).ready(function () {
    $('#idNumber').on('keydown', function (event) {
        if (event.key === 'Enter') {
            validateAndSubmit();
            event.preventDefault(); 
        }
    });
    $('#idNumber').on('input', function (event) {
        if (isIdValidated) {
            isIdValidated = false;
            toggleHiddenSection(false);
            $('#idNumberMessage').hide();

            $('#studentName').val("");
            $('#studentCourse').val("");
            $('#studentYear').val("");
        }
    });
});