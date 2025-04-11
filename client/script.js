const todoList = document.getElementById('items-list');
const addTodoForm = document.getElementById('add-todo-form');
const addProgressionForm = document.getElementById('add-progression-form');
const printButton = document.getElementById('print-button');
const printOutput = document.getElementById('print-output');

const API_URL = 'http://localhost:5297/TodoItems'; // Ajusta la URL si es diferente

// Función para obtener los datos de la API
async function getTodoItems() {
    try {
        const response = await fetch(API_URL);
        console.log("getTodoItems:", response);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching Todo items:', error);
        return [];
    }
}

// Función para mostrar los TodoItems en la lista
async function displayTodoItems() {
    const items = await getTodoItems();
    todoList.innerHTML = '';
    items.forEach(item => {
        const li = document.createElement('li');
        li.classList.add('todo-item');
        li.innerHTML = `
            <h3>${item.title} - ${item.description} (${item.category})</h3>
        `;
        if (item.progressions && item.progressions.length > 0) {
            item.progressions.forEach(progression => {
                const totalPercent = item.progressions.reduce((sum, p) => sum + p.percent, 0);
                const progressBarWidth = (progression.percent / 100) * 100;
                li.innerHTML += `
                  <div class="progression">
                      <p>${new Date(progression.date).toLocaleDateString()} - ${totalPercent}%</p>
                      <div class="progress-bar">
                          <div class="progress-bar-fill" style="width: ${totalPercent}%;"></div>
                      </div>
                  </div>
              `;
            });
        }
        todoList.appendChild(li);
    });
}

// Función para agregar un nuevo TodoItem
async function addTodoItem(event) {
    event.preventDefault();
    const title = document.getElementById('title').value;
    const description = document.getElementById('description').value;
    const category = document.getElementById('category').value;

    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ title, description, category })
        });
        if (response.ok) {
            displayTodoItems();
            addTodoForm.reset();
        } else {
            console.error('Failed to add Todo item');
        }
    } catch (error) {
        console.error('Error adding Todo item:', error);
    }
}

// Función para agregar un progreso a un TodoItem
async function addProgression(event) {
    event.preventDefault();
    const todoId = parseInt(document.getElementById('todo-id').value);
    const date = document.getElementById('date').value;
    const percent = parseInt(document.getElementById('percent').value);
    try {
        const response = await fetch(`${API_URL}/${todoId}/progressions`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: todoId, date: date, percent: percent })
        });
        if (response.ok) {
            displayTodoItems();
            addProgressionForm.reset();
        } else {
            console.error('Failed to add progression');
        }
    } catch (error) {
        console.error('Error adding progression:', error);
    }
}

async function printItems() {
    try {
        const response = await fetch(`${API_URL}/print`);
        if (response.ok) {
            printOutput.innerText = 'Printed successfully';
        } else {
            printOutput.innerText = 'Failed to print';
        }
    } catch (error) {
        console.error('Error printing items:', error);
    }
}
// Event Listeners
addTodoForm.addEventListener('submit', addTodoItem);
addProgressionForm.addEventListener('submit', addProgression);
printButton.addEventListener('click', printItems);

// Inicializar la aplicación mostrando los TodoItems
displayTodoItems();