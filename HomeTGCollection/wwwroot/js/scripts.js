async function updateQuantity(id, delta, deltaFoil = 0) {
    fetch(`/UpdateQuantity?Id=${id}&deltaQuantity=${delta}&deltaFoilQuantity=${deltaFoil}`, {
        method: 'PUT',
    })
        .then(response => response.text())
        .then(html => {
            var cardDetails = document.querySelector('.card-info[id="details-' + id + '"]');
            cardDetails.innerHTML = html;
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
            // var cardDetails = document.querySelector('.card-info[id="details-' + id + '"]');
            // cardDetails.innerHTML = html;
            console.info(html);
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}
