const API_URL = 'http://localhost:5297/TodoItems'; // Ajusta la URL si es diferente

// Función para obtener los datos de la API
export async function getTodoItems() {
    try {
        const response = await fetch(API_URL);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching Todo items:', error);
        return [];
    }
}

// Función para agregar un nuevo TodoItem
export async function addTodoItem(item) {
    try {

        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        });
        if (response.ok) {
            return true;
        } else {
            const error = await response.json();
            console.error('Failed to add Todo item:', error);
            return false;
        }
    } catch (error) {
        console.error('Error adding Todo item:', error);
        return false;
    }
}

export async function updateTodoItem(item) {
    try {
        console.info('Updating Todo item:', item);
        const response = await fetch(`${API_URL}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        });
        if (response.ok) {
            return true;
        } else {
            const error = await response.json();
           Swal.fire({
                title: `Failed to update Todo item`,
                text: `${error.message}`,
                icon: 'error',
            }); 
            return false;
        }
    } catch (error) {
        console.error('Error updating Todo item:', error);
        return false;
    }
}
// Función para agregar un progreso a un TodoItem
export async function addProgression(progression) {
    try {
        const response = await fetch(`${API_URL}/${progression.id}/progressions`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(progression)
        });
        if (response.ok) {
            return true;
        } else {
            const error = await response.json();
            Swal.fire({
                title: `Failed to add Progression`,
                text: `${error.message}`,
                icon: 'error',
            });     
            return false;
        }
    } catch (error) {
        console.error('Error adding progression:', error);
        return false;
    }
}

export async function printItems() {
    try {
        const response = await fetch(`${API_URL}/print`);
        if (response.ok) {
            return 'Printed successfully';
        } else {
            return 'Failed to print';
        }
    } catch (error) {
        console.error('Error printing items:', error);
        return 'Failed to print';
    }
}

export async function getCategories() {
    try {
        const response = await fetch(`${API_URL}/categories`);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching categories:', error);
        return [];
    }
}

// Función para eliminar un TodoItem
export async function removeTodoItem(id) {
    try {
        const response = await fetch(`${API_URL}/${id}`, {
            method: 'DELETE'
        });
        if (response.ok) {
            return true;
        } else {
            const error = await response.json(); // Leer el cuerpo de la respuesta de error
            Swal.fire({
                title: `Failed to delete Todo item`,
                text: `${error.message}`,
                icon: 'error',
            }); 
            return false;
        }
    } catch (error) {
        console.error('Error deleting Todo item:', error);
        return false;
    }
}