async function updateQuantity(id, delta, deltaFoil = 0) {
    fetch(`/UpdateQuantity?Id=${id}&deltaQuantity=${delta}&deltaFoilQuantity=${deltaFoil}`, {
        method: 'PUT',
    })
        .then(response => response.text())
        .then(html => {
            var cardDetails = document.querySelectorAll('.card-info[data-id="details-' + id + '"]');
            cardDetails.forEach((div) => {
                div.innerHTML = html;
            });
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}

async function searchMTGDB() {
    var inputData = document.querySelector('#search-bar').value;
    fetch(`/Search?Name=${inputData}`, {
        method: 'GET',
    })
        .then(response => response.text())
        .then(html => {
            var searchDiv = document.querySelector('.search-results[id="search-results"]');
            searchDiv.innerHTML = html;
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}
