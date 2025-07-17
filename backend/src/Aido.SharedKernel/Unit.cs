namespace Aido.SharedKernel;

/// <summary>
/// Represents a void return type for operations that don't return a value.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = new();

    public bool Equals(Unit other) => true;

    public override bool Equals(object? obj) => obj is Unit;

    public override int GetHashCode() => 0;

    public override string ToString() => "()";

    public static bool operator ==(Unit left, Unit right) => true;

    public static bool operator !=(Unit left, Unit right) => false;
}