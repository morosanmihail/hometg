import React, { useState, useEffect } from 'react';
import { useSearchParams  } from "react-router-dom";
import Card from './Card';
import { useOperations } from '../OperationsContext';
import ReactPaginate from "react-paginate";

function Search({ dedicatedPage = false, onAdd }) {
    const ops = useOperations();
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [shouldSearch, setShouldSearch] = useState(dedicatedPage);

    const [searchParams, setSearchParams] = useSearchParams();
    const [searchOptions, setSearchOptions] = useState({
        "name": searchParams.get("name") != null ? searchParams.get("name") : "",
        "setCode": searchParams.get("setCode") != null ? searchParams.get("setCode") : "",
        "artist": searchParams.get("artist") != null ? searchParams.get("artist") : "",
        "collectorNumber": searchParams.get("collectorNumber") != null ? searchParams.get("collectorNumber") : "",
        "text": searchParams.get("text") != null ? searchParams.get("text") : "",
    });

    let pageSize = 24;

    useEffect(() => {
        if(shouldSearch) {
            setLoading(true);

            ops.fetch(
                "Searching the MtG database", [],
                '/mtg/cards/search?pageSize=' + pageSize + '&offset=' + ((pageNumber-1) * pageSize),
                {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(searchOptions)
                }
            ).then(data => {
                setCards(data);
                setLoading(false);
                setShouldSearch(false);
            })
        }
    }, [pageNumber, shouldSearch])

    const handleSearchInput = (event, field) => {
        let newState = Object.assign({}, searchOptions);
        newState[field] = event.target.value;
        setSearchOptions(newState);
        setSearchParams(newState);
    }

    const handlePageChange = (event) => {
        setShouldSearch(true);
        setPageNumber(parseInt(event.selected)+1);
    };

    return (
        <React.Fragment>
            <div className={"collapse" + (dedicatedPage === true ? " show" : "")} id="search">
                <h2>Search</h2>
                <div className="list-group list-group-flush mx-3 mt-4">
                    <div className="input-group mb-3">
                        <input onChange={event => handleSearchInput(event, "name")} type="text" className="form-control" id="search-bar-name" placeholder="Name" value={searchOptions["name"]} />
                        <input onChange={event => handleSearchInput(event, "setCode")} type="text" className="form-control" id="search-bar-set" placeholder="Set" value={searchOptions["setCode"]} />
                    </div>
                    <div className="input-group mb-3">
                        <input onChange={event => handleSearchInput(event, "artist")} type="text" className="form-control" id="search-bar-artist" placeholder="Artist" value={searchOptions["artist"]} />
                        <input onChange={event => handleSearchInput(event, "collectorNumber")} type="text" className="form-control" id="search-bar-collector-number" placeholder="Collector Number" value={searchOptions["collectorNumber"]} />
                    </div>
                    <div className="input-group mb-3">
                        <input onChange={event => handleSearchInput(event, "text")} type="text" className="form-control" id="search-bar-text" placeholder="Text" value={searchOptions["text"]} />
                    </div>
                    <div className="input-group mb-3">
                        <button onClick={event => setShouldSearch(true)} className="btn btn-outline-secondary" type="button" id="button-addon2">Search</button>
                    </div>
                    <div className="search-results" id="search-results">
                        {
                            loading ?
                            <p>Loading...</p>
                            :
                            <div className="card-grid list">
                                {cards.map(card =>
                                    <Card key={card.id} id={card.id} card={card} onAdd={onAdd} />
                                )}
                            </div>
                        }
                    </div>
                        { cards.length > 0 ?
                    <ReactPaginate
                        previousLabel="Previous" nextLabel="Next"
                        pageClassName="page-item" pageLinkClassName="page-link"
                        previousClassName="page-item" previousLinkClassName="page-link"
                        nextClassName="page-item" nextLinkClassName="page-link"
                        breakLabel="..." breakClassName="page-item" breakLinkClassName="page-link"
                        containerClassName="pagination" activeClassName="active"
                        pageCount={(cards.length === pageSize) ? pageNumber + 1 : pageNumber}
                        marginPagesDisplayed={2}
                        pageRangeDisplayed={5}
                        onPageChange={handlePageChange}
                        forcePage={(cards.length > 0) ? Math.max(0, pageNumber - 1) : -1}
                    />
            : null }
                </div>
                <hr />
            </div>
        </React.Fragment>
    );
}

export default Search;
