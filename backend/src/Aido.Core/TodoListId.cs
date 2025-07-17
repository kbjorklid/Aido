namespace Aido.Core;

/// <summary>
/// Strongly-typed identifier for TodoList entities.
/// </summary>
public readonly struct TodoListId : IEquatable<TodoListId>
{
    public Guid Value { get; }

    public TodoListId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TodoListId cannot be empty.", nameof(value));
        
        Value = value;
    }

    public static TodoListId New() => new(Guid.NewGuid());

    public static implicit operator Guid(TodoListId id) => id.Value;
    public static implicit operator TodoListId(Guid value) => new(value);

    public bool Equals(TodoListId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is TodoListId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(TodoListId left, TodoListId right) => left.Equals(right);
    public static bool operator !=(TodoListId left, TodoListId right) => !left.Equals(right);
}