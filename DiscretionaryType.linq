<Query Kind="Program" />

public readonly struct DiscretionaryType<T> : IEquatable<DiscretionaryType<T>>
{
	public bool IsSpecified { get; }

	private T Value { get; }

	private DiscretionaryType(T value)
	{
		IsSpecified = true;
		Value = value;
	}

	public static implicit operator DiscretionaryType<T>(T val) => new DiscretionaryType<T>(val);

	public T GetValueOrDefault(T defaultValue)
	{
		return IsSpecified ? Value : defaultValue;
	}

	public bool Equals(DiscretionaryType<T> other)
	{
		return EqualityComparer<T>.Default.Equals(Value, other.Value) && IsSpecified == other.IsSpecified;
	}

	public override bool Equals(object obj)
	{
		return obj is DiscretionaryType<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Value, IsSpecified);
	}

	public static bool operator ==(DiscretionaryType<T> left, DiscretionaryType<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(DiscretionaryType<T> left, DiscretionaryType<T> right)
	{
		return !left.Equals(right);
	}
}
