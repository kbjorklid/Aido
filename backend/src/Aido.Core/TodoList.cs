namespace Aido.Core;

/// <summary>
/// Represents a collection of related todo items that acts as an aggregate root.
/// </summary>
public class TodoList
{
    private readonly List<TodoItem> _items = new();

    public TodoListId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

    private TodoList() 
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public TodoList(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("List name cannot be empty or whitespace only.", nameof(name));

        Id = TodoListId.New();
        Name = name;
        Description = description ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public TodoItem AddItem(string title, string description, bool isSuggestionFromAi = false)
    {
        if (_items.Count >= 1000)
            throw new InvalidOperationException("Maximum of 1000 items per list exceeded.");

        var item = new TodoItem(title, description, isSuggestionFromAi);
        _items.Add(item);
        UpdatedAt = DateTime.UtcNow;
        return item;
    }

    public void RemoveItem(TodoItemId itemId)
    {

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public TodoItem? GetItem(TodoItemId itemId)
    {
        return _items.FirstOrDefault(i => i.Id == itemId);
    }

    public void UpdateName(string name)
    {

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("List name cannot be empty or whitespace only.", nameof(name));

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string description)
    {

        Description = description ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }
}
