using Microsoft.AspNetCore.Mvc;
using MediatR;
using TodoApp.Application.Commands;
using System.Threading.Tasks;
using TodoApp.Application.Queries;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddTodoItem([FromBody] AddTodoItemCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodoItem([FromBody] UpdateTodoItemCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveTodoItem(int id)
    {
        await _mediator.Send(new RemoveTodoItemCommand { Id = id });
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoItems()
    {
        var result = await _mediator.Send(new GetAllTodoItemsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItemById(int id)
    {
        var result = await _mediator.Send(new GetTodoItemByIdQuery { Id = id });
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost("{id}/progressions")]
    public async Task<IActionResult> RegisterProgression(int id, [FromBody] RegisterProgressionCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in route and body do not match");
        }
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id}/progressions")]
    public async Task<IActionResult> GetTodoItemProgressions(int id)
    {
        var result = await _mediator.Send(new GetTodoItemProgressionsQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("print")]
    public async Task<IActionResult> PrintTodoItems()
    {
        await _mediator.Send(new PrintTodoItemsCommand());
        return Ok();
    }
}
