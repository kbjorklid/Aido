using Aido.Application.Repositories;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoLists.Queries.GetAllTodoLists;

public class GetAllTodoListsUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public GetAllTodoListsUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<IEnumerable<TodoListResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var todoLists = await _todoListRepository.GetAllAsync(cancellationToken);
            var response = todoLists.Select(TodoListResponse.FromDomain);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<TodoListResponse>>(ex.Message);
        }
    }
}