import { removeTodoItem } from './api.js';
import { fetchAndDisplayTodoItems } from './app.js'; 

export function displayPrintItems(items, todoListElement) {
    todoListElement.innerHTML = '';
    items.forEach(item => {
        console.log("displayTodoItems:", item);
        const li = document.createElement('li');
        li.classList.add('todo-item', 'list-group-item');
        li.innerHTML = `
            <h3>${item.title} - ${item.description} (${item.category}) Completed: ${item.isCompleted}</h3>
        `;
        if (item.progressions && item.progressions.length > 0) {
            let totalPercent = 0;
            item.progressions.forEach(progression => {
                totalPercent = totalPercent + progression.percent;
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
        todoListElement.appendChild(li);
    });
};

export function displayTodoItems(items, itemsTableBody, calculateTotalPercent) {
    itemsTableBody.innerHTML = ''; // Clear the table
    items.forEach(item => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${item.id}</td>
            <td>${item.title}</td>
            <td>${item.description}</td>
            <td>${item.category}</td>
            <td>${item.isCompleted ? 'Yes' : 'No'}</td>
            <td>
             <button class="btn btn-sm btn-outline-primary edit-todoItem-btn" data-id="${item.id}" data-title="${item.title}" data-description="${item.description}" data-category="${item.category}">
                   <i class="bi bi-pencil"></i></i>
                </button>
                <button class="btn btn-sm btn-outline-primary add-progression-btn" data-id="${item.id}" data-title="${item.title}">
                   <i class="bi bi-plus-square-fill"></i>
                </button>
                 <button class="btn btn-sm btn-outline-primary remove-todoItem-btn" data-id="${item.id}">
                   <i class="bi bi-trash3"></i>
                </button>
            </td>
        `;
        itemsTableBody.appendChild(row);
    });

    // Add event listeners to the progression buttons
    const addProgressionButtons = document.querySelectorAll('.add-progression-btn');
    addProgressionButtons.forEach(button => {
        button.addEventListener('click', () => {
            const todoId = parseInt(button.dataset.id, 10);
            document.getElementById('todo-id').value = todoId; // Select the item in the dropdown
            const todoTitle = button.dataset.title; // Get the title from the button's data attribute
            document.getElementById('todo-title').value = todoTitle; // Set the title in the modal
            // Use Bootstrap's Modal class to show the modal
            const progressionModalElement = document.getElementById('addProgressionModal');
            const progressionModal = new bootstrap.Modal(progressionModalElement);
            progressionModal.show();
        });
    });

    const editTodoItemButtons = document.querySelectorAll('.edit-todoItem-btn');
    editTodoItemButtons.forEach(button => {
        button.addEventListener('click', () => {
            console.log("Edit Todo Item Button clicked:", button.dataset);
            const todoId = parseInt(button.dataset.id, 10);
            document.getElementById('id').value = todoId; // Select the item in the dropdown
            const todoTitle = button.dataset.title; // Get the title from the button's data attribute
            document.getElementById('title').value = todoTitle; // Set the title in the modal
            const todoDescription = button.dataset.description; // Get the description from the button's data attribute
            document.getElementById('description').value = todoDescription; // Set the description in the modal
            const todoCategory = button.dataset.category; // Get the category from the button's data attribute
            document.getElementById('category').value = todoCategory; // Set the category in the modal
            // Use Bootstrap's Modal class to show the modal
            const addTodoModalElement = document.getElementById('addTodoModal');
            const addTodoModal = new bootstrap.Modal(addTodoModalElement);
            addTodoModal.show();
        });
    });

    // Add event listeners to the remove buttons
    const removeTodoItemButtons = document.querySelectorAll('.remove-todoItem-btn');
    removeTodoItemButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const todoId = parseInt(button.dataset.id, 10);
            if (confirm('Are you sure you want to delete this Todo item?')) {
                try {
                    if (await removeTodoItem(todoId)) {
                        await fetchAndDisplayTodoItems();
                    } else {
                        alert('Failed to delete Todo item!');
                    }
                } catch (error) {
                    console.error('Error deleting Todo item:', error);
                    alert('An error occurred while deleting the Todo item.');
                }
            }
        });
    });
};