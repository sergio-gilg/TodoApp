import { getTodoItems, addTodoItem, addProgression, getCategories, updateTodoItem } from './api.js';
import { displayTodoItems, displayPrintItems } from './ui.js';

const todoListElement = document.getElementById('items-list');
const addTodoForm = document.getElementById('add-todo-form');
const addProgressionForm = document.getElementById('add-progression-form');
const categorySelect = document.getElementById('category');
const itemsTableBody = document.querySelector('#items-table tbody');
const btnAddTodoModal = document.getElementById('btnAddTodoModal');
const btnAddTodoForm = document.getElementById('btnAddTodoForm');
const btnAddProgressionModal = document.getElementById('btnAddProgressionModal');

const todoItems = [];
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
        Swal.fire('Fields cannot be empty or contain only spaces.');
        return;
    }

    const newTodo = { id, title, description, category };

    if (isNaN(id)) {
        // Llamar a la función para agregar el nuevo TodoItem
        const success = await addTodoItem(newTodo);
        if (success) {

            // Actualizar la tabla de TodoItems
            await fetchAndDisplayTodoItems();

            // Limpiar el formulario
            addTodoForm.reset();

            // Ocultar el modal de Bootstrap
            $('#addTodoModal').modal('hide');
        } else {
            const error = await response.json();
            Swal.fire(`Failed to add Todo Item: ${error.message}`);
        }

    } else {
        // Actualizar el TodoItem existente            
        const todoItem = todoItems.find(item => item.id === id);
        if (todoItem) {
            todoItem.title = title;
            todoItem.description = description;
            todoItem.category = category;

            if (await updateTodoItem(todoItem)) {
                // Actualizar la tabla de TodoItems
                await fetchAndDisplayTodoItems();

                // Cerrar el modal de Bootstrap
                $('#addTodoModal').modal('hide');
            } else {
                const error = await response.json();
                Swal.fire(`Failed to update Todo Item: ${error.message}`);
            }

        } else {
            Swal.fire('Todo item not found!');
        }
    }
};

const handleAddProgression = async (event) => {
    event.preventDefault();

    const todoId = parseInt(document.getElementById('todo-id').value);
    const date = document.getElementById('date').value;
    const percent = parseInt(document.getElementById('percent').value);

    // Verificar si los campos están vacíos
    if (!todoId || !date || isNaN(percent)) {
        Swal.fire('Please fill in all fields.');
        return;
    }

    // Verificar si el porcentaje está dentro del rango permitido
    if (percent < 0 || percent > 100) {
        s
        return;
    }

    // Asignar los valores a newProgression
    newProgression = { id: todoId, date, percent };

    if (await addProgression(newProgression)) {
        await fetchAndDisplayTodoItems();
        addProgressionForm.reset();

        // Ocultar el modal de Bootstrap      
        $('#addProgressionModal').modal('hide');

    } else {
        const error = await response.json();
        Swal.fire(`Failed to add progression: ${error.message}`);
    }
};

// Función para manejar el evento de clic en el botón "Agregar Todo"
const handlerbtnAddTodoModal = (event) => {
    event.preventDefault();
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
    document.getElementById('id').value = "";
    document.getElementById('title').value = "";
    document.getElementById('description').value = "";
    document.getElementById('category').value = "";

    $('#addTodoModal').modal('show');
};


const updateTodoTable = (items) => {
    displayTodoItems(items, itemsTableBody);
};

const updatePrintList = (items) => {
    displayPrintItems(items, todoListElement);
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

btnAddTodoForm.addEventListener('click', handleAddTodoItem);
btnAddProgressionModal.addEventListener('click', handleAddProgression);
btnAddTodoModal.addEventListener('click', handlerbtnAddTodoModal);
