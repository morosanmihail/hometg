import React, { useState, useEffect } from 'react';
import Card from './Card';
import { Link, useParams } from "react-router-dom";

function Sidebar() {
    const { collection } = useParams();
    const [collections, setCollections] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch('/collection/list').then(response => {
            if (response.status === 200) {
                response.json().then(data => {
                    setCollections(data);
                    setLoading(false);
                })
            }
        });
    }, [collection])

    const renderCollections = (collections) => {
        return (
            <div className="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                {collections.map(c =>
                    <Link to={"/" + c.id} key={c.id}>
                        <button className={"nav-link" + (c.id === collection ? " active" : "")} type="button">{c.id}</button>
                    </Link>
                )}
            </div>
        );
    }

    let contents = loading
        ? <p>Loading...</p>
        : renderCollections(collections);

    return (
        <header>
        <nav id="sidebarMenu" className="collapse d-lg-block sidebar collapse bg-white">
                    <div className="position-sticky">
                        <div className="nav flex-column nav-pills me-3" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                            <button className="btn btn-default" type="button" data-toggle="collapse" data-target="#search">Search</button>
                        </div>

                    {contents}
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
    );
}

export default Sidebar;