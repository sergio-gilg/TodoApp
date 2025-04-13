using Microsoft.AspNetCore.Mvc;
using MediatR;
using TodoApp.Application.Commands;
using System.Threading.Tasks;
using TodoApp.Application.Queries;
using TodoApp.Api.ApiResponses;

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
        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodoItem([FromBody] UpdateTodoItemCommand command)
    {
        try
        {
            Console.WriteLine($"Updating TodoItem with ID: {command.Id}");
            await _mediator.Send(command);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveTodoItem(int id)
    {
        try
        {
            await _mediator.Send(new RemoveTodoItemCommand { Id = id });
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoItems()
    {
        try
        {
            var result = await _mediator.Send(new GetAllTodoItemsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItemById(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetTodoItemByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new ErrorResponse { Message = "TodoItem not found." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }
    }

    [HttpPost("{id}/progressions")]
    public async Task<IActionResult> RegisterProgression(int id, [FromBody] RegisterProgressionCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest("ID in route and body do not match");
            }
            await _mediator.Send(command);
            return Ok();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("{id}/progressions")]
    public async Task<IActionResult> GetTodoItemProgressions(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetTodoItemProgressionsQuery { Id = id });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }

    }

    [HttpGet("print")]
    public async Task<IActionResult> PrintTodoItems()
    {
        try
        {
            await _mediator.Send(new PrintTodoItemsCommand());
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        try{
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = "An error occurred while processing your request." });
        }  
    }
}
