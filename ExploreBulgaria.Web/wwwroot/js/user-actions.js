$(function () {
    let attractionId = document.getElementById('attractionId').value;
    const addToFavEl = document.getElementById('addToFavorites');
    const wantToVisitEl = document.getElementById('wantToVisit');
    const addToVisitedEl = document.getElementById('addToVisited');

    addToFavEl.addEventListener('click', function (event) {
        fetch('/api/VisitorsApi/addToFavorites', {
            method: 'POST',
            headers: {
                'content-type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value
            },
            body: JSON.stringify({ attractionId })
        })
            .then(() => {
                if (event.target.classList.contains('fill-red')) {
                    event.target.classList.remove('fill-red')
                } else {
                    event.target.classList.add('fill-red');
                }
            })
            .catch(err => { })
    });

    wantToVisitEl.addEventListener('click', function (event) {
        fetch('/api/VisitorsApi/wantToVisit', {
            method: 'POST',
            headers: {
                'content-type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value
            },
            body: JSON.stringify({ attractionId })
        })
            .then(() => {
                if (event.target.classList.contains('fill-green')) {
                    event.target.classList.remove('fill-green')
                } else {
                    if (addToVisitedEl.classList.contains('fill-green')) {
                        addToVisitedEl.classList.remove('fill-green')
                    }
                    event.target.classList.add('fill-green');
                }
            })
            .catch(err => { })
    });

    addToVisitedEl.addEventListener('click', function (event) {
        fetch('/api/VisitorsApi/addToVisited', {
            method: 'POST',
            headers: {
                'content-type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value
            },
            body: JSON.stringify({ attractionId })
        })
            .then(() => {
                if (event.target.classList.contains('fill-green')) {
                    event.target.classList.remove('fill-green')
                } else {
                    if (wantToVisitEl.classList.contains('fill-green')) {
                        wantToVisitEl.classList.remove('fill-green')
                    }
                    event.target.classList.add('fill-green');
                }
            })
            .catch(err => { })
    });
});