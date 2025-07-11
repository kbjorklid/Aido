using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoItems.Commands.IncompleteTodoItem;

public class IncompleteTodoItemUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public IncompleteTodoItemUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<TodoItemResponse>> ExecuteAsync(IncompleteTodoItemRequest request, CancellationToken cancellationToken = default)
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

            todoItem.MarkAsIncomplete();

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