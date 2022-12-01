const categorySelectEl = document.getElementById('chooseCategory');
const subcategorySelectEl = document.getElementById('chooseSubcategory');
const regionSelectEl = document.getElementById('chooseRegion');
const locationSelectEl = document.getElementById('chooseLocation');
const antiForgeryToken = document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value;

categorySelectEl.addEventListener('change', () => {
    const categoryName = categorySelectEl.value;

    fetch("/api/AttractionsApi/subcategories", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ categoryName })
    })
        .then(response => response.json())
        .then(subcategories => {
            subcategorySelectEl.innerHTML = '';
            subcategorySelectEl.appendChild(new Option(''));
            let childElements = subcategories.map(c => new Option(c.name));
            childElements.forEach(c => subcategorySelectEl.appendChild(c));
        })

    fetch("/api/AttractionsApi/regions", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ categoryName })
    })
        .then(response => response.json())
        .then(regions => {
            regionSelectEl.innerHTML = '';
            regionSelectEl.appendChild(new Option(''));
            let childElements = regions.map(r => new Option(r.name));
            childElements.forEach(c => regionSelectEl.appendChild(c));
        })

    fetch("/api/AttractionsApi/locations", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ categoryName })
    })
        .then(response => response.json())
        .then(locations => {
            locationSelectEl.innerHTML = '';
            locationSelectEl.appendChild(new Option(''));
            let childElements = locations.map(l => new Option(l.name));
            childElements.forEach(l => locationSelectEl.appendChild(l));
        })

})

subcategorySelectEl.addEventListener('change', () => {
    const categoryName = categorySelectEl.value;
    const subcategoryName = subcategorySelectEl.value;

    fetch("/api/AttractionsApi/regions", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ categoryName, subcategoryName })
    })
        .then(response => response.json())
        .then(regions => {
            regionSelectEl.innerHTML = '';
            regionSelectEl.appendChild(new Option(''));
            let childElements = regions.map(r => new Option(r.name));
            childElements.forEach(c => regionSelectEl.appendChild(c));
        })

    fetch("/api/AttractionsApi/locations", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ categoryName, subcategoryName })
    })
        .then(response => response.json())
        .then(locations => {
            locationSelectEl.innerHTML = '';
            locationSelectEl.appendChild(new Option(''));
            let childElements = locations.map(l => new Option(l.name));
            childElements.forEach(l => locationSelectEl.appendChild(l));
        })

})

regionSelectEl.addEventListener('change', () => {
    const regionName = regionSelectEl.value;
    const categoryName = categorySelectEl.value;
    const subcategoryName = subcategorySelectEl.value;

    fetch("/api/AttractionsApi/locations", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'X-CSRF-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify({ categoryName, subcategoryName, regionName })
    })
        .then(response => response.json())
        .then(locations => {
            locationSelectEl.innerHTML = '';
            locationSelectEl.appendChild(new Option(''));
            let childElements = locations.map(l => new Option(l.name));
            childElements.forEach(l => locationSelectEl.appendChild(l));
        })
})
