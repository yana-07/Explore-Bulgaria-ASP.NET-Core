$(function () {
    const categorySelectEl = document.getElementById('chooseCategory');
    const subcategorySelectEl = document.getElementById('chooseSubcategory');
    const regionSelectEl = document.getElementById('chooseRegion');
    const villageSelectEl = document.getElementById('chooseVillage');
    const antiForgeryToken = document.querySelector('#antiForgeryForm input[name=__RequestVerificationToken]').value;

    let categoryName = categorySelectEl.value;
    let subcategoryName = subcategorySelectEl.value;
    let regionName = regionSelectEl.value;

    //getSubcategories(null, categoryName);
    //getRegions(null, categoryName, subcategoryName);
    //getLocations(null, categoryName, subcategoryName, regionName);

    categorySelectEl.addEventListener('change', (event) => {
        categoryName = categorySelectEl.value;

        getSubcategories(event, categoryName);
        getRegions(event, categoryName);
        getLocations(event, categoryName);
    })

    subcategorySelectEl.addEventListener('change', (event) => {
        categoryName = categorySelectEl.value;
        subcategoryName = subcategorySelectEl.value;

        getRegions(event, categoryName, subcategoryName);
        getLocations(event, categoryName, subcategoryName);
    })

    regionSelectEl.addEventListener('change', (event) => {
        regionName = regionSelectEl.value;
        categoryName = categorySelectEl.value;
        subcategoryName = subcategorySelectEl.value;

        getLocations(event, categoryName, subcategoryName, regionName);
    })

    function getSubcategories(event, categoryName) {
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
    }

    function getRegions(event, categoryName, subcategoryName) {
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
    }

    function getLocations(event, categoryName, subcategoryName, regionName) {
        fetch("/api/AttractionsApi/villages", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'X-CSRF-TOKEN': antiForgeryToken,
            },
            body: JSON.stringify({ categoryName, subcategoryName, regionName })
        })
            .then(response => response.json())
            .then(locations => {
                villageSelectEl.innerHTML = '';
                villageSelectEl.appendChild(new Option(''));

                let childElements = locations.map(l => new Option(l.name));
                childElements.forEach(l => villageSelectEl.appendChild(l));
            })
    }   
})