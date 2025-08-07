using Aido.Application.UseCases.TodoItems.Commands.AddTodoItem;
using Aido.Application.UseCases.TodoItems.Commands.RemoveTodoItem;
using Aido.Core;
using Microsoft.AspNetCore.Mvc;

namespace Aido.Presentation.Controllers;

[ApiController]
[Route("api/todo-lists/{listId}/items")]
public class TodoItemsController : ControllerBase
{
    private readonly AddTodoItemUseCase _addTodoItemUseCase;
    private readonly RemoveTodoItemUseCase _removeTodoItemUseCase;

    public TodoItemsController(AddTodoItemUseCase addTodoItemUseCase, RemoveTodoItemUseCase removeTodoItemUseCase)
    {
        _addTodoItemUseCase = addTodoItemUseCase;
        _removeTodoItemUseCase = removeTodoItemUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> AddTodoItem(Guid listId, [FromBody] AddTodoItemRequestDto requestDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "BadRequest", message = "Invalid request data", details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var request = new AddTodoItemRequest(new TodoListId(listId), requestDto.Title, requestDto.Description ?? string.Empty);
        var result = await _addTodoItemUseCase.ExecuteAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error == "Todo list not found")
            {
                return NotFound(new { error = "NotFound", message = result.Error });
            }
            if (result.Error.Contains("Maximum of 1000 items") || result.Error.Contains("title cannot be empty"))
            {
                return BadRequest(new { error = "BadRequest", message = result.Error });
            }
            return StatusCode(500, new { error = "InternalServerError", message = result.Error });
        }

        return CreatedAtAction("GetTodoListById", "TodoLists", new { id = listId }, result.Value);
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> RemoveTodoItem(Guid listId, Guid itemId, CancellationToken cancellationToken)
    {
        var request = new RemoveTodoItemRequest(new TodoListId(listId), new TodoItemId(itemId));
        var result = await _removeTodoItemUseCase.ExecuteAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error == "Todo list not found" || result.Error == "Todo item not found")
            {
                return NotFound(new { error = "NotFound", message = result.Error });
            }
            return StatusCode(500, new { error = "InternalServerError", message = result.Error });
        }

        return NoContent();
    }
}

public class AddTodoItemRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}