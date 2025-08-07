using Aido.Application.UseCases.TodoLists.Commands.CreateTodoList;
using Aido.Application.UseCases.TodoLists.Commands.DeleteTodoList;
using Aido.Application.UseCases.TodoLists.Commands.GenerateAiSuggestions;
using Aido.Application.UseCases.TodoLists.Queries.GetAllTodoLists;
using Aido.Application.UseCases.TodoLists.Queries.GetTodoListById;
using Aido.Core;
using Aido.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using TodoListResponse = Aido.Application.UseCases.TodoLists.Queries.GetTodoListById.TodoListResponse;

namespace Aido.Presentation.Controllers;

[ApiController]
[Route("api/todo-lists")]
public class TodoListsController : ControllerBase
{
    private readonly GetAllTodoListsUseCase _getAllTodoListsUseCase;
    private readonly GetTodoListByIdUseCase _getTodoListByIdUseCase;
    private readonly CreateTodoListUseCase _createTodoListUseCase;
    private readonly DeleteTodoListUseCase _deleteTodoListUseCase;
    private readonly GenerateAiSuggestionsUseCase _generateAiSuggestionsUseCase;

    public TodoListsController(
        GetAllTodoListsUseCase getAllTodoListsUseCase,
        GetTodoListByIdUseCase getTodoListByIdUseCase,
        CreateTodoListUseCase createTodoListUseCase,
        DeleteTodoListUseCase deleteTodoListUseCase,
        GenerateAiSuggestionsUseCase generateAiSuggestionsUseCase)
    {
        _getAllTodoListsUseCase = getAllTodoListsUseCase;
        _getTodoListByIdUseCase = getTodoListByIdUseCase;
        _createTodoListUseCase = createTodoListUseCase;
        _deleteTodoListUseCase = deleteTodoListUseCase;
        _generateAiSuggestionsUseCase = generateAiSuggestionsUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodoLists(CancellationToken cancellationToken)
    {
        var result = await _getAllTodoListsUseCase.ExecuteAsync(cancellationToken);
        
        if (result.IsFailure)
        {
            return StatusCode(500, new { error = "InternalServerError", message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoListById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetTodoListByIdRequest(new TodoListId(id));
        
        Result<TodoListResponse> result = await _getTodoListByIdUseCase.ExecuteAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error == "Todo list not found")
            {
                return NotFound(new { error = "NotFound", message = result.Error });
            }
            return StatusCode(500, new { error = "InternalServerError", message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoList([FromBody] CreateTodoListRequestDto requestDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "BadRequest", message = "Invalid request data", details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var request = new CreateTodoListRequest(requestDto.Name, requestDto.Description ?? string.Empty);
        var result = await _createTodoListUseCase.ExecuteAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(new { error = "BadRequest", message = result.Error });
        }

        return CreatedAtAction(nameof(GetTodoListById), new { id = result.Value.Id }, result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteTodoListRequest(new TodoListId(id));
        var result = await _deleteTodoListUseCase.ExecuteAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error == "Todo list not found")
            {
                return NotFound(new { error = "NotFound", message = result.Error });
            }
            return StatusCode(500, new { error = "InternalServerError", message = result.Error });
        }

        return NoContent();
    }

    [HttpPost("{id}/ai-suggestions")]
    public async Task<IActionResult> GenerateAiSuggestions(Guid id, [FromBody] GenerateAiSuggestionsRequestDto? requestDto, CancellationToken cancellationToken)
    {
        var maxSuggestions = requestDto?.MaxSuggestions ?? 3;
        
        if (maxSuggestions < 1 || maxSuggestions > 10)
        {
            return BadRequest(new { error = "BadRequest", message = "MaxSuggestions must be between 1 and 10" });
        }

        var request = new GenerateAiSuggestionsRequest(new TodoListId(id), maxSuggestions);
        var result = await _generateAiSuggestionsUseCase.ExecuteAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error == "Todo list not found")
            {
                return NotFound(new { error = "NotFound", message = result.Error });
            }
            return StatusCode(500, new { error = "InternalServerError", message = result.Error });
        }

        return Ok(result.Value);
    }
}

public class CreateTodoListRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class GenerateAiSuggestionsRequestDto
{
    public int MaxSuggestions { get; set; } = 3;
}
