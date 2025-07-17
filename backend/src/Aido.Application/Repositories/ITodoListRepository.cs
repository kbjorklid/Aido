using Aido.Core;

namespace Aido.Application.Repositories;

/// <summary>
/// Repository interface for managing TodoList persistence operations.
/// </summary>
public interface ITodoListRepository
{
    Task<TodoList?> GetByIdAsync(TodoListId id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TodoList>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(TodoList todoList, CancellationToken cancellationToken = default);

    Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(TodoListId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TodoListId id, CancellationToken cancellationToken = default);
}