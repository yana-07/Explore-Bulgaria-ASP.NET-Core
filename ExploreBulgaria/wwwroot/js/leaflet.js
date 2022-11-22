const lat = Number(document.getElementById('latitude').value);
const lng = Number(document.getElementById('longitude').value);
var descriptionElement = document.getElementById('description');
var description = "";

const zoom = 13;

var map = L.map('map').setView([lat, lng], zoom);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

var marker = L.marker([lat, lng]).addTo(map);

if (descriptionElement) {
    description = descriptionElement.value;

    marker.bindPopup(`${description}...`).openPopup();;
}

