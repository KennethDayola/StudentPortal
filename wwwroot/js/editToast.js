function displayToast(alertMessage, reloadLocation) {
    if (alertMessage) {
        const toastMessageElement = document.getElementById('toastMessage');
        const toastTitleElement = document.getElementById('toastTitle');
        const toastSquare = document.getElementById('toastSquare');
        const formElement = document.getElementById('editForm');

        formElement.classList.add('disable-interaction');

        if (alertMessage.includes("successfully")) {
            toastSquare.style.backgroundColor = '#51d251';
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

        toastElement.addEventListener('hide.bs.toast', function () {
            formElement.classList.remove('disable-interaction');
            if (reloadLocation) window.location.href = reloadLocation;
        });
    }
}