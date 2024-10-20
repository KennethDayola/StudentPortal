document.querySelectorAll('.subj-btn').forEach(button => {
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
});

const subjContent = document.querySelector('.subj-content');
const subjContainer = document.querySelector('.subj-container');
const subjDotsIcon = document.querySelector('.subj-content .three-dots-icon');

const schedContent = document.querySelector('.sched-content');
const schedContainer = document.querySelector('.sched-container');
const schedDotsIcon = document.querySelector('.sched-content .three-dots-icon')

const patternBg = document.querySelector('.pattern-bg');
const scheduleTransition = document.querySelector('.schedule-transition');
const subjectTransition = document.querySelector('.subj-transition');

let currentState = "schedule";
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
            console.log("Fetching subject codes for:", request.term); // Log the term being searched
            $.ajax({
                url: '/SubjectSchedules/GetSubjectCodes', // Action to get valid subject codes
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    console.log("Received subject codes:", data); // Log the received data
                    response(data); // Directly use the array of strings
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching subject codes:", status, error); // Log any errors
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            console.log("Selected subject code:", ui.item.value); // Log the selected item
            $("#SubjectCode").val(ui.item.value); // Set selected item
            return false;
        }
    });

    $("form").submit(function (event) {
        var subjectCode = $("#SubjectCode").val();
        console.log("Submitting form with subject code:", subjectCode); // Log the subject code being submitted
        $.ajax({
            url: '/SubjectSchedules/ValidateSubjectCode', // Action to validate the subject code
            type: "GET",
            data: { code: subjectCode },
            success: function (isValid) {
                console.log("Subject code validation result:", isValid); // Log validation result
                if (!isValid) {
                    event.preventDefault(); // Prevent form submission if invalid
                    alert("Invalid Subject Code");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error validating subject code:", status, error); // Log any errors during validation
            },
            async: false // Consider switching to async: true for better performance
        });
    });
});