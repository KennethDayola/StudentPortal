const track = document.getElementById("image-track");

window.onmousedown = e => {
    const trackDimensions = track.getBoundingClientRect();

    if (inTrackRect(e)) {
        track.dataset.mouseDownAt = e.clientX;
        document.body.style.userSelect = 'none';
        document.body.style.pointerEvents = 'none';
    }
}

function inTrackRect(e) {
    const trackDimensions = track.getBoundingClientRect();
    return e.clientX >= trackDimensions.left && e.clientX <= trackDimensions.right &&
        e.clientY >= trackDimensions.top && e.clientY <= trackDimensions.bottom;
}

window.onmouseup = () => {
    track.dataset.mouseDownAt = "0";
    track.dataset.prevPercentage = track.dataset.percentage;

    document.body.style.userSelect = '';
    document.body.style.pointerEvents = '';
}
window.onmousemove = e => {
    if (track.dataset.mouseDownAt === "0") return;



    const mouseDelta = parseFloat(track.dataset.mouseDownAt) - e.clientX,
        maxDelta = window.innerWidth / 2;

    const percentage = (mouseDelta / maxDelta) * -100,
        nextPercentageUnconstrained = parseFloat(track.dataset.prevPercentage) + percentage,
        nextPercentage = Math.max(Math.min(nextPercentageUnconstrained, 0), -100);

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
};

// Helper function to map one range to another
const mapRange = (value, inMin, inMax, outMin, outMax) => {
    return ((value - inMin) * (outMax - outMin)) / (inMax - inMin) + outMin;
};
