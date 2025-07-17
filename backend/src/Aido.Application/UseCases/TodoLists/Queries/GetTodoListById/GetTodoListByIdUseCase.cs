using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoLists.Queries.GetTodoListById;

public class GetTodoListByIdUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public GetTodoListByIdUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<TodoListResponse>> ExecuteAsync(GetTodoListByIdRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (todoList == null)
            {
                return Result.Failure<TodoListResponse>("Todo list not found");
            }

            var response = TodoListResponse.FromDomain(todoList);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<TodoListResponse>(ex.Message);
        }
    }
}