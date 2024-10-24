document.querySelectorAll('.submit-btn').forEach(button => {
    const svg = button.querySelector('svg');
    const span = button.querySelector('span');

    button.addEventListener('mouseover', () => {
        const buttonWidth = button.offsetWidth;
        const svgRect = svg.getBoundingClientRect();
        const svgWidth = svgRect.width;

        const translateX55Percent = -0.55 * svgWidth;

        const translateXCombined = (buttonWidth - svgWidth) / 2 + translateX55Percent;

        svg.style.transform = `translateX(${translateXCombined}px) rotate(45deg) scale(1.1)`;
    });

    button.addEventListener('mouseout', () => {
        svg.style.transform = ''; 
    });

    button.addEventListener('mousedown', () => {
        button.style.transform = 'translateX(-50%)'; // Reset on mousedown
    });
});

const subjContent = document.querySelector('.subj-content');
const subjContainer = document.querySelector('.subj-container');
const subjDotsIcon = document.querySelector('.subj-content .three-dots-icon');

const schedContent = document.querySelector('.sched-content');
const schedContainer = document.querySelector('.sched-container');
const schedDotsIcon = document.querySelector('.sched-content .three-dots-icon')

//const patternBg = document.querySelector('.pattern-bg');
const scheduleTransition = document.querySelector('.schedule-transition');
const subjectTransition = document.querySelector('.subj-transition');

let currentState;
let recentlyPressed = false;

document.querySelectorAll('.transition-btn').forEach(button => {
    function replaceAnimation(element, newAnimation) {
        const computedStyle = window.getComputedStyle(element);
        const finalTransform = computedStyle.transform;

        element.style.transform = finalTransform;

        element.style.animation = 'none'; 
        element.offsetHeight;
        element.style.animation = newAnimation;  
    }
    button.addEventListener('click', function () {
        if (currentState === "subject") {
            replaceAnimation(schedContent, 'slideRight 2s cubic-bezier(0.7, 0, 1, 1) reverse forwards');
            replaceAnimation(schedContainer, 'longSlideLeft 1.8s cubic-bezier(0.7, 0, 1, 1) reverse forwards');
            replaceAnimation(schedDotsIcon, 'longSlideLeft 1.5s cubic-bezier(0.7, 0, 1, 1) reverse forwards');

            replaceAnimation(subjContent, 'slideLeft 1.7s forwards');
            replaceAnimation(subjContainer, 'longSlideRight 1.6s forwards');
            replaceAnimation(subjDotsIcon, 'longSlideRight 1.3s forwards');

            scheduleTransition.style.transform = `translateX(-30vw)`;
            subjectTransition.style.transform = `translateX(0)`;

            currentState = "schedule";
        }
        else if (currentState === "schedule") {
            replaceAnimation(subjContent, 'slideLeft 2s cubic-bezier(0.7, 0, 1, 1) reverse forwards');
            replaceAnimation(subjContainer, 'longSlideRight 1.8s cubic-bezier(0.7, 0, 1, 1) reverse forwards');
            replaceAnimation(subjDotsIcon, 'longSlideRight 1.97s cubic-bezier(0.7, 0, 1, 1) reverse forwards');

            replaceAnimation(schedContent, 'slideRight 1.7s forwards');
            replaceAnimation(schedContainer, 'longSlideLeft 1.6s forwards');
            replaceAnimation(schedDotsIcon, 'longSlideLeft 1.3s forwards');

            subjectTransition.style.transform = `translateX(30vw)`;
            scheduleTransition.style.transform = `translateX(0)`;

            currentState = "subject";
        }
    });
});

$(function () {
    $("#SubjectCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/SubjectSchedules/GetSubjectCodes', // Action to get valid subject codes
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response(data); // Directly use the array of strings
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching subject codes:", status, error); // Log any errors
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            $("#SubjectCode").val(ui.item.value); // Set selected item
            return false;
        }
    });

    $("#SchedForm").submit(function (event) {
        const startTime = $("#StartTime").val();
        const endTime = $("#EndTime").val();
        const selectedXM = $("#XM").val(); 
        const maxSize = parseInt($("input[asp-for='MaxSize']").val(), 10);
        const classSize = parseInt($("input[asp-for='ClassSize']").val(), 10);

        if (classSize >= maxSize) {
            alertMessage = `Max Size must be more than Class Size!`;
            displayToast();
            event.preventDefault();
            return;
        }

        function getTimePeriod(time) {
            const [hours] = time.split(":").map(Number);
            return hours < 12 ? "AM" : "PM"; // Hours 0 to 11 are AM, 12 to 23 are PM
        }
        if (selectedXM && getTimePeriod(startTime) !== selectedXM) {
            alertMessage = `Start Time must match the selected XM period!`;
            displayToast();
            event.preventDefault();
            return;
        }

        if (startTime >= endTime) {
            alertMessage = "Start Time must be less than End Time!"
            displayToast();
            event.preventDefault(); 
            return; 
        }

        var subjectCode = $("#SubjectCode").val();
        $.ajax({
            url: '/SubjectSchedules/ValidateSubjectCode', // Action to validate the subject code
            type: "GET",
            data: { code: subjectCode },
            success: function (isValid) {
                if (!isValid) {
                    alertMessage = "Subject Code does not exist!"
                    displayToast();
                    event.preventDefault(); // Prevent form submission if invalid
                }
            },
            error: function (xhr, status, error) {
                console.error("Error validating subject code:", status, error); // Log any errors during validation
            },
            async: false // Consider switching to async: true for better performance
        });
    });
});

let alertMessage = document.getElementById('alertMessage').value;
function displayToast() {
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

let controllerName = document.getElementById('controllerName').value;
document.addEventListener("DOMContentLoaded", function () {
    const recentForm = document.getElementById('recentForm').value;
    if (recentForm)
        controllerName = recentForm;
    if (controllerName == 'Subjects') {
        subjContent.style.transform = `translateX(0)`;
        subjContent.style.display = 'block';
        schedContent.style.display = 'block';

        subjectTransition.style.transform = `translateX(30vw)`;
        subjectTransition.style.display = 'block';
        scheduleTransition.style.display = 'block';

        currentState = "subject";
    }
    else if (controllerName == 'SubjectSchedules') {
        schedContent.style.transform = `translateX(0)`;
        schedContent.style.display = 'block';
        subjContent.style.display = 'block';

        scheduleTransition.style.transform = `translateX(30vw)`;
        scheduleTransition.style.display = 'block';
        subjectTransition.style.display = 'block';

        currentState = "schedule";
    }

    displayToast();
});