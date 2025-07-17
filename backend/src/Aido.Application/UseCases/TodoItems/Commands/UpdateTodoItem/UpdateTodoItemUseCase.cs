using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public UpdateTodoItemUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<TodoItemResponse>> ExecuteAsync(UpdateTodoItemRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.ListId, cancellationToken);
            
            if (todoList == null)
            {
                return Result.Failure<TodoItemResponse>("Todo list not found");
            }

            var todoItem = todoList.GetItem(request.ItemId);
            
            if (todoItem == null)
            {
                return Result.Failure<TodoItemResponse>("Todo item not found");
            }

            todoItem.UpdateTitle(request.Title);
            todoItem.UpdateDescription(request.Description);

            await _todoListRepository.UpdateAsync(todoList, cancellationToken);
            
            var response = TodoItemResponse.FromDomain(todoItem);
            return Result.Success(response);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<TodoItemResponse>(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure<TodoItemResponse>(ex.Message);
        }
    }
}