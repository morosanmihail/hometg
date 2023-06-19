import React, { Fragment, useState } from 'react';
import { useParams } from "react-router-dom";
import Sidebar from "./Components/Sidebar";
import Search from "./Components/Search";
import CardList from "./Components/CardList";

function BaseApp() {
    const { collection = "Main", offset = 0 } = useParams();

    return (
        <Fragment>
            <header>
                <Sidebar collection={collection} />

                <nav id="main-navbar" className="navbar navbar-expand-lg navbar-light bg-white fixed-top">
                    <div className="container-fluid">
                        <a className="navbar-brand" href="/">
                            HomeTG
                        </a>
                    </div>
                </nav>
            </header>
            <main>
                <Search collection={collection} />
                <CardList collection={collection} offset={offset} />
            </main>
        </Fragment>
    );
}

export default BaseApp;
