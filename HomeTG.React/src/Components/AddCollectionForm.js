import React, { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useOperations } from '../OperationsContext';
import { useCollectionsDispatch } from './CollectionContext';

function AddCollectionForm() {
    const [showForm, setShowForm] = useState(false);
    const [newItem, setNewItem] = useState('');

    const collectionsDispatch = useCollectionsDispatch();
    const ops = useOperations();

    const handleToggleForm = () => setShowForm(!showForm);
    const handleHideForm = () => setShowForm(false);

    const handleSubmit = (event) => {
        event.preventDefault();
        ops.fetch("Adding new collection", {}, '/collection/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id: newItem }),
        })
            .then((data) => {
                collectionsDispatch({
                    type: 'added',
                    item: {id:newItem},
                });
            });
    };

    return (
        <>
            <Button onClick={handleToggleForm}>Add Collection</Button>
            {showForm && (
                <Form onSubmit={handleSubmit}>
                    <Form.Group controlId="newItem">
                        <Form.Control
                            type="text"
                            value={newItem}
                            placeholder="New Collection"
                            onChange={(event) => setNewItem(event.target.value)}
                        />
                    </Form.Group>
                    <Button variant="primary" type="submit">
                        Submit
                    </Button>
                    <Button variant="secondary" onClick={handleHideForm}>
                        Cancel
                    </Button>
                </Form>
            )}
        </>
    );
};

export default AddCollectionForm;
