using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoItems.Commands.CompleteTodoItem;

public class CompleteTodoItemUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public CompleteTodoItemUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<TodoItemResponse>> ExecuteAsync(CompleteTodoItemRequest request, CancellationToken cancellationToken = default)
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

            todoItem.MarkAsCompleted();

            await _todoListRepository.UpdateAsync(todoList, cancellationToken);
            
            var response = TodoItemResponse.FromDomain(todoItem);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<TodoItemResponse>(ex.Message);
        }
    }
}