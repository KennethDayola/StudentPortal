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
        // Get the computed transform style after the first animation ends
        const computedStyle = window.getComputedStyle(element);
        const finalTransform = computedStyle.transform;

        // Apply the final transform as an inline style
        element.style.transform = finalTransform;

        // Re-trigger the animation by resetting the style and applying the new animation
        element.style.animation = 'none';  // Temporarily remove the animation
        element.offsetHeight;  // Force a reflow
        element.style.animation = newAnimation;  // Apply the new animation
    }

    // Example usage:
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