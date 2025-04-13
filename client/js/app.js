import { getTodoItems, addTodoItem, addProgression, printItems, getCategories, updateTodoItem } from './api.js';
import { displayTodoItems, displayPrintItems } from './ui.js';

const todoListElement = document.getElementById('items-list');
const addTodoForm = document.getElementById('add-todo-form');
const addProgressionForm = document.getElementById('add-progression-form');
const printButton = document.getElementById('print-button');
const printOutput = document.getElementById('print-output');
const categorySelect = document.getElementById('category');
const itemsTableBody = document.querySelector('#items-table tbody');
const addTodoModal = document.getElementById('addTodoModal');
const addProgressionModal = document.getElementById('addProgressionModal');

const todoItems = [];
const newTodo = { id: 0, title: '', description: '', category: '' };
const newProgression = { id: null, date: null, percent: null };

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

const initializeTodoItemsSelect = () => {
    selectTodoId.innerHTML = '<option value="" disabled selected>Select Todo</option>'; // Reset select
    todoItems.forEach(item => {
        const option = document.createElement('option');
        option.value = item.id;
        option.textContent = `<span class="math-inline">\{item\.title\} \(</span>{item.category})`;
        selectTodoId.appendChild(option);
    });
};

const handleAddTodoItem = async (event) => {
    event.preventDefault();
    const id = parseInt(document.getElementById('id').value);
    const title = document.getElementById('title').value;
    const description = document.getElementById('description').value;
    const category = document.getElementById('category').value;
    // Verificar si los campos están vacíos
    if (!title || !description || !category) {
        alert('Please fill in all fields.');
        return; // Detiene la ejecución si algún campo está vacío
    }
    console.log('Adding Todo item:', id, "todoItems:", todoItems);
    if (isNaN(id)) {
        // Asignar los valores a newTodo

        newTodo.title = title;
        newTodo.description = description;
        newTodo.category = category;

        // Llamar a la función para agregar el nuevo TodoItem
        if (await addTodoItem(newTodo)) {
            await fetchAndDisplayTodoItems();
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
            if (await updateTodoItem(todoItem)) {
                await fetchAndDisplayTodoItems();
                addTodoModal.classList.remove('show'); // Ocultar el modal después de agregar        
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
    event.preventDefault();
    const todoId = parseInt(document.getElementById('todo-id').value);
    const date = document.getElementById('date').value;
    const percent = parseInt(document.getElementById('percent').value);
    // Verificar si los campos están vacíos
    if (!todoId || !date || isNaN(percent)) {
        alert('Please fill in all fields.');
        return; // Detiene la ejecución si algún campo está vacío
    }
    // Asignar los valores a newProgression
    newProgression.id = todoId;
    newProgression.date = date;
    newProgression.percent = percent;

    if (await addProgression(newProgression)) {
        await fetchAndDisplayTodoItems();
        newProgression.id = null;
        newProgression.date = null;
        newProgression.percent = null;
        addProgressionForm.reset();
       
        // Ocultar el modal de Bootstrap
        const addProgressionModalElement = document.getElementById('addProgressionModal');
        const addProgressionModal = new bootstrap.Modal(addProgressionModalElement);
        addProgressionModal.hide();
       
    } else {
        const error = await response.json(); // Leer el cuerpo de la respuesta de error
        alert(`Failed to add progression: ${error.message}`); // Mostrar el mensaje de error de la API
    }
};


const handlePrintItems = async () => {
    printOutput.innerText = await printItems();
};

export const fetchAndDisplayTodoItems = async () => {
    const items = await getTodoItems();
    todoItems.length = 0;
    todoItems.push(...items);
    displayTodoItems(items, itemsTableBody, calculateTotalPercent);
    displayPrintItems(items, todoListElement, calculateTotalPercent);
};

// Event Listeners
addTodoForm.addEventListener('submit', handleAddTodoItem);
addProgressionForm.addEventListener('submit', handleAddProgression);
printButton.addEventListener('click', handlePrintItems);

// Inicializar la aplicación mostrando los TodoItems
fetchAndDisplayTodoItems();
initializeCategories();