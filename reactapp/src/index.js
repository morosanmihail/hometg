import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import CardList from './Components/CardList'

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
  <header>
        <nav id="sidebarMenu"
             class="collapse d-lg-block sidebar collapse bg-white">
            <div class="position-sticky">
                <div class="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                    <button class="nav-link" id="nav-search-tab" data-bs-toggle="tab" data-bs-target="#nav-search" type="button" role="tab" aria-controls="nav-search" aria-selected="true">Search</button>

                        <button class="nav-link active" id="nav-Main-tab" data-bs-toggle="tab" data-bs-target="#nav-main" type="button" role="tab" aria-controls="nav-main" aria-selected="true" onclick="">Button</button>               
                </div>
            </div>
        </nav>
        <nav id="main-navbar" class="navbar navbar-expand-lg navbar-light bg-white fixed-top">
            <div class="container-fluid">
                <button class="navbar-toggler"
                        type="button"
                        data-mdb-toggle="collapse"
                        data-mdb-target="#sidebarMenu"
                        aria-controls="sidebarMenu"
                        aria-expanded="false"
                        aria-label="Toggle navigation">
                    <i class="fas fa-bars">=</i>
                </button>

                <a class="navbar-brand" href="#">
                    HomeTG
                </a>

                <div class="progress col">
                    <div title="import-progress" class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
                <button class="button" onclick="importCSV()">Test Import</button>

            </div>
        </nav>
        </header>
        <main>
            <div class="container pt-4 tab-content">
                <div class="tab-pane fade show active" id="nav-main" role="tabpanel" aria-labelledby="nav-main-tab">
                    <div id="main-content">
                        <CardList />
                    </div>
                    <nav aria-label="Page navigation">
                        <ul class="pagination center">
                            <li class="page-item"><a class="page-link" onclick="listCards(-1)" href="#">Previous</a></li>
                            <li class="page-item"><a id="list-page" class="page-link">1</a></li>
                            <li class="page-item"><a class="page-link" onclick="listCards(1)" href="#">Next</a></li>
                        </ul>
                    </nav>
                </div>

                <div class="tab-pane fade" id="nav-search" role="tabpanel" aria-labelledby="nav-search-tab">
                    <h2>Search</h2>
                    <div class="list-group list-group-flush mx-3 mt-4">
                        <div class="input-group mb-3">
                            <input type="text" class="form-control" id="search-bar-name" placeholder="Name" aria-describedby="button-addon2"/>
                            <input type="text" class="form-control" id="search-bar-set" placeholder="Set" aria-describedby="button-addon2"/>
                        </div>
                                <div class="input-group mb-3">
                                    <input type="text" class="form-control" id="search-bar-artist" placeholder="Artist" aria-describedby="button-addon2"/>
                                        <input type="text" class="form-control" id="search-bar-collector-number" placeholder="Collector Number" aria-describedby="button-addon2"/>
                                        </div>
                                        <div class="input-group mb-3">
                                            <input type="text" class="form-control" id="search-bar-text" placeholder="Text" aria-describedby="button-addon2"/>
                                        </div>
                                        <div class="input-group mb-3">
                                            <button onclick="searchMTGDB()" class="btn btn-outline-secondary" type="button" id="button-addon2">Search</button>
                                        </div>
                                        <div class="search-results" id="search-results"></div>
                                </div>
                        </div>

                    </div>
                </main>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
