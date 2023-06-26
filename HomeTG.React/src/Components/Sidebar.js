import React from 'react';
import { Link } from "react-router-dom";
import AddCollectionForm from './AddCollectionForm';
import { useCollection, useCollections } from './CollectionContext';
import OperationsTracker from './CardListNavButtons/OperationsTracker';

export default function Sidebar() {
    const collection = useCollection();
    const collections = useCollections();

    const renderCollections = () => {
        return (
            collections.map(c =>
                <Link to={"/c/" + c.id} key={c.id} className={"nav-link" + (c.id === collection ? " active" : "")}>
                    {c.id}
                </Link>
            )
        );
    }

    return (
        <header>
            <nav id="sidebarMenu" className="d-lg-block sidebar bg-white">
                <div className="position-sticky">
                    <div className="nav flex-column nav-pills me-3" role="tablist" aria-orientation="vertical">
                        <button className="btn btn-secondary" type="button" data-bs-toggle="collapse" data-bs-target="#search">Search</button>
                    </div>
                    <hr/>
                    <div className="nav flex-column nav-pills me-3" role="tablist" aria-orientation="vertical">
                        <React.Fragment>
                            {renderCollections(collections)}
                        </React.Fragment>
                        <hr/>
                        <AddCollectionForm />
                        <hr/>

                        <OperationsTracker/>
                    </div>
                </div>
            </nav>
            <nav id="main-navbar" className="navbar navbar-expand-lg bg-body-tertiary fixed-top">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">
                        HomeTG
                    </a>
                </div>
            </nav>
        </header>
    );
}
