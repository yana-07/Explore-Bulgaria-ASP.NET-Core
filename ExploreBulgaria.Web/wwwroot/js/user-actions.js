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
                if (event.target.classList.contains('fa-solid') &&
                    event.target.classList.contains('fill-red')) {
                    event.target.classList.remove('fa-solid');
                    event.target.classList.remove('fill-red');
                    event.target.classList.add('fa-regular');
                } else {
                    event.target.classList.add('fa-solid');
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
                if (event.target.classList.contains('fa-solid') &&
                    event.target.classList.contains('fill-green')) {
                    event.target.classList.remove('fa-solid');
                    event.target.classList.remove('fill-green');
                    event.target.classList.add('fa-regular');
                } else {
                    if (addToVisitedEl.classList.contains('fa-solid') &&
                        addToVisitedEl.classList.contains('fill-green')) {
                        addToVisitedEl.classList.remove('fa-solid');
                        addToVisitedEl.classList.remove('fill-green');
                        addToVisitedEl.classList.add('fa-regular');
                    }
                    event.target.classList.add('fa-solid');
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
                if (event.target.classList.contains('fa-solid') &&
                    event.target.classList.contains('fill-green')) {
                    event.target.classList.remove('fa-solid');
                    event.target.classList.remove('fill-green');
                    event.target.classList.add('fa-regular');
                } else {
                    if (wantToVisitEl.classList.contains('fa-solid') &&
                        wantToVisitEl.classList.contains('fill-green')) {
                        wantToVisitEl.classList.remove('fa-solid');
                        wantToVisitEl.classList.remove('fill-green');
                        wantToVisitEl.classList.add('fa-regular');
                    }
                    event.target.classList.add('fa-solid');
                    event.target.classList.add('fill-green');
                }
            })
            .catch(err => { })
    });
});