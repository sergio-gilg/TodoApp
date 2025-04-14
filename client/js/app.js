import { getTodoItems, addTodoItem, addProgression, printItems, getCategories, updateTodoItem } from './api.js';
import { displayTodoItems, displayPrintItems } from './ui.js';

const todoListElement = document.getElementById('items-list');
const addTodoForm = document.getElementById('add-todo-form');
const addProgressionForm = document.getElementById('add-progression-form');
const printOutput = document.getElementById('print-output');
const categorySelect = document.getElementById('category');
const itemsTableBody = document.querySelector('#items-table tbody');
const addTodoModal = document.getElementById('addTodoModal');
const addProgressionModal = document.getElementById('addProgressionModal');
const btnAddTodoModal = document.getElementById('btnAddTodoModal');

const todoItems = [];
const newTodo = { id: 0, title: '', description: '', category: '' };
let newProgression = { id: null, date: null, percent: null };

const calculateTotalPercent = (progressions) => {
    if (!progressions || progressions.length === 0) {
        return 0;
    }
    return progressions.reduce((sum, p) => sum + p.percent, 0);
};

const initializeCategories = async () => {
    const categories = await getCategories();
    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category;
        option.textContent = category;
        categorySelect.appendChild(option);
    });
};

const handleAddTodoItem = async (event) => {
    event.preventDefault();

    // Obtener los valores de los campos del formulario
    const id = parseInt(document.getElementById('id').value);
    const title = document.getElementById('title').value;
    const description = document.getElementById('description').value;
    const category = document.getElementById('category').value;

    // Verificar si los campos están vacíos
    if (!title.trim() || !description.trim() || !category.trim()) {
        alert('Fields cannot be empty or contain only spaces.');
        return;
    }

    if (isNaN(id)) {
        // Asignar los valores a newTodo
        newTodo.title = title;
        newTodo.description = description;
        newTodo.category = category;

        // Llamar a la función para agregar el nuevo TodoItem
        if (await addTodoItem(newTodo)) {
            //await fetchAndDisplayTodoItems();
            newTodo.title = '';
            newTodo.description = '';
            newTodo.category = '';
            addTodoForm.reset();
            // Ocultar el modal de Bootstrap
            const modal = bootstrap.Modal(addTodoModal);
            modal.hide();
        } else {
            const error = await response.json(); // Leer el cuerpo de la respuesta de error
            alert(`Failed to add Todo Item: ${error.message}`); // Mostrar el mensaje de error de la API
        }

    } else {
        // Actualizar el TodoItem existente
        console.log('Updating Todo item:', id, "todoItems:", todoItems);
        const todoItem = todoItems.find(item => item.id === id);
        if (todoItem) {
            todoItem.title = title;
            todoItem.description = description;
            todoItem.category = category;
            console.log('Todo item found:', todoItem);

            if (await updateTodoItem(todoItem)) {
                // await fetchAndDisplayTodoItems();
                // Cerrar el modal de Bootstrap
                const modal = new bootstrap.Modal(addTodoModal);
                modal.hide();
            } else {
                const error = await response.json(); // Leer el cuerpo de la respuesta de error
                alert(`Failed to update Todo Item: ${error.message}`); // Mostrar el mensaje de error de la API
            }

        } else {
            alert('Todo item not found!');
        }
    }
};

const handleAddProgression = async (event) => {
    console.log('handleAddProgression called:', event);
    event.preventDefault();

    const todoId = parseInt(document.getElementById('todo-id').value);
    const date = document.getElementById('date').value;
    const percent = parseInt(document.getElementById('percent').value);

    // Verificar si los campos están vacíos
    if (!todoId || !date || isNaN(percent)) {
        alert('Please fill in all fields.');
        return;
    }

    // Verificar si el porcentaje está dentro del rango permitido
    if (percent < 0 || percent > 100) {
        percentInput.classList.add('is-invalid');
        document.getElementById('percent-error').textContent = 'Percent must be between 1 and 99.';
        return;
    }

    // Asignar los valores a newProgression
    newProgression = { id: todoId, date, percent };

    if (await addProgression(newProgression)) {
        //await fetchAndDisplayTodoItems();
        addProgressionForm.reset();

        // Ocultar el modal de Bootstrap      
        const bsAddProgression = bootstrap.Modal.getInstance(addProgressionModal);
        bsAddProgression.hide();

    } else {
        const error = await response.json(); // Leer el cuerpo de la respuesta de error
        alert(`Failed to add progression: ${error.message}`); // Mostrar el mensaje de error de la API
    }
};

// Función para manejar el evento de clic en el botón "Agregar Todo"
const handlerbtnAddTodoModal = () => {
    // Cambiar el título del modal
    const modalTitle = document.querySelector('#addTodoModal .modal-title');
    modalTitle.textContent = 'Add Todo Item';
    // Cambiar el texto del botón "Agregar"
    const modalButton = document.querySelector('#addTodoModal .btn-primary');
    modalButton.textContent = 'Add';
    // Desactivar el campo Title
    const titleInput = document.getElementById('title');
    titleInput.disabled = false;
    // Desactivar el campo Category
    const categoryInput = document.getElementById('category');
    categoryInput.disabled = false;
    document.getElementById('id').value = ""; // Select the item in the dropdown
    document.getElementById('title').value = ""; // Set the title in the modal
    document.getElementById('description').value = ""; // Set the description in the modal
    document.getElementById('category').value = ""; // Set the category in the modal            
}


const handlePrintItems = async () => {
    printOutput.innerText = await printItems();
};

const handleApiError = (error) => {
    console.error('API Error:', error);
    alert('An unexpected error occurred. Please try again later.');
};

const showLoading = () => {
    document.getElementById('loading-indicator').style.display = 'block';
};

const hideLoading = () => {
    document.getElementById('loading-indicator').style.display = 'none';
};

const updateTodoTable = (items) => {
    displayTodoItems(items, itemsTableBody, calculateTotalPercent);
};

const updatePrintList = (items) => {
    displayPrintItems(items, todoListElement, calculateTotalPercent);
};

export const fetchAndDisplayTodoItems = async () => {
    const items = await getTodoItems();
    todoItems.length = 0;
    todoItems.push(...items);
    updateTodoTable(items);
    updatePrintList(items);
};


fetchAndDisplayTodoItems();
initializeCategories();

addTodoForm.addEventListener('submit', handleAddTodoItem);
addProgressionForm.addEventListener('submit', handleAddProgression);
btnAddTodoModal.addEventListener('click', handlerbtnAddTodoModal);

