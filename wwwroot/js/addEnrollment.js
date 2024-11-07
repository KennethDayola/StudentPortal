let isHidden = true;
function toggleHiddenSection(show) {
    const hiddenSection = document.querySelector('.hidden-section');
    const studInfoBox = document.querySelector('.stud-info-box');
    if (show) {
        isHidden = false;
        hiddenSection.style.height = hiddenSection.scrollHeight + 'px';
        studInfoBox.style.top = '7rem';
    }
    else {
        isHidden = true;
        hiddenSection.style.height = 0;
        studInfoBox.style.top = '11rem';
    }
}

let confirmedStudInfo = false;
function studButtonFunction() {
    const studInfoWrapper = $('.stud-info-wrapper');
    const studInfoBox = $('.stud-info-box');
    const enrollmentBox = $('.enrollment-box');

    if (!isIdValidated) {
        validateAndSubmit();
    }
    else if (confirmedStudInfo) {
        toggleButtonDesign("Transition");
        studInfoWrapper.toggleClass('less-opacity');
        $('#idNumber').prop('readonly', false);

        studInfoBox.css({
            'left': '50%',
            'transform': 'translateX(-50%)'
        });

        enrollmentBox.css({
            'transform': 'translateX(110%)'
        });
    }
    else {
        studInfoWrapper.toggleClass('less-opacity');

        studInfoBox.css({
            'left': '3vw',
            'transform': 'translateX(0)'
        });

        enrollmentBox.css({
            'display': 'block',
            'transform': 'translateX(0)'
        });

        toggleButtonDesign("Go Back");

        $('#idNumber').prop('readonly', true);
    }
}

function toggleButtonDesign(task) {
    const studInfoBtn = $('#studInfoBtn');
    const leftSvg = $('.left-svg');
    const studBtnSpan = $('#studBtnSpan');
    const proceedLabel = $('.proceed-label');

    if (task === "Find Id") {
        studInfoBtn.removeClass('transition-state');
        studBtnSpan.removeClass('transition-span');

        leftSvg.css({
            'display': 'none',
            'transform': 'translate(300px, -50%)' 
        });
    }
    else if (task === "Transition") {
        studInfoBtn.addClass('transition-state');
        studBtnSpan.addClass('transition-span');

        setTimeout(function () {
            leftSvg.css({
                'display': 'block',
                'transform': 'translate(0, -50%)'  
            });
        }, 500);  

        const btnOffset = studInfoBtn.offset();    
        const btnWidth = studInfoBtn.outerWidth(); 

        if (confirmedStudInfo) {
            proceedLabel.removeClass('fade-in').addClass('fade-out');
            setTimeout(function () {
                proceedLabel.text('Proceed');  // Change the text
                proceedLabel.removeClass('fade-out').addClass('fade-in');
            }, 600);

            confirmedStudInfo = false;
        }
        else {
            proceedLabel.css({
                'display': 'block',
            });
        }
    }
    else if (task == "Go Back") {
        leftSvg.css({
            'transform': 'translate(0, -50%) scaleX(-1)'  
        });

        proceedLabel.removeClass('fade-in').addClass('fade-out');
        setTimeout(function () {
            proceedLabel.text('Go Back');  
            proceedLabel.removeClass('fade-out').addClass('fade-in');
        }, 400);

        confirmedStudInfo = true;
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
                toggleButtonDesign("Transition");
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
            toggleButtonDesign("Find Id");
            $('#idNumberMessage').hide();

            $('#studentName').val("");
            $('#studentCourse').val("");
            $('#studentYear').val("");
        }
    });
});