const track = document.getElementById("image-track");

let startMouseX = 0;
let startMouseY = 0;
let isDragging = false;
const dragThreshold = 5;

let zoomedImage = false;
let clonedImage, originalImgRect;
let checkedCheckbox = null;

const trackDimensions = track.getBoundingClientRect();

const headerElement = document.querySelector('header');
const headerHeight = headerElement ? headerElement.offsetHeight : 0;

let zoomContainer;

window.onmousedown = e => {
    if (isInRect(e, trackDimensions)) {
        startMouseX = e.clientX;
        startMouseY = e.clientY;
        track.dataset.mouseDownAt = startMouseX;
    }
    else
        isDragging = false;
};

window.onload = () => {
    //document.getElementById('loading-screen').style.display = 'none';
    const startX = trackDimensions.left + 100;  
    const endX = startX - 10;  

    const mouseDownEvent = new MouseEvent('mousedown', {
        clientX: startX,
        clientY: trackDimensions.top + 10, 
        bubbles: true
    });
    window.dispatchEvent(mouseDownEvent);

    setTimeout(() => {
        const mouseMoveEvent = new MouseEvent('mousemove', {
            clientX: endX, 
            clientY: trackDimensions.top + 10, 
            bubbles: true
        });
        window.dispatchEvent(mouseMoveEvent);

        const mouseUpEvent = new MouseEvent('mouseup', {
            clientX: endX,
            clientY: trackDimensions.top + 10,
            bubbles: true
        });
        window.dispatchEvent(mouseUpEvent);
    }, 150);  
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

document.getElementById('left-icon-checkbox').addEventListener('change', function () {
    animateTrack(40, 600, 'cubic-bezier(0.25, 1, 0.5, 1)');
    track.dataset.mouseDownAt = "0";
    track.dataset.prevPercentage = track.dataset.percentage;
});

document.getElementById('right-icon-checkbox').addEventListener('change', function () {
    animateTrack(-45, 600, 'cubic-bezier(0.25, 1, 0.5, 1)');
    track.dataset.mouseDownAt = "0";
    track.dataset.prevPercentage = track.dataset.percentage;
});

document.querySelectorAll('.image-checkbox').forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        const zoomTextId = this.getAttribute('data-zoom-text');
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

                zoomContainer = document.createElement('div');
                zoomContainer.classList.add('zoom-container');

                const correspondingImage = checkedCheckbox.nextElementSibling;
                clonedImage = correspondingImage.cloneNode(true);
                originalImgRect = correspondingImage.getBoundingClientRect();

                clonedImage.classList.add('zoomed-image');
                clonedImage.style.top = originalImgRect.top + window.scrollY + 'px';
                clonedImage.style.left = originalImgRect.left + window.scrollX + 'px';
                clonedImage.style.width = originalImgRect.width + 'px';
                clonedImage.style.height = originalImgRect.height + 'px';

                const zoomTextsClone = document.getElementById(zoomTextId).cloneNode(true);
                zoomTextsClone.style.visibility = 'visible';

                zoomContainer.appendChild(clonedImage);
                zoomContainer.appendChild(zoomTextsClone);

                document.body.appendChild(zoomContainer);

                
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
        // Reverse animation for the zoomed text
        const zoomTextsClone = zoomContainer.querySelector('div');
        if (zoomTextsClone) {
            const keyframes = [
                { transform: 'translateY(0)', opacity: 1 }, // Starting point (visible and in place)
                { transform: 'translateY(20%)', opacity: 0 },
            ];
            const options = {
                duration: 500, // 500ms for the transition
                easing: 'ease', // Smooth easing
                fill: 'forwards' // Maintain the final state (opacity 0 and moved) after the animation completes
            };

            // Apply the animation
            zoomTextsClone.animate(keyframes, options);

        }
        // Animate the image back to its original position and size
        clonedImage.style.transition = "transform 0.5s ease, width 0.5s ease, height 0.5s ease, top 0.5s ease, left 0.5s ease";
        clonedImage.style.position = 'absolute';
        clonedImage.style.top = originalImgRect.top + window.scrollY + 'px';
        clonedImage.style.left = originalImgRect.left + window.scrollX + 'px';
        clonedImage.style.width = originalImgRect.width + 'px';
        clonedImage.style.height = originalImgRect.height + 'px';

        // Wait for the reverse animation to complete before removing the zoomContainer
        setTimeout(() => {
            zoomTextsClone.remove();
            if (zoomContainer && zoomContainer.parentElement) {
                zoomContainer.remove();
            }
            document.body.style.overflowY = 'auto';
            checkedCheckbox.checked = false;
            zoomedImage = false;
            track.style.pointerEvents = "auto";
            track.dataset.mouseDownAt = "0";
        }, 510); // Ensure this timeout matches the reverse animation duration
    }
}, { passive: false });


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
            animateTrack(100, 350)
            duration = 350;
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

function animateTrack(percentage, duration, easing = null) {
    const nextPercentageUnconstrained = parseFloat(track.dataset.prevPercentage) + percentage;
    const nextPercentage = Math.max(Math.min(nextPercentageUnconstrained, 0), -100);

    track.dataset.percentage = nextPercentage;

    const translateValue = mapRange(nextPercentage, 0, -100, -27, -73);

    const animationOptions = {
        duration: duration,
        fill: "forwards"
    };

    if (easing) {
        animationOptions.easing = easing; 
    }

    track.animate({
        transform: `translate(${translateValue}%, 0)`
    }, animationOptions);

    for (const image of track.getElementsByClassName("image")) {
        image.animate({
            objectPosition: `${100 + nextPercentage}% center`
        }, animationOptions);
    }
}

const mapRange = (value, inMin, inMax, outMin, outMax) => {
    return ((value - inMin) * (outMax - outMin)) / (inMax - inMin) + outMin;
};

function isInRect(mouseEvent, rectangle) {
    return mouseEvent.clientX >= rectangle.left && mouseEvent.clientX <= rectangle.right &&
        mouseEvent.clientY >= rectangle.top && mouseEvent.clientY <= rectangle.bottom;
}