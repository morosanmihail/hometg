﻿async function updateQuantity(id, delta, deltaFoil = 0) {
    var collection = getCollection();
    fetch(`/${collection}/UpdateQuantity?Id=${id}&deltaQuantity=${delta}&deltaFoilQuantity=${deltaFoil}`, {
        method: 'PUT',
    })
        .then(response => response.text())
        .then(html => {
            var cardDetails = document.querySelectorAll('.card-info[data-id="details-' + id + '"]');
            cardDetails.forEach((div) => {
                div.innerHTML = html;
            });
            listCards();
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}

async function searchMTGDB() {
    var searchName = document.querySelector('#search-bar-name').value;
    var searchSet = document.querySelector('#search-bar-set').value;
    var searchArtist = document.querySelector('#search-bar-artist').value;
    var searchCollectorNumber = document.querySelector('#search-bar-collector-number').value;
    var searchText = document.querySelector('#search-bar-text').value;
    fetch(`/Search?Name=${searchName}&SetCode=${searchSet}&CollectorNumber=${searchCollectorNumber}&Artist=${searchArtist}&Text=${searchText}`, {
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

async function listCards(delta = 0, collection = "") {
    var collection = collection;
    if (collection == "") {
        collection = getCollection();
    }
    var offsetBase = 12;
    var currentPage = document.querySelector('#list-page').innerHTML;
    var newPage = Math.max(0, currentPage - 1 + delta);
    var offset = Math.max(0, newPage * offsetBase);
    fetch(`/${collection}/ListItems?offset=${offset}`, {
        method: 'GET',
    })
        .then(response => response.text())
        .then(html => {
            var contentDiv = document.querySelector('#main-content');
            contentDiv.innerHTML = html;

            resetFilters();

            document.querySelector('#list-page').innerHTML = newPage + 1;

            const nextURL = "/" + collection + "?offset=" + offset;
            window.history.pushState({}, '', nextURL);

            setCollection(collection);
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}

function resetFilters() {
    var options = {
        valueNames: ['name', 'setCode']
    };
    var cardList = new List('listjs-grid', options);
}

async function importCSV() {
    var filenameId = "test";
    fetch('/Main/ImportCSV', { method: 'POST' })
        .then(response => response.json())
        .then(taskId => {
            var progress = 0;
            var intervalId = setInterval(function () {
                // Get the current progress
                fetch('/ImportProgress?Filename=' + filenameId)
                    .then(response => response.json())
                    .then(currentProgress => {
                        progress = currentProgress.current * 100 / currentProgress.total;

                        // Update the progress in the HTML
                        var progressBar = document.querySelector('.progress-bar');
                        progressBar.style.width = progress + '%';
                        progressBar.ariaValueNow = progress;
                        progressBar.innerHTML = progress + '%';

                        // If the task is complete, clear the interval
                        if (progress === 100) {
                            clearInterval(intervalId);
                        }
                    })
                    .catch(error => {
                        console.error('An error occurred:', error);
                    });
            }, 1000);
        })
        .catch(error => {
            console.error('An error occurred:', error);
        });
}

function getCollection() {
    return window.activeCollection;
}

function resetCollection() {
    const pathname = window.location.pathname;
    const firstSlashIndex = pathname.indexOf('/');
    const questionMarkIndex = pathname.indexOf('?');
    let mySubstring;
    if (questionMarkIndex !== -1) {
        mySubstring = pathname.substring(firstSlashIndex + 1, questionMarkIndex);
    } else {
        mySubstring = pathname.substring(firstSlashIndex + 1);
    }
    if (mySubstring == "") {
        mySubstring = "Main"
    }

    setCollection(mySubstring);
}

function setCollection(collection) {
    window.activeCollection = collection;
}