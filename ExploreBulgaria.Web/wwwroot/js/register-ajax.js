let antiForgeryToken = document.querySelector('input[name=__RequestVerificationToken]').value;

let usernameInputElement = document.getElementById('username');
usernameInputElement.addEventListener('blur', () => {
    let userName = usernameInputElement.value;
    fetch('/api/UsersApi/username', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken
        },
        body: JSON.stringify({ userName })
    })
        .then(response => response.json())
        .then(exists => {
            if (exists) {
                document.getElementById('usernameErr')
                    .textContent = 'Потребителското име е заето.';
            }
        });
})

let emailInputElement = document.getElementById('email');
emailInputElement.addEventListener('blur', () => {
    let email = emailInputElement.value;
    fetch('/api/UsersApi/email', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken
        },
        body: JSON.stringify({ email })

    })
        .then(response => response.json())
        .then(exists => {
            if (exists) {
                document.getElementById('emailErr')
                    .textContent = 'Имейлът е зает.';
            }
        });
})