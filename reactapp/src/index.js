import React, { Fragment } from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route, Link, useParams } from "react-router-dom";
import './index.css';
import reportWebVitals from './reportWebVitals';
import CardList from './Components/CardList';
import Search from './Components/Search';
import Sidebar from './Components/Sidebar';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <BrowserRouter>
        <Fragment>
            <header>
                <Routes>
                    <Route exact path="/" element={<Sidebar />} />
                    <Route path="/:collection" element={<Sidebar />} />
                </Routes>
                
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
                <Routes>
                    <Route exact path="/" element={<CardList />} />
                    <Route path="/:collection" element={<CardList/> } />
                    <Route path="/:collection/:offset" element={<CardList />} />
                </Routes>
            </main>
        </Fragment>
    </BrowserRouter>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
