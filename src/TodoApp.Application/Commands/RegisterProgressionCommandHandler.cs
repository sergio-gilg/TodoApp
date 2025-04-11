using MediatR;
using TodoApp.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Application.Commands;

public class RegisterProgressionCommandHandler : IRequestHandler<RegisterProgressionCommand, Unit>
{
    private readonly ITodoList _todoList;

    public RegisterProgressionCommandHandler(ITodoList todoList)
    {
        _todoList = todoList;
    }

    public async Task<Unit> Handle(RegisterProgressionCommand request, CancellationToken cancellationToken)
    {
        _todoList.RegisterProgression(request.Id, request.Date, request.Percent);
        return Unit.Value;
    }
}