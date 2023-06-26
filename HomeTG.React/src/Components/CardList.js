import React, { useState, useEffect, createContext, useContext } from 'react';
import Card from './Card';
import { useNavigate } from "react-router-dom";
import Search from "./Search";
import CardListNav from "./CardListNav";
import { CardCacheProvider } from './CardCacheContext';
import { useCollection, usePageNumber } from './CollectionContext';
import { useOperations } from '../OperationsContext';
import { useSelectedCardsDispatch } from './CardListContexts/SelectedCardsContext';
import ReactPaginate from "react-paginate";

const RefreshCardListContext = createContext(null);
export function useRefreshCardList() {
    return useContext(RefreshCardListContext);
}

export default function CardList({ showSearch=false }) {
    const navigate = useNavigate();
    const ops = useOperations();
    const collection = useCollection();
    const pageNumber = usePageNumber();
    const selectedDispatch = useSelectedCardsDispatch();

    const [cards, setCards] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refresh, setRefresh] = useState(false);
    const [cardCount, setCardCount] = useState(0);

    let pageSize = 12;

    useEffect(() => {
        ops.fetch(
            "Listing items in " + collection, [],
            '/collection/cards/' + collection + '/list?offset=' + ((pageNumber-1) * pageSize) + '&pageSize=' + pageSize
            ).then(data => {
                setCards(data);
                setLoading(false);
                setRefresh(false);
                selectedDispatch({type:'empty'});
            });
        ops.fetch(
            "Getting card count in " + collection, 0,
            '/collection/cards/' + collection + '/count'
        ).then(data => {
            setCardCount(data);
        });
    }, [collection, pageNumber, refresh])

    const onAdd = (newCard) => {
        let updated = false;
        const newCards = cards.map(c => {
            if (c.id === newCard.id) {
                updated = true;
                return newCard;
            }
            return c;
        })
        if (updated) {
            setCards(newCards);
        } else {
            if (pageNumber === 1) {
                setCards([newCard, ...cards.slice(0, pageSize - 1)]);
            } else {
                setRefresh(true)
            }
        }
    }

    const triggerRefresh = () => {
        setRefresh(true);
    }

    const handlePageChange = (event) => {
        navigate('/c/' + collection + '/' + (parseInt(event.selected)+1));
    };

    return (
        <CardCacheProvider>
            <RefreshCardListContext.Provider value={triggerRefresh}>
                <Search onAdd={onAdd} dedicatedPage={showSearch} />
                <CardListNav/>
                <div className="card-grid list">
                    {
                        (loading || refresh) ? <p>Loading...</p> :
                            <React.Fragment>
                                {cards.map(card =>
                                    <Card id={card.id} details={card} key={card.collectionId + "-" + card.id}
                                        onAdd={onAdd} />
                                )}
                            </React.Fragment>
                    }
                </div>
                <ReactPaginate
                    previousLabel="Previous"
                    nextLabel="Next"
                    pageClassName="page-item"
                    pageLinkClassName="page-link"
                    previousClassName="page-item"
                    previousLinkClassName="page-link"
                    nextClassName="page-item"
                    nextLinkClassName="page-link"
                    breakLabel="..."
                    breakClassName="page-item"
                    breakLinkClassName="page-link"
                    pageCount={Math.ceil(parseInt(cardCount) / pageSize)}
                    marginPagesDisplayed={2}
                    pageRangeDisplayed={5}
                    onPageChange={handlePageChange}
                    containerClassName="pagination"
                    activeClassName="active"
                    forcePage={pageNumber - 1}
                />
            </RefreshCardListContext.Provider>
        </CardCacheProvider>
    );
}
