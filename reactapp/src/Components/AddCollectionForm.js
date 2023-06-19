import React, { useState } from 'react';
import { Button, Form } from 'react-bootstrap';

const AddCollectionForm = () => {
    const [showForm, setShowForm] = useState(false);
    const [newItem, setNewItem] = useState('');

    const handleToggleForm = () => setShowForm(!showForm);
    const handleHideForm = () => setShowForm(false);

    const handleSubmit = (event) => {
        event.preventDefault();
        fetch('/collection/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ item: newItem }),
        })
            .then((response) => response.json())
            .then((data) => console.log(data))
            .catch((error) => console.error(error));
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
