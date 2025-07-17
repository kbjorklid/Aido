using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoItems.Commands.RemoveTodoItem;

public class RemoveTodoItemUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public RemoveTodoItemUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<Unit>> ExecuteAsync(RemoveTodoItemRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.ListId, cancellationToken);
            
            if (todoList == null)
            {
                return Result.Failure<Unit>("Todo list not found");
            }

            var todoItem = todoList.GetItem(request.ItemId);
            
            if (todoItem == null)
            {
                return Result.Failure<Unit>("Todo item not found");
            }

            todoList.RemoveItem(request.ItemId);

            await _todoListRepository.UpdateAsync(todoList, cancellationToken);
            
            return Result.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<Unit>(ex.Message);
        }
    }
}