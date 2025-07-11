using Aido.Application.Repositories;
using Aido.Core;
using System.Collections.Concurrent;

namespace Aido.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of ITodoListRepository for testing and development purposes.
/// </summary>
public class InMemoryTodoListRepository : ITodoListRepository
{
    private readonly ConcurrentDictionary<TodoListId, TodoList> _todoLists = new();

    public Task<TodoList?> GetByIdAsync(TodoListId id, CancellationToken cancellationToken = default)
    {
        _todoLists.TryGetValue(id, out var todoList);
        return Task.FromResult(todoList);
    }

    public Task<IEnumerable<TodoList>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_todoLists.Values.AsEnumerable());
    }

    public Task AddAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        _todoLists[todoList.Id] = todoList;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        _todoLists[todoList.Id] = todoList;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteByIdAsync(TodoListId id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_todoLists.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(TodoListId id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_todoLists.ContainsKey(id));
    }
}