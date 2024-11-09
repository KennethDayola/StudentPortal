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
        validateIdAndSubmit();
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

let alertMessage = $('#alertMessage').val();
function displayToast(toastMessage) {
    if (toastMessage) {
        alertMessage = toastMessage;
    }
    if (alertMessage) {
        const toastMessageElement = document.getElementById('toastMessage');
        const toastTitleElement = document.getElementById('toastTitle');
        const toastSquare = document.getElementById('toastSquare');

        if (alertMessage.includes("successfully")) {
            toastTitleElement.innerText = "Complete!";
        } else {
            toastSquare.style.backgroundColor = '#d12525';
            toastTitleElement.innerText = "Error!";
        }

        toastMessageElement.innerText = alertMessage;

        const toastElement = document.getElementById('toast');
        toastElement.style.display = 'block';
        const toast = new bootstrap.Toast(toastElement);
        toast.show();
    }
}

let isIdValidated = false;
function validateIdAndSubmit() {
    const idNumber = $('#idNumber').val();
    const loader = $('.loader');
    const idNumberMessage = $('#idNumberMessage');

    $('#hiddenIdInput').val(idNumber);

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

function edpCodeAddition() {
    var edpCode = $("#edpCode").val().trim();

    $.ajax({
        url: '/Enrollment/ValidateEDPCode',
        data: { edpCode: edpCode },
        type: 'GET',
        success: function (response) {
            if (response.success) {
                var existsInTable = false;
                $(".enrollment-table tbody tr").each(function () {
                    var existingEDPCode = $(this).find("td:nth-child(1)").text().trim();
                    if (existingEDPCode === edpCode) {
                        existsInTable = true;
                        return false;
                    }
                });
                if (existsInTable) {
                    displayToast("EDP Code already in the list!");
                    return;
                }

                if (response.dataObject.subjectSchedule.classSize >= response.dataObject.subjectSchedule.maxSize) {
                    displayToast("EDP Code already at max capacity!");
                    return;
                }

                if (totalUnits + response.dataObject.units >= 27) {
                    displayToast("EDP Code will exceed max units of 27!");
                    return;
                }

                var formatTime = function (date) {
                    var d = new Date(date);
                    return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
                };

                var startTime = formatTime(response.dataObject.subjectSchedule.startTime);
                var endTime = formatTime(response.dataObject.subjectSchedule.endTime);
                var days = response.dataObject.subjectSchedule.days;

                const parseDays = (days) => {
                    const tCount = (days.match(/T/g) || []).length;
                    const hCount = (days.match(/H/g) || []).length;

                    if (tCount === 2 && hCount === 1) {
                        return ["T", "TH"]; 
                    } else if (tCount === 1 && hCount === 1) {
                        return ["TH"];
                    } else {
                        return [...days]; 
                    }
                };

                var dayArray = parseDays(days);

                var conflictDetected = false;
                $(".enrollment-table tbody tr").each(function () {
                    var existingDays = $(this).find("td:nth-child(5)").text().trim();
                    var existingDayArray = parseDays(existingDays);
                    var existingStartTime = $(this).find("td:nth-child(3)").text().trim();
                    var existingEndTime = $(this).find("td:nth-child(4)").text().trim();

                    if (dayArray.some(day => existingDayArray.includes(day))) {
                        if (timeOverlap(existingStartTime, existingEndTime, startTime, endTime)) {
                            conflictDetected = true;
                            return false;
                        }
                    }
                });

                if (conflictDetected) {
                    displayToast("Time conflict detected for the selected days.");
                    return;
                }

                addTableRow(response.dataObject, startTime, endTime, days);
                
            } else {
                displayToast(response.message);
            }
        },
        error: function () {
            displayToast("An error occurred while validating the EDP code.");
        }
    });
}

let rowIndex = 0;
function addTableRow(dataObject, startTime, endTime, days) {
    var newRow = `<tr>
                    <td>${dataObject.subjectSchedule.edpCode}</td>
                    <td>${dataObject.subjectSchedule.subjectCode}</td>
                    <td>${startTime}</td>
                    <td>${endTime}</td>
                    <td>${days}</td>
                    <td>${dataObject.subjectSchedule.room}</td>
                    <td>${dataObject.units}</td>
                    <td>
                        <button class="delete-btn"><svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#a34747" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"></polyline>
                        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>
                        </button>
                        <input type="hidden" name="EnrollmentDetails[${rowIndex}].EDPCode" value="${dataObject.subjectSchedule.edpCode}" />
                        <input type="hidden" name="EnrollmentDetails[${rowIndex}].SubjectCode" value="${dataObject.subjectSchedule.subjectCode}" />
                    </td>
                 </tr>`;
            
    // Append the new row to the table
    $(".enrollment-table tbody").append(newRow);
    $(".placeholder-row").hide();

    rowIndex++;
    $(".enrollment-table").on("click", ".delete-btn", function () {
        $(this).closest("tr").remove();
        rowIndex--;
    });
}

let totalUnits = 0;
function timeOverlap(startTime1, endTime1, startTime2, endTime2) {
    var parseTime = function (timeStr) {
        var time = timeStr.match(/(\d+):(\d+)\s*(AM|PM)/);
        var hours = parseInt(time[1], 10);
        var minutes = parseInt(time[2], 10);
        var period = time[3];

        if (period === "PM" && hours < 12) hours += 12;
        if (period === "AM" && hours === 12) hours = 0;

        return hours * 60 + minutes;
    };

    var start1 = parseTime(startTime1);
    var end1 = parseTime(endTime1);
    var start2 = parseTime(startTime2);
    var end2 = parseTime(endTime2);

    return (start1 < end2 && start2 < end1);
}

document.getElementById("EnrollmentForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let submitButton = document.querySelector("#enrollmentBtn");
    submitButton.disabled = true;

    if (totalUnits == 0) {
        displayToast("Please add subjects before submitting!");
        return; 
    }

    let enrollmentDetails = [];

    document.querySelectorAll(".enrollment-table tbody tr:not(.placeholder-row)").forEach((row) => {
        let edpCode = row.querySelector("input[name*='EDPCode']").value;
        let studId = document.getElementById("hiddenIdInput").value; // Assuming StudId is constant for all rows
        let subjCode = row.querySelector("input[name*='SubjectCode']").value;

        enrollmentDetails.push(edpCode, studId, subjCode);

        console.log("EDPCode:", edpCode + subjCode);
        console.log("StudId:", studId);
    });

    $.ajax({
        url: '/Enrollment/ValidateSubjectAdditions',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(enrollmentDetails),
        success: function (data) {
            if (!data.success) {
                displayToast(data.message); // Show duplicate message if found
                submitButton.disabled = false;
            } else {
                 $('#EnrollmentForm').submit();
            }
        },
        error: function () {
            console.error("Error occurred during the request.");
            submitButton.disabled = false;
        }
    });
});

$(document).ready(function () {
    displayToast();
    console.log($('#exceptionMsg').val());

    $('#idNumber').on('keydown', function (event) {
        if (event.key === 'Enter') {
            validateIdAndSubmit();
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

    $("#edpCode").on("keydown", function (e) {
        if (e.key === "Enter") {
            edpCodeAddition();
        }
    });

    var tableObserver = new MutationObserver(function () {
        totalUnits = $(".enrollment-table tbody tr").toArray().reduce(function (sum, row) {
            var units = $(row).find("td:nth-child(7)").text().trim();
            return sum + (parseInt(units) || 0); 
        }, 0);

        $("#hiddenUnitsInput").val(totalUnits);
        $("#unitsTextBox").val(totalUnits);
    });

    tableObserver.observe($(".enrollment-table tbody")[0], {
        childList: true,
    });
});