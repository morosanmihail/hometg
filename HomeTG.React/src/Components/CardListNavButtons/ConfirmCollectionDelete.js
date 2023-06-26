import React, { useRef } from "react";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import { confirmable, createConfirmation } from "react-confirm";

const ConfirmCollectionDelete = ({ show, proceed, dismiss, cancel, confirmType, collection, collections, selectedCount }) => {
    const inputRef = useRef();

    const handleOnClick = () => {
        return () => {
            proceed({
                input: inputRef.current != null ? inputRef.current.value : null
            });
        };
    };

    return (
        <div className="static-modal">
            <Modal animation={false} show={show} onHide={dismiss}>
                <Modal.Header>
                    <Modal.Title>Please confirm</Modal.Title>
                </Modal.Header>
                {
                    confirmType === "collection" ?
                    <Modal.Body>
                        <p>Are you sure you want to remove the collection {collection}?</p>
                        <p>If yes, would you like to move cards in this collection to a different one? Leave blank if no.</p>
                        <select ref={inputRef} className="form-control" defaultValue="">
                            {collections.map(c =>
                                <option key={"confirm" + c.id} dropdown={c.id} value={c.id}>{c.id}</option>
                            )}
                        </select>
                    </Modal.Body>
                : null}
                {
                    confirmType === "cards" ?
                        <Modal.Body>
                            <p>Are you sure you want to remove the {selectedCount} cards?</p>
                        </Modal.Body>
                : null}
                <Modal.Footer>
                    <Button
                        className="button-l"
                        onClick={handleOnClick()}
                    >Yes</Button>
                    <Button onClick={cancel}>Cancel</Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export const confirm = createConfirmation(confirmable(ConfirmCollectionDelete));
