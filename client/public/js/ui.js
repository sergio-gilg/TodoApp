import { removeTodoItem } from './api.js';
import { fetchAndDisplayTodoItems } from './app.js';

export function displayPrintItems(items, todoListElement) {
    todoListElement.innerHTML = '';
    items.forEach(item => {
        const li = document.createElement('li');
        li.classList.add('todo-item', 'list-group-item');
        li.innerHTML = `
            <h4>${item.id}) ${item.title} - ${item.description} (${item.category}) Completed: ${item.isCompleted}</h4>
        `;
        if (item.progressions && item.progressions.length > 0) {
            let totalPercent = 0;
            item.progressions.forEach(progression => {
                totalPercent = totalPercent + progression.percent;
                li.innerHTML += `
                    <div class="progression">
                        <p>${new Date(progression.date).toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' })} - ${totalPercent}%</p>
                        <div class="progress-bar" style="height: 20px;">
                            <div class="progress-bar-fill" style="width: ${totalPercent}%;" aria-valuenow="${totalPercent}"> ${totalPercent}%</div>
                        </div>
                    </div>
                `;
            });
        }
        todoListElement.appendChild(li);
    });
};

export function displayTodoItems(items, itemsTableBody) {
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
        button.addEventListener('click', (event) => {
            event.preventDefault();
            const todoId = parseInt(button.dataset.id, 10);
            document.getElementById('todo-id').value = todoId; 
            const todoTitle = button.dataset.title; 
            document.getElementById('todo-title').value = todoTitle; 

            // Limpiar los campos de la modal
            document.getElementById('date').value = ''; 
            document.getElementById('percent').value = ''; 

            // Use Bootstrap's Modal class to show the modal          
            $("#addProgressionModal").modal("show");
        });
    });

    const editTodoItemButtons = document.querySelectorAll('.edit-todoItem-btn');
    editTodoItemButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            event.preventDefault();
            console.log("Edit Todo Item Button clicked:", button.dataset);
            const todoId = parseInt(button.dataset.id, 10);
            document.getElementById('id').value = todoId; 
            const todoTitle = button.dataset.title; 
            document.getElementById('title').value = todoTitle;
            const todoDescription = button.dataset.description; 
            document.getElementById('description').value = todoDescription; 
            const todoCategory = button.dataset.category; 
            document.getElementById('category').value = todoCategory; 

            // Cambiar el título del modal
            const modalTitle = document.querySelector('#addTodoModal .modal-title');
            modalTitle.textContent = 'Update Todo Item';

            // Cambiar el texto del botón "Agregar"
            const modalButton = document.querySelector('#addTodoModal .btn-primary');
            modalButton.textContent = 'Update';

            // Desactivar el campo Title
            const titleInput = document.getElementById('title');
            titleInput.disabled = true;

            // Desactivar el campo Category
            const categoryInput = document.getElementById('category');
            categoryInput.disabled = true;

            // Use Bootstrap's Modal class to show the modal 
            $("#addTodoModal").modal("show");        
        });
    });

    // Add event listeners to the remove buttons
    const removeTodoItemButtons = document.querySelectorAll('.remove-todoItem-btn');
    removeTodoItemButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            event.preventDefault();

            const todoId = parseInt(button.dataset.id);
            Swal.fire({
                title: "Are you sure you want to delete this Todo item?",
                showCancelButton: true,
                confirmButtonText: "Remove",
                cancelButtonText: "Cancel"
            }).then(async (result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    try {
                        if (await removeTodoItem(todoId)) {
                            await fetchAndDisplayTodoItems();
                        }
                    } catch (error) {
                        console.error('Error deleting Todo item:', error);
                        Swal.fire('An error occurred while deleting the Todo item.');
                    }
                }
            });
        });
    });
};