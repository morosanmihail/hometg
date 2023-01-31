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
            // update offset to be, uh, correct
            listCards(0);
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

async function listCards(offset) {
    fetch(`/ListItems?offset=${offset}`, {
        method: 'GET',
    })
        .then(response => response.text())
        .then(html => {
            var contentDiv = document.querySelector('#main-content');
            contentDiv.innerHTML = html;
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}
