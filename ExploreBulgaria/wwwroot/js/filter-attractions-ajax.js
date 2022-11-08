const categorySelectEl = document.getElementById('chooseCategory');
const subcategorySelectEl = document.getElementById('chooseSubcategory');
const regionSelectEl = document.getElementById('chooseRegion');

categorySelectEl.addEventListener('change', () => {
    const categoryName = categorySelectEl.value;

    fetch("/api/FilterAttractions/subcategories", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
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

    fetch("/api/FilterAttractions/regions", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
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

    fetch("/api/FilterAttractions/regions", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
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
