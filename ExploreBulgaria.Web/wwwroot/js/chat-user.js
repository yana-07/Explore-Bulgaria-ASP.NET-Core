"use strict";

let sendBtn = document.getElementById('sendBtn');
sendBtn.disabled = true;

let connection = null;
let myUserIdentifier = document.getElementById('myUserIdentifier').value;

const setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl('/chatHub')
        .build();

    connection.on("ReceiveMessage", (user, userIdentifier, avatar, message, dateTime) => {
        let messagesDiv = document.querySelector('.direct-chat-messages');
        let messageDiv = document.createElement('div');
        messageDiv.classList.add('direct-chat-msg');
        if (myUserIdentifier == userIdentifier) {
            messageDiv.classList.add('right');
        }
        messageDiv.innerHTML = `
             <div class="direct-chat-infos clearfix">
                 <span class="direct-chat-name float-${myUserIdentifier == userIdentifier ? 'right' : 'left'} text-primary">${user}</span>
                 <span class="direct-chat-timestamp float-${myUserIdentifier == userIdentifier ? 'left' : 'right'}">${new Date(dateTime).toLocaleString()}</span>
             </div>
             <img class="direct-chat-img" src="${avatar}" alt="message user image">
             <div class="direct-chat-text">
                 ${message}
             </div>`;
        messagesDiv.appendChild(messageDiv);
    });

    connection.start()
        .then(() => sendBtn.disabled = false)
        .catch(err => console.error(err.toString()));
};

setupConnection();

sendBtn.addEventListener('click', (event) => {
    event.preventDefault();

    connection.invoke("JoinPrivateGroup", '').catch(err => {
        console.error(err.toString());
    });

    let messageInput = document.getElementById('message');
    let message = messageInput.value;

    connection.invoke("SendMessageToGroup", message, '')
       .then(() => messageInput.value = '')
       .catch(err => {
        return console.error(err.toString());
       });
});

document.getElementById('closeBtn').addEventListener('click', () => {
    let antiForgeryToken = document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value;
    let group = `private@${myUserIdentifier}`;
    fetch('/api/ChatApi/clearNotification', {
        method: 'POST',
        headers: {
            "Content-Type": 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken
        },
        body: JSON.stringify({ group })
    });

    document.getElementById('messagesContainer').remove();
    window.location.href = '/';
});
