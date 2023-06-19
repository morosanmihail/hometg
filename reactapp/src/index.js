import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import CardList from './Components/CardList';
import Search from './Components/Search';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
  <header>
        <nav id="sidebarMenu" className="collapse d-lg-block sidebar collapse bg-white">
                <div className="position-sticky">
                    <div className="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                        <button className="btn btn-default" type="button" data-toggle="collapse" data-target="#search">Search</button>
                    </div>
                    
                    <div className="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                        <button className="nav-link active" id="nav-Main-tab" data-bs-toggle="tab" data-bs-target="#nav-main" type="button" role="tab" aria-controls="nav-main" aria-selected="true">Button</button>
                    </div>
                </div>
        </nav>
        <nav id="main-navbar" className="navbar navbar-expand-lg navbar-light bg-white fixed-top">
            <div className="container-fluid">
                <a className="navbar-brand" href="/">
                    HomeTG
                </a>
            </div>
        </nav>
        </header>
        <main>
            <Search />

            <div className="container pt-4 tab-content">
                <div className="tab-pane fade show active" id="nav-main" role="tabpanel" aria-labelledby="nav-main-tab">
                    <div id="main-content">
                        <CardList />
                    </div>
                    <nav aria-label="Page navigation">
                        <ul className="pagination center">
                            <li className="page-item"><button className="page-link" href="#">Previous</button></li>
                            <li className="page-item"><button id="list-page" className="page-link">1</button></li>
                            <li className="page-item"><button className="page-link" href="#">Next</button></li>
                        </ul>
                    </nav>
                </div>
            </div>
         </main>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
