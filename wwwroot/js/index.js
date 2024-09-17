const track = document.getElementById("image-track");
let startMouseX = 0;
let startMouseY = 0;
let isDragging = false; // Flag to determine if dragging occurred
const dragThreshold = 5; // Minimum movement to consider a drag

const leftIcon = document.querySelector('.left-icon img');
const rightIcon = document.querySelector('.right-icon img');

let zoomedImage = false;
let clonedImage, originalImgRect;
let checkedCheckbox = null; // Global variable to store the current checkbox


window.onmousedown = e => {
    const trackDimensions = track.getBoundingClientRect();

    if (isInRect(e, trackDimensions)) {

        startMouseX = e.clientX;
        startMouseY = e.clientY;
        track.dataset.mouseDownAt = startMouseX;
        
    }
    else
        isDragging = false; 

    
};

function isInRect(mouseEvent, rectangle) {
    return mouseEvent.clientX >= rectangle.left && mouseEvent.clientX <= rectangle.right &&
        mouseEvent.clientY >= rectangle.top && mouseEvent.clientY <= rectangle.bottom;
}

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
        // Uncheck all checkboxes and reset images first
        document.querySelectorAll('.image-checkbox').forEach(cb => cb.checked = false);
        checkbox.checked = true;  // Only keep the clicked checkbox checked

        // Store the checked checkbox in the global variable
        checkedCheckbox = checkbox;

        const existingZoomedImage = document.querySelector('.zoomed-image');
        if (existingZoomedImage) {
            existingZoomedImage.remove();  // Remove previous zoomed image
        }

        if (checkedCheckbox) {
            zoomedImage = true;
            const correspondingImage = checkedCheckbox.nextElementSibling; // Get the corresponding image
            clonedImage = correspondingImage.cloneNode(true); // Clone the image

            // Get the height of the header
            const headerElement = document.querySelector('header');
            const headerHeight = headerElement ? headerElement.offsetHeight : 0;

            originalImgRect = correspondingImage.getBoundingClientRect();

            clonedImage.style.position = 'absolute';
            clonedImage.style.top = originalImgRect.top + window.scrollY + 'px';
            clonedImage.style.left = originalImgRect.left + window.scrollX + 'px';
            clonedImage.style.width = originalImgRect.width + 'px';
            clonedImage.style.height = originalImgRect.height + 'px';
            clonedImage.style.objectFit = 'cover';
            clonedImage.style.zIndex = '100000';
            clonedImage.classList.add('zoomed-image');

            clonedImage.style.transition = 'top 0.5s ease, left 0.5s ease, width 0.5s ease, height 0.5s ease';

            document.body.appendChild(clonedImage);
            document.body.style.overflow = 'hidden';


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
            document.body.style.overflow = 'auto';
            clonedImage.remove();
            checkedCheckbox.checked = false; // Uncheck the checkbox
            zoomedImage = false;
            track.style.pointerEvents = "auto";
        }, 500);
    }
}, { passive: false })


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
    if (track.dataset.mouseDownAt === "0" || zoomedImage) return;

    const mouseDeltaX = e.clientX - startMouseX;
    const mouseDeltaY = e.clientY - startMouseY;

    // Check if the mouse has moved beyond the threshold
    if (Math.abs(mouseDeltaX) > dragThreshold || Math.abs(mouseDeltaY) > dragThreshold) {
        isDragging = true;

        document.querySelectorAll('input[type="checkbox"]').forEach(function (checkbox) {
            checkbox.disabled = true;
        });

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
    var rect = track.getBoundingClientRect();
    function setInitialSize() {
        var backgroundColor = document.querySelector('.background-color');

        // Set the dimensions and position of the .background-color element
        backgroundColor.style.top = (rect.top) + 'px';
        backgroundColor.style.left = (rect.left - 400) + 'px'; // Adjust position for padding
        backgroundColor.style.width = (rect.width + 400) + 'px'; // Adjust width for padding
        backgroundColor.style.height = (rect.height + 5) + 'px'; // Height remains the same
    }

    function setPositionContainer() {
        var container = document.querySelector('.container');

        container.style.marginTop = (rect.height + 50) + 'px';

    }

    setPositionContainer();

    // Set initial size of the background color element
    setInitialSize();

    
});
