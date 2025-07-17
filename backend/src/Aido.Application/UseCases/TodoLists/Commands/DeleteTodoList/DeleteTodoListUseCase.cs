using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public DeleteTodoListUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<Unit>> ExecuteAsync(DeleteTodoListRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _todoListRepository.ExistsAsync(request.Id, cancellationToken);
            
            if (!exists)
            {
                return Result.Failure<Unit>("Todo list not found");
            }

            var deleted = await _todoListRepository.DeleteByIdAsync(request.Id, cancellationToken);
            
            if (!deleted)
            {
                return Result.Failure<Unit>("Failed to delete todo list");
            }

            return Result.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<Unit>(ex.Message);
        }
    }
}