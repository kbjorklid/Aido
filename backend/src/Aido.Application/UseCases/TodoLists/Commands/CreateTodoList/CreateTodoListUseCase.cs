using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoLists.Commands.CreateTodoList;

public class CreateTodoListUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public CreateTodoListUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<TodoListResponse>> ExecuteAsync(CreateTodoListRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoList = new TodoList(request.Name, request.Description);
            
            await _todoListRepository.AddAsync(todoList, cancellationToken);
            
            var response = TodoListResponse.FromDomain(todoList);
            return Result.Success(response);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<TodoListResponse>(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure<TodoListResponse>(ex.Message);
        }
    }
}