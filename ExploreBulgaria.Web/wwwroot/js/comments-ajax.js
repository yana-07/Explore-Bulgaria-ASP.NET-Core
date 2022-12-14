const addComment = function (event) {
    event.preventDefault();
    const attractionId = $('#attractionId').val();
    const text = $('#commentText').val();
    const antiForgeryToken = $('input[name=__RequestVerificationToken]').val();

    fetch('/api/CommentsApi/add', {
        method: 'POST',
        headers: {
            'X-CSRF-TOKEN': antiForgeryToken,
            'content-type': 'application/json'
        },
        body: JSON.stringify({ attractionId, text })
    })
        .then(result => result.json())
        .then(data => {
            var element = document.createElement('div');
            element.setAttribute('class', 'card p-3 mt-2');
            element.innerHTML =
                `<div class="d-flex justify-content-between align-items-center">
                             <div class="user d-flex flex-row align-items-center">
                                 <img src="${data.addedByVisitor.userAvatarUrl}" class="user-img rounded-circle mr-2">
                                 <div class="row">
                                     <small class="font-weight-bold text-primary">${data.addedByVisitor.userFirstName} ${data.addedByVisitor.userLastName}</small>
                                     <small class="font-weight-bold text-secondary">${data.text}</small>
                                 </div>
                             </div>
                             <small class="text-secondary">${new Date(data.createdOn).toLocaleDateString()}</small>
                        </div>
                        <div class="action d-flex justify-content-between mt-2 align-items-center">
                             <div class="reply px-4">
                                 <small>Remove</small>
                             </div>
                             <input id="commentId" type="hidden" name="commentId" value="${data.id}">
                             <div class="d-flex flex-row align-items-center like">
                                 <i onclick="likeComment(event)" class="far fa-thumbs-up mx-2 fa-xs text-black"></i>
                                 <p class="small text-muted mb-0 likes">0</p>
                             </div>
                             <div class="d-flex flex-row align-items-center dislike">
                                 <i onclick="dislikeComment(event)" class="far fa-thumbs-down mx-2 fa-xs text-black"></i>
                                 <p class="small text-muted mb-0 dislikes">0</p>
                             </div>
                             <div class="d-flex flex-row align-items-center">
                                 <i onclick="appendReplySection(event)" class="far fa-comment mx-2 fa-xs text-black"></i>
                                 <p class="small text-muted mb-0 replies">0</p>
                             </div>
                             <div class="icons align-items-center">
                                 <i class="fa fa-check-circle-o check-icon text-primary"></i>
                             </div>
                         </div>`;

            document.getElementById('addComment').after(element);

            $('#commentText').val('');
        })
}

const antiForgeryToken = document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value;

const likeComment = function (event) {
    const parentDivEl = event.target.parentElement.parentElement;
    const commentId = parentDivEl.querySelector('input[name="commentId"]').value;

    fetch('/api/CommentsApi/like', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ commentId })
    })
        .then(result => result.json())
        .then(data => {
            event.target.parentElement.querySelector('.likes').textContent = data;

            const dislikeEl = parentDivEl.querySelector('.fa-thumbs-down');
            if (dislikeEl.classList.contains('disabled')) {
                dislikeEl.classList.remove('disabled');
            } else {
                dislikeEl.classList.add('disabled');
            }
        })
}

const dislikeComment = function (event) {
    const parentDivEl = event.target.parentElement.parentElement;
    const commentId = parentDivEl.querySelector('input[name="commentId"]').value;

    fetch('/api/CommentsApi/dislike', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ commentId })
    })
        .then(result => result.json())
        .then(data => {
            event.target.parentElement.querySelector('.dislikes').textContent = data;

            const likeEl = parentDivEl.querySelector('.fa-thumbs-up');
            if (likeEl.classList.contains('disabled')) {
                likeEl.classList.remove('disabled');
            } else {
                likeEl.classList.add('disabled');
            }
        })
}

const appendReplySection = function (event) {
    const parentDivEl = event.target.parentElement.parentElement.parentElement;
    const repliesContainerDiv = parentDivEl.querySelector('.replies-container');
    const addReplyDiv = parentDivEl.querySelector('.add-reply');
    const commentId = parentDivEl.querySelector('input[name="commentId"]').value;

    if (addReplyDiv || repliesContainerDiv.childElementCount != 0) {
        return;
    }

    fetch('/api/CommentsApi/getReplies', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ commentId })
    })
        .then(result => result.json())
        .then(data => {

            for (const reply of data) {
                const replyDivElement = document.createElement('div');
                replyDivElement.setAttribute('class', 'card p-4 mt-2 col-lg-11 offset-lg-1');
                replyDivElement.innerHTML = `
                            <div class="user d-flex flex-row align-items-center mb-3">
                                <img src="${reply.author.userAvatarUrl}" class="user-img rounded-circle mr-2">
                                <div class="row">
                                    <small class="font-weight-bold text-primary">${reply.author.userFirstName} ${reply.author.userLastName}</small>
                                    <small class="font-weight-bold text-secondary">${reply.text}</small>
                                </div>
                            </div>
                            <small class="text-secondary">${new Date(reply.createdOn).toLocaleDateString()}</small> `;

                repliesContainerDiv.appendChild(replyDivElement);
            }

            parentDivEl.appendChild(repliesContainerDiv);

            const divElement = document.createElement('div');
            divElement.setAttribute('class', 'card p-3 mt-2 add-reply');
            divElement.innerHTML = `
                             <form method="post">
                                 <input type="hidden" name="commentId" value="${commentId}" />
                                 <textarea class="text-secondary col-lg-9" placeholder="Напиши отговор..."></textarea>
                                 <button type="submit" onclick="addReply(event)" class="main-button col-lg-3" style="width: 115px; margin: 4px">Добави</button>
                             </form>`;

            parentDivEl.appendChild(divElement);
        })
}

const addReply = function (event) {
    event.preventDefault();
    const commentId = event.target.parentElement.querySelector('input[name="commentId"]').value;
    const textAreaElement = event.target.parentElement.querySelector('textarea');
    const replyText = textAreaElement.value;

    fetch('/api/CommentsApi/addReply', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ commentId, replyText })
    })
        .then(result => result.json())
        .then(data => {
            const divContainerElement = event.target.parentElement.parentElement.parentElement;
            const addReplyDivElement = divContainerElement.querySelector('div.add-reply');

            const newReplyElement = document.createElement('div');
            newReplyElement.setAttribute('class', 'card p-4 mt-2 col-lg-11 offset-lg-1');

            newReplyElement.innerHTML = `
                         <div class="user d-flex flex-row align-items-center">
                             <img src="${data.addedByVisitor.userAvatarUrl}" class="user-img rounded-circle mr-2">
                             <div class="row">
                                 <small class="font-weight-bold text-primary">${data.addedByVisitor.userFirstName} ${data.addedByVisitor.userLastName}</small>
                                 <small class="font-weight-bold text-secondary">${data.text}</small>
                             </div>
                         </div>
                         <small class="text-secondary">${new Date(data.createdOn).toLocaleDateString()}</small>`;

            divContainerElement.querySelector('div.replies-container').appendChild(newReplyElement);
            divContainerElement.querySelector('p.replies').textContent = data.repliesCount;
            textAreaElement.value = '';
        })
}