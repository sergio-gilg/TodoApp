using MediatR;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands
{
    public class RemoveTodoItemCommandHandler : IRequestHandler<RemoveTodoItemCommand, Unit>
    {
        private readonly ITodoList _todoList;

        public RemoveTodoItemCommandHandler(ITodoList todoList)
        {
            _todoList = todoList;
        }

        public async Task<Unit> Handle(RemoveTodoItemCommand request, CancellationToken cancellationToken)
        {
            _todoList.RemoveItem(request.Id);
            return Unit.Value;
        }
       
    }
}