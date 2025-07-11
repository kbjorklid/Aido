namespace Aido.Core;

/// <summary>
/// Strongly-typed identifier for TodoItem entities.
/// </summary>
public readonly struct TodoItemId : IEquatable<TodoItemId>
{
    public Guid Value { get; }

    public TodoItemId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TodoItemId cannot be empty.", nameof(value));
        
        Value = value;
    }

    public static TodoItemId New() => new(Guid.NewGuid());

    public static implicit operator Guid(TodoItemId id) => id.Value;
    public static implicit operator TodoItemId(Guid value) => new(value);

    public bool Equals(TodoItemId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is TodoItemId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static bool operator ==(TodoItemId left, TodoItemId right) => left.Equals(right);
    public static bool operator !=(TodoItemId left, TodoItemId right) => !left.Equals(right);
}