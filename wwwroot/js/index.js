const track = document.getElementById("image-track");

let startMouseX = 0;
let startMouseY = 0;
let isDragging = false;
const dragThreshold = 5;

let isZoomed = false;
let clonedImage, clickedImgRect;
let checkedCheckbox = null;

let zoomedImages, currentImageIndex;

const trackDimensions = track.getBoundingClientRect();

const closeBtn = document.querySelector('.close-btn');
let closeBtnClone;

const leftIcon = document.querySelector('.left-icon');
const rightIcon = document.querySelector('.right-icon');
let leftIconZoomVer, rightIconZoomVer;

const leftIconRect = document.querySelector('.left-icon').getBoundingClientRect();
const rightIconRect = document.querySelector('.right-icon').getBoundingClientRect();

const fadeUpKeyFrames = [
    { transform: 'translateY(0)', opacity: 1 },
    { transform: 'translateY(-11%)', opacity: 0 },
];
const fadeUpOptions = {
    duration: 500,
    easing: 'ease',
    fill: 'forwards'
};

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
                isZoomed = true;

                zoomContainer = document.createElement('div');
                zoomContainer.classList.add('zoom-container');

                zoomContainer.style.textAlign = 'center';

                const zoomTextsClone = document.getElementById(zoomTextId).cloneNode(true);
                zoomTextsClone.style.display = 'block';

                leftIconZoomVer = leftIcon.cloneNode(true);
                rightIconZoomVer = rightIcon.cloneNode(true);

                leftIconZoomVer.style.position = 'fixed';
                rightIconZoomVer.style.position = 'fixed';

                leftIconZoomVer.style.zIndex = 1000000000;
                rightIconZoomVer.style.zIndex = 1000000000;

                leftIcon.style.display = 'none';
                rightIcon.style.display = 'none';

                setArrowDimensions(leftIconZoomVer, leftIconRect, 'scale(1.0)');
                setArrowDimensions(rightIconZoomVer, rightIconRect, 'scale(1.0)');

                closeBtnClone = closeBtn.cloneNode(true);
                closeBtnClone.style.display = 'block';

                closeBtnClone.addEventListener('click', function () {
                    zoomOut();
                });

                zoomContainer.appendChild(zoomTextsClone);
                zoomContainer.appendChild(leftIconZoomVer);
                zoomContainer.appendChild(rightIconZoomVer);
                zoomContainer.appendChild(closeBtnClone);

                document.body.appendChild(zoomContainer);

                document.body.style.overflowY = 'hidden';

                document.querySelectorAll('.image-checkbox + img').forEach(image => {
                    const clonedImage = image.cloneNode(true);
                    clonedImage.classList.add('zoomed-image');
                    zoomContainer.appendChild(clonedImage);
                });

                zoomedImages = document.querySelectorAll('.zoomed-image');

                currentImageIndex = Array.from(document.querySelectorAll('.image-checkbox')).findIndex(cb => cb === checkedCheckbox);

                if (currentImageIndex >= 0) {
                    currentImageIndex = currentImageIndex; 
                }

                function showImage(newImageIndex, direction) {
                    const currentImage = zoomedImages[currentImageIndex];
                    const nextImage = zoomedImages[newImageIndex];

                    currentImage.style.display = 'block';
                    nextImage.style.display = 'block';

                    if (direction === 'right') {
                        currentImage.style.animation = 'slideOutToLeft 0.5s forwards';
                        nextImage.style.animation = 'slideInFromRight 0.5s forwards';
                    } else if (direction === 'left') {
                        currentImage.style.animation = 'slideOutToRight 0.5s forwards';
                        nextImage.style.animation = 'slideInFromLeft 0.5s forwards';
                    }                    
                    currentImageIndex = newImageIndex;

                    zoomedImages.forEach((img, imgPosition) => {
                        if (imgPosition !== currentImageIndex && imgPosition !== newImageIndex) {
                            img.style.display = 'none'; 
                            img.style.animation = '';   
                        }
                    });
                }
                showImage(currentImageIndex);

                clickedImgRect = checkedCheckbox.nextElementSibling.getBoundingClientRect();
                zoomedImages[currentImageIndex].style.top = clickedImgRect.top + window.scrollY + 'px';
                zoomedImages[currentImageIndex].style.left = clickedImgRect.left + window.scrollX + 'px';
                zoomedImages[currentImageIndex].style.width = clickedImgRect.width + 'px';
                zoomedImages[currentImageIndex].style.height = clickedImgRect.height + 'px';
                
                leftIconZoomVer.addEventListener('click', () => {
                    if (currentImageIndex > 0) {
                        currentImageIndex--; // Decrease index only if it's greater than 0
                        showImage(currentImageIndex, 'left');
                    }
                });

                rightIconZoomVer.addEventListener('click', () => {
                    if (currentImageIndex < zoomedImages.length - 1) {
                        currentImageIndex++; // Increase index only if it's less than the last index
                        showImage(currentImageIndex, 'right');
                    }
                });

                setTimeout(() => {
                    closeBtnClone.style.opacity = 1;

                    zoomedImages.forEach(image => {
                        image.style.top = `${headerHeight + window.scrollY}px`;
                        image.style.left = 0;
                        image.style.width = '100vw';
                        image.style.height = `calc(100vh - ${headerHeight}px)`;
                    });

                    leftIconZoomVer.style.top = '50%';
                    leftIconZoomVer.style.transform = 'scale(1.15)';

                    rightIconZoomVer.style.top = '50%';
                    rightIconZoomVer.style.left = `calc(100% -  ${rightIconZoomVer.offsetWidth}px)`;
                    rightIconZoomVer.style.transform = 'scale(1.15)';

                    setTimeout(() => {
                        zoomedImages.forEach(image => {
                            image.style.position = 'fixed';
                            image.style.top = `${headerHeight}px`;
                        });
                    }, 500);
                }, 10);

                track.style.pointerEvents = "none";
            }
        }, timerCountdown);
    });
});


window.addEventListener('wheel', function (event) {
    if (isZoomed && event.deltaY > 0) {
        zoomOut();
    }
}, { passive: false });

function zoomOut() {
    const zoomTextsClone = zoomContainer.querySelector('div');

    zoomTextsClone.animate(fadeUpKeyFrames, fadeUpOptions);
    closeBtnClone.animate(fadeUpKeyFrames, fadeUpOptions);

    const recentImgRect = document.querySelectorAll('.image-checkbox')[currentImageIndex].nextElementSibling.getBoundingClientRect();

    zoomedImages[currentImageIndex].style.transition = "transform 0.5s ease, width 0.5s ease, height 0.5s ease, top 0.5s ease, left 0.5s ease";
    zoomedImages[currentImageIndex].style.position = 'absolute';
    zoomedImages[currentImageIndex].style.top = recentImgRect.top + window.scrollY + 'px';
    zoomedImages[currentImageIndex].style.left = recentImgRect.left + window.scrollX + 'px';
    zoomedImages[currentImageIndex].style.width = recentImgRect.width + 'px';
    zoomedImages[currentImageIndex].style.height = recentImgRect.height + 'px';

    setArrowDimensions(leftIconZoomVer, leftIconRect, 'scale(1.0)');
    setArrowDimensions(rightIconZoomVer, rightIconRect, 'scale(1.0)');

    setTimeout(() => {
        leftIcon.style.display = 'block';
        rightIcon.style.display = 'block';
        document.body.style.overflowY = 'auto';
        checkedCheckbox.checked = false;
        isZoomed = false;
        track.style.pointerEvents = "auto";
        track.dataset.mouseDownAt = "0";
        zoomContainer.remove();
        
    }, 510);
}

function setArrowDimensions(element, rect, scale) {
    element.style.top = `${rect.top + headerHeight}px`;
    element.style.left = `${rect.left}px`;
    element.style.width = `${rect.width}px`;
    element.style.height = `${rect.height}px`;
    element.style.transform = scale;
}

window.onmousemove = e => {
    if (track.dataset.mouseDownAt === "0" || isZoomed) return;

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
        return parseFloat(values[4]); 
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