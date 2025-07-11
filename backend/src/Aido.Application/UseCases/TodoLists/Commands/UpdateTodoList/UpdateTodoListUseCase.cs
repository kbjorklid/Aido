using Aido.Application.Repositories;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListUseCase
{
    private readonly ITodoListRepository _todoListRepository;

    public UpdateTodoListUseCase(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<TodoListResponse>> ExecuteAsync(UpdateTodoListRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var todoList = await _todoListRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (todoList == null)
            {
                return Result.Failure<TodoListResponse>("Todo list not found");
            }

            todoList.UpdateName(request.Name);
            todoList.UpdateDescription(request.Description);

            await _todoListRepository.UpdateAsync(todoList, cancellationToken);
            
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