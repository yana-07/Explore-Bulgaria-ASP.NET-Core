const categorySelectEl = document.getElementById('chooseCategory');
const subcategorySelectEl = document.getElementById('chooseSubcategory');
const regionSelectEl = document.getElementById('chooseRegion');
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
        .then(categories => {
            subcategorySelectEl.innerHTML = '';
            subcategorySelectEl.appendChild(new Option(''));
            var childElements = categories.map(c => new Option(c.name, c.id));
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
            var childElements = regions.map(r => new Option(r.name, r.id));
            childElements.forEach(c => regionSelectEl.appendChild(c));
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
            var childElements = regions.map(r => new Option(r.name, r.id));
            childElements.forEach(c => regionSelectEl.appendChild(c));
        })

})
