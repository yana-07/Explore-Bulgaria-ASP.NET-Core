const postVote = function (event) {
    if (!event.target.hasAttribute('data-vote')) {
        return;
    }

    const value = event.target.dataset.vote;
    const antiForgeryToken = document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value;
    const attractionId = document.getElementById('attractionId').value;

    fetch('/api/Votes/postVote', {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-CSRF-TOKEN": antiForgeryToken
        },
        body: JSON.stringify({ attractionId, value })
    })
        .then(result => result.json())
        .then(data => {
            var voteElement = document.getElementById("averageVoteValue");
            if (Number.isInteger(data)) {
                voteElement.textContent = `${data}.0`;
            } else {
                voteElement.textContent = data;
            }
            
            var liElements = document.querySelectorAll(".item-rating li");

            for (let i = 0; i < liElements.length - 1; i++) {
                const iElement = liElements[i].querySelector("i");
                if (data >= iElement.dataset.vote) {
                    if (liElements[i].classList.contains('star-empty')) {
                        liElements[i].classList.remove('star-empty');
                    }
                    if (!liElements[i].classList.contains('star-fill')) {
                        liElements[i].classList.add('star-fill');
                    }
                } else {
                    if (liElements[i].classList.contains('star-fill')) {
                        liElements[i].classList.remove('star-fill');
                    }
                    if (!liElements[i].classList.contains('star-empty')) {
                        liElements[i].classList.add('star-empty');
                    }
                }
            }
        })
}