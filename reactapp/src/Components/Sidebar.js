import React, { useState, useEffect } from 'react';
import { Link, useParams } from "react-router-dom";
import AddCollectionForm from './AddCollectionForm';

function Sidebar() {
    const { collection = "Main" } = useParams();
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
            collections.map(c =>
                <Link to={"/" + c.id} key={c.id} className={"nav-link" + (c.id === collection ? " active" : "")}>
                    {c.id}
                </Link>
            )
        );
    }

    let contents = loading
        ? <p>Loading...</p>
        : renderCollections(collections);

    return (
        <header>
            <nav id="sidebarMenu" className="collapse d-lg-block sidebar collapse bg-white">
                <div className="position-sticky">
                    <div className="nav flex-column nav-pills me-3" role="tablist" aria-orientation="vertical">
                        <button className="btn btn-default" type="button" data-toggle="collapse" data-target="#search">Search</button>
                    </div>
                    <hr/>
                    <div className="nav flex-column nav-pills me-3" role="tablist" aria-orientation="vertical">
                        {contents}
                        <AddCollectionForm/>
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
    );
}

export default Sidebar;