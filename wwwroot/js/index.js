const track = document.getElementById("image-track");
let startMouseX = 0;
let startMouseY = 0;
let isDragging = false; // Flag to determine if dragging occurred
const dragThreshold = 5; // Minimum movement to consider a drag

const leftIcon = document.querySelector('.left-icon img');
const rightIcon = document.querySelector('.right-icon img');



window.onmousedown = e => {
    const trackDimensions = track.getBoundingClientRect();

    if (isInRect(e, trackDimensions)) {

        startMouseX = e.clientX;
        startMouseY = e.clientY;
        track.dataset.mouseDownAt = startMouseX;
        isDragging = false; 
    }
};

function isInRect(mouseEvent, rectangle) {
    return mouseEvent.clientX >= rectangle.left && mouseEvent.clientX <= rectangle.right &&
        mouseEvent.clientY >= rectangle.top && mouseEvent.clientY <= rectangle.bottom;
}

window.onmouseup = () => {
    if (isDragging) {
        track.dataset.mouseDownAt = "0";
        track.dataset.prevPercentage = track.dataset.percentage;
    }
    isDragging = false; // Reset dragging flag on mouse up
};

function animateTrack() {
    track.dataset.percentage = nextPercentage;

    // Use the mapped value for the animation
    const translateValue = mapRange(nextPercentage, 0, -100, -27, -73);

    track.animate({
        transform: `translate(${translateValue}%, -50%)`
    }, { duration: 1200, fill: "forwards" });

    for (const image of track.getElementsByClassName("image")) {
        image.animate({
            objectPosition: `${100 + nextPercentage}% center`
        }, { duration: 1200, fill: "forwards" });
    }
}

window.onmousemove = e => {
    if (track.dataset.mouseDownAt === "0") return;

    const mouseDeltaX = e.clientX - startMouseX;
    const mouseDeltaY = e.clientY - startMouseY;

    // Check if the mouse has moved beyond the threshold
    if (Math.abs(mouseDeltaX) > dragThreshold || Math.abs(mouseDeltaY) > dragThreshold) {
        isDragging = true;

        const mouseDelta = startMouseX - e.clientX;
        const maxDelta = window.innerWidth / 2;

        const percentage = (mouseDelta / maxDelta) * -100;
        const nextPercentageUnconstrained = parseFloat(track.dataset.prevPercentage) + percentage;
        const nextPercentage = Math.max(Math.min(nextPercentageUnconstrained, 0), -100);


        // Use the mapped value for the animation
        track.dataset.percentage = nextPercentage;

        // Use the mapped value for the animation
        const translateValue = mapRange(nextPercentage, 0, -100, -27, -73);

        track.animate({
            transform: `translate(${translateValue}%, -50%)`
        }, { duration: 1200, fill: "forwards" });

        for (const image of track.getElementsByClassName("image")) {
            image.animate({
                objectPosition: `${100 + nextPercentage}% center`
            }, { duration: 1200, fill: "forwards" });
        }
    }
};

// Helper function to map one range to another
const mapRange = (value, inMin, inMax, outMin, outMax) => {
    return ((value - inMin) * (outMax - outMin)) / (inMax - inMin) + outMin;
};

function updateImageSources() {
    const images = document.querySelectorAll('#image-track .image');
    images.forEach((img) => {
        const randomNumber = Math.floor(Math.random() * 230) + 1; // Random number between 1 and 300
        img.src = `https://picsum.photos/id/${randomNumber}/1920/1080`;
    });
}

// Call the function to update image sources when the page loads
updateImageSources();

document.addEventListener("DOMContentLoaded", function () {
    function setInitialSize() {
        var imageTrack = document.getElementById('image-track');
        var backgroundColor = document.querySelector('.background-color');

        // Get the initial bounding rectangle of the #image-track element
        var rect = imageTrack.getBoundingClientRect();

        // Set the dimensions and position of the .background-color element
        backgroundColor.style.top = (rect.top) + 'px';
        backgroundColor.style.left = (rect.left - 400) + 'px'; // Adjust position for padding
        backgroundColor.style.width = (rect.width + 400) + 'px'; // Adjust width for padding
        backgroundColor.style.height = (rect.height + 5) + 'px'; // Height remains the same
    }

    // Set initial size of the background color element
    setInitialSize();
});
