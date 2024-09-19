const track = document.getElementById("image-track");

let startMouseX = 0;
let startMouseY = 0;
let isDragging = false;
const dragThreshold = 5;

const leftIcon = document.querySelector('.left-icon img');
const rightIcon = document.querySelector('.right-icon img');

let zoomedImage = false;
let clonedImage, originalImgRect;
let checkedCheckbox = null;

const trackDimensions = track.getBoundingClientRect();
const leftDimensions = leftIcon.getBoundingClientRect();
const rightDimensions = rightIcon.getBoundingClientRect();

const headerElement = document.querySelector('header');
const headerHeight = headerElement ? headerElement.offsetHeight : 0;

window.onmousedown = e => {
    if (isInRect(e, leftDimensions)) {
        animateTrack(25, 600);
        track.dataset.mouseDownAt = "0";
        track.dataset.prevPercentage = track.dataset.percentage;
    }

    if (isInRect(e, rightDimensions)) {
        animateTrack(-25, 600);
        track.dataset.mouseDownAt = "0";
        track.dataset.prevPercentage = track.dataset.percentage;
    }

    if (isInRect(e, trackDimensions)) {
        startMouseX = e.clientX;
        startMouseY = e.clientY;
        track.dataset.mouseDownAt = startMouseX;
    }
    else
        isDragging = false;
};

window.onmouseup = () => {
    if (isDragging) {
        setTimeout(() => {
            document.querySelectorAll('input[type="checkbox"]').forEach(function (checkbox) {
                checkbox.disabled = false;
            });
        }, 200);
        track.dataset.mouseDownAt = "0";
        track.dataset.prevPercentage = track.dataset.percentage;
    }
    isDragging = false;
};

document.querySelectorAll('.image-checkbox').forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        let timerCountdown = 0;
        if (window.pageYOffset != 0) {
            timerCountdown = 500;
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        }
        setTimeout(() => {
            document.querySelectorAll('.image-checkbox').forEach(cb => cb.checked = false);
            checkbox.checked = true;

            checkedCheckbox = checkbox;

            const existingZoomedImage = document.querySelector('.zoomed-image');
            if (existingZoomedImage) {
                existingZoomedImage.remove();
            }

            if (checkedCheckbox) {
                zoomedImage = true;
                const correspondingImage = checkedCheckbox.nextElementSibling;
                clonedImage = correspondingImage.cloneNode(true);

                originalImgRect = correspondingImage.getBoundingClientRect();

                clonedImage.style.position = 'absolute';
                clonedImage.style.top = originalImgRect.top + window.scrollY + 'px';
                clonedImage.style.left = originalImgRect.left + window.scrollX + 'px';
                clonedImage.style.width = originalImgRect.width + 'px';
                clonedImage.style.height = originalImgRect.height + 'px';
                clonedImage.style.objectFit = 'cover';
                clonedImage.style.zIndex = '100000';
                clonedImage.style.filter = 'brightness(80%)';
                clonedImage.classList.add('zoomed-image');

                clonedImage.style.transition = 'top 0.5s ease, left 0.5s ease, width 0.5s ease, height 0.5s ease';

                document.body.appendChild(clonedImage);
                document.body.style.overflowY = 'hidden';


                setTimeout(() => {
                    clonedImage.style.top = `${headerHeight + window.scrollY}px`;
                    clonedImage.style.left = 0;
                    clonedImage.style.width = '100vw';
                    clonedImage.style.height = `calc(100vh - ${headerHeight}px)`;

                    setTimeout(() => {
                        clonedImage.style.position = 'fixed';
                        clonedImage.style.top = `${headerHeight}px`;
                    }, 500);
                }, 10);

                track.style.pointerEvents = "none";
            }
        }, timerCountdown);
    });
});

window.addEventListener('wheel', function (event) {
    if (zoomedImage && event.deltaY > 0) {
        clonedImage.style.position = 'absolute';
        clonedImage.style.top = originalImgRect.top + window.scrollY + 'px';
        clonedImage.style.left = originalImgRect.left + window.scrollX + 'px';
        clonedImage.style.width = originalImgRect.width + 'px';
        clonedImage.style.height = originalImgRect.height + 'px';


        setTimeout(() => {
            document.body.style.overflowY = 'auto';
            clonedImage.remove();
            checkedCheckbox.checked = false;
            zoomedImage = false;
            track.style.pointerEvents = "auto";
            track.dataset.mouseDownAt = "0";
        }, 500);
    }
}, { passive: false })


window.onmousemove = e => {
    if (track.dataset.mouseDownAt === "0" || zoomedImage) return;

    const mouseDeltaX = e.clientX - startMouseX;
    const mouseDeltaY = e.clientY - startMouseY;

    if (Math.abs(mouseDeltaX) > dragThreshold || Math.abs(mouseDeltaY) > dragThreshold) {
        isDragging = true;

        document.querySelectorAll('input[type="checkbox"]').forEach(function (checkbox) {
            checkbox.disabled = true;
        });

        const mouseDelta = startMouseX - e.clientX;
        const maxDelta = window.innerWidth / 2;

        const percentage = (mouseDelta / maxDelta) * -100;

        animateTrack(percentage, 1200);
    }
};

document.querySelector(".intro-btn1").addEventListener("click", function () {
    const targetSection = document.querySelector(".featured-section");
    const scrollToPosition = targetSection.getBoundingClientRect().top + window.scrollY - headerHeight;
    window.scrollTo({
        top: scrollToPosition,
        behavior: 'smooth'
    });
});

document.querySelector(".intro-btn2").addEventListener("click", function () {
    const firstCheckbox = document.querySelector('#image-track .image-checkbox');
    const transformValue = getTransformX(track);

    let duration = 0;

    if (firstCheckbox) {
        if (transformValue != 0) {
            animateTrack(100, 500)
            duration = 500;
        }

        setTimeout(() => {
            firstCheckbox.checked = true;
            firstCheckbox.dispatchEvent(new Event('change'));
        }, duration);
    }
});

function getTransformX(element) {
    const transform = window.getComputedStyle(element).transform;
    if (transform !== 'none') {
        const values = transform.split('(')[1].split(')')[0].split(',');
        return parseFloat(values[4]); // Extract the X value from the matrix
    }
    return 0;
}

function animateTrack(percentage, duration) {
    const nextPercentageUnconstrained = parseFloat(track.dataset.prevPercentage) + percentage;
    const nextPercentage = Math.max(Math.min(nextPercentageUnconstrained, 0), -100);

    track.dataset.percentage = nextPercentage;

    const translateValue = mapRange(nextPercentage, 0, -100, -27, -73);

    track.animate({
        transform: `translate(${translateValue}%, 0)`
    }, { duration: duration, fill: "forwards" });

    for (const image of track.getElementsByClassName("image")) {
        image.animate({
            objectPosition: `${100 + nextPercentage}% center`
        }, { duration: duration, fill: "forwards" });
    }
}

const mapRange = (value, inMin, inMax, outMin, outMax) => {
    return ((value - inMin) * (outMax - outMin)) / (inMax - inMin) + outMin;
};

function isInRect(mouseEvent, rectangle) {
    return mouseEvent.clientX >= rectangle.left && mouseEvent.clientX <= rectangle.right &&
        mouseEvent.clientY >= rectangle.top && mouseEvent.clientY <= rectangle.bottom;
}

//document.addEventListener("DOMContentLoaded", function () {
//    function updateImageSources() {
//        const images = document.querySelectorAll('#image-track .image');

//        images.forEach((img) => {
//            const randomNumber = Math.floor(Math.random() * 230) + 1; // Random number between 1 and 230
//            img.src = `https://picsum.photos/id/${randomNumber}/1920/1080`;
//        });
//    }

//    updateImageSources();
//});