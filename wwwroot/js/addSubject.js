const button = document.querySelector('.subj-btn');
const svg = document.querySelector('.subj-btn svg');
const span = button.querySelector('.subj-btn span');

button.addEventListener('mouseover', () => {
    const buttonWidth = button.offsetWidth;
    const svgRect = svg.getBoundingClientRect();
    const svgWidth = svgRect.width;

    const translateX55Percent = -0.55 * svgWidth;

    const translateXCombined = (buttonWidth - svgWidth) / 2 + translateX55Percent;

    svg.style.transform = `translateX(${translateXCombined}px) rotate(45deg) scale(1.1)`;
});

button.addEventListener('mouseout', () => {
    svg.style.transform = ''; // Reset transformation on mouse out
    span.style.transform = '';
});

const subjContent = document.querySelector('.subj-content');
const subjContainer = document.querySelector('.subj-container');
const dotsIcon = document.querySelector('.three-dots-icon');

const schedContent = document.querySelector('.sched-content');

document.querySelector('.transition-btn').addEventListener('click', function () {

    schedContent.style.animation = 'slideRight 2s cubic-bezier(0.7, 0, 1, 1)  reverse forwards';

    setTimeout(() => {
        subjContent.style.animation = 'slideLeft 1.7s  forwards';
        subjContainer.style.animation = 'longSlideRight 1.6s forwards';
        dotsIcon.style.animation = 'longSlideRight 1.6s forwards';
    }, 0);
});