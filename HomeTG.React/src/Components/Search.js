import React, { useState, useEffect } from 'react';
import { useSearchParams  } from "react-router-dom";
import Card from './Card';
import { useOperations } from '../OperationsContext';
import ReactPaginate from "react-paginate";
import { useCardSets } from './ReusableConstants/CardSets';
import { useCollections } from './CollectionContext';

function Search({ startSearch = false, dedicatedPage = false }) {
    const ops = useOperations();
    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [shouldSearch, setShouldSearch] = useState(startSearch);
    const cardSets = useCardSets();
    const collections = useCollections();

    const [searchParams, setSearchParams] = useSearchParams();
    const [searchOptions, setSearchOptions] = useState({
        "name": searchParams.get("name") != null ? searchParams.get("name") : "",
        "setCode": searchParams.get("setCode") != null ? searchParams.get("setCode") : "",
        "artist": searchParams.get("artist") != null ? searchParams.get("artist") : "",
        "collectorNumber": searchParams.get("collectorNumber") != null ? searchParams.get("collectorNumber") : "",
        "text": searchParams.get("text") != null ? searchParams.get("text") : "",
        "rarity": searchParams.get("rarity") != null ? searchParams.get("rarity") : "",
        "colorIdentities": searchParams.getAll("colorIdentities") != null ? searchParams.getAll("colorIdentities") : [],
    });
    const [searchCollection, setSearchCollection] = useState("");

    let pageSize = 24;

    useEffect(() => {
        if(shouldSearch) {
            setLoading(true);

            let url = (searchCollection !== "" && searchCollection !== "skipNotOwned") ?
                "/collection/cards/" + searchCollection + "/search?pageSize=" :
                "/collection/search?pageSize=";
            url = url + pageSize + '&offset=' + ((pageNumber-1) * pageSize);

            if (searchCollection === "skipNotOwned") {
                url = url + "&skipNotOwned=true"
            }

            ops.fetch(
                "Searching", [], url,
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
    }, [pageNumber, shouldSearch]);

    const handleSearchInput = (event, field) => {
        let newState = Object.assign({}, searchOptions);
        newState[field] = event.target.value;
        setSearchOptions(newState);
        setSearchParams(newState);
    };

    const handleColourIdentitiesInput = (event, colour) => {
        let newState = Object.assign({}, searchOptions);
        if (event.target.checked) {
            newState["colorIdentities"] = [...newState["colorIdentities"], event.target.value];
        } else {
            newState["colorIdentities"] = newState["colorIdentities"].filter(c => c != event.target.value);
        }
        setSearchOptions(newState);
        setSearchParams(newState);
    }

    const handleCollectionInput = (event) => {
        setSearchCollection(event.target.value);
    }

    const handlePageChange = (event) => {
        setShouldSearch(true);
        setPageNumber(parseInt(event.selected)+1);
    };

    return (
        <React.Fragment>
            <div className={(dedicatedPage === true ? "" : "collapse")} id={dedicatedPage ? "main-search" : "search"}>
                <h2>Search</h2>
                <div className="list-group list-group-flush mx-3 mt-4">
                    <div className="input-group">
                        <input onChange={event => handleSearchInput(event, "name")} type="text" className="form-control" id="search-bar-name" placeholder="Name" value={searchOptions["name"]} />
                        <input onChange={event => handleSearchInput(event, "setCode")} className="form-control" list="datalistOptions" id="search-bar-set" placeholder="Set Code" value={searchOptions["setCode"]} />
                        <datalist id="datalistOptions">
                            {cardSets.map(c =>
                                <option key={c} value={c}/>
                            )}
                        </datalist>
                    </div>
                    <div className="input-group">
                        <input onChange={event => handleSearchInput(event, "artist")} type="text" className="form-control" id="search-bar-artist" placeholder="Artist" value={searchOptions["artist"]} />
                        <input onChange={event => handleSearchInput(event, "collectorNumber")} type="text" className="form-control" id="search-bar-collector-number" placeholder="Collector Number" value={searchOptions["collectorNumber"]} />
                    </div>
                    <div className="input-group">
                        <input onChange={event => handleSearchInput(event, "text")} type="text" className="form-control" id="search-bar-text" placeholder="Text" value={searchOptions["text"]} />
                    </div>
                    <div className='input-group'>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleColourIdentitiesInput(e, "W")} className="form-check-input" type="checkbox" id="inlineCheckbox1" value="W"/>
                            <label className="form-check-label" for="inlineCheckbox1">W</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleColourIdentitiesInput(e, "U")} className="form-check-input" type="checkbox" id="inlineCheckbox2" value="U"/>
                            <label className="form-check-label" for="inlineCheckbox2">U</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleColourIdentitiesInput(e, "B")} className="form-check-input" type="checkbox" id="inlineCheckbox3" value="B"/>
                            <label className="form-check-label" for="inlineCheckbox3">B</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleColourIdentitiesInput(e, "R")} className="form-check-input" type="checkbox" id="inlineCheckbox4" value="R"/>
                            <label className="form-check-label" for="inlineCheckbox4">R</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleColourIdentitiesInput(e, "G")} className="form-check-input" type="checkbox" id="inlineCheckbox5" value="G"/>
                            <label className="form-check-label" for="inlineCheckbox5">G</label>
                        </div>
                    </div>
                    { false ?
                    <div className='input-group'>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleSearchInput(e, "rarity")} className="form-check-input" type="radio" id="rarityRadio1" value="C"/>
                            <label className="form-check-label" for="rarityRadio1">Common</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleSearchInput(e, "rarity")} className="form-check-input" type="radio" id="rarityRadio2" value="U"/>
                            <label className="form-check-label" for="rarityRadio2">Uncommon</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleSearchInput(e, "rarity")} className="form-check-input" type="radio" id="rarityRadio3" value="R"/>
                            <label className="form-check-label" for="rarityRadio3">Rare</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input onChange={e => handleSearchInput(e, "rarity")} className="form-check-input" type="radio" id="rarityRadio4" value="M"/>
                            <label className="form-check-label" for="rarityRadio4">Mythic</label>
                        </div>
                    </div>
                    : null }
                    <div className="input-group">
                        <button onClick={event => {setPageNumber(1); setShouldSearch(true);}} className="btn btn-outline-secondary" type="button" id="button-addon2">Search</button>
                        <select onChange={(e) => handleCollectionInput(e)} className="form-control" id="searchInCollection">
                            <option key={"searchincol-empty"} dropdown="in MtG database" value={""}>in MtG database</option>
                            <option key={"searchincol-collections"} dropdown="in all collections" value={"skipNotOwned"}>in all collections</option>
                            {collections.map(c =>
                                <option key={"searchincol-" + c.id} dropdown={c.id} value={c.id}>{"in " + c.id}</option>
                            )}
                        </select>
                    </div>
                    <div className="search-results" id="search-results">
                        {
                            loading ?
                            <p>Loading...</p>
                            :
                            <div className="card-grid list">
                                {cards.map(card =>
                                    <Card key={card.mtGCard.id + "-" + (card.card != null ? card.card.collectionId : "")} id={card.mtGCard.id} card={card.mtGCard} details={card.card} />
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
                        pageCount={(cards.length >= pageSize) ? pageNumber + 1 : pageNumber}
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
