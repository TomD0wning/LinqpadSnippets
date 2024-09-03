<Query Kind="Program" />

void Main()
{
	
}

public class PolicyUri : BaseUri
{
	public PolicyUri(string uri) : base(uri)
	{

	}
}

public interface IPolicyId
{
	PolicyId PolicyId { get; }
}

public class PolicyId : BaseId
{
	public PolicyId(string id) : base(id)
	{
	}
}


public abstract class BaseId
{
	public string Value { get; }

	protected BaseId(string id) => Value = id;

	public override string ToString() => Value;
}

public abstract class BaseUri : IEquatable<BaseUri>
{
	public string Uri { get; }

	protected BaseUri(string uri)
	{
		Uri = uri;

		ThrowIfInvalid();
	}

	private bool IsValid => Uri != null;

	private void ThrowIfInvalid()
	{
		if (!IsValid)
		{
			throw new ArgumentException($"{GetType().Name} is invalid");
		}
	}

	public bool Equals(BaseUri? other)
	{
		if (ReferenceEquals(null, other))
			return false;
		if (ReferenceEquals(this, other))
			return true;
		return Uri == other.Uri;
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj))
			return false;
		if (ReferenceEquals(this, obj))
			return true;
		if (obj.GetType() != GetType())
			return false;
		return Equals((BaseUri?)obj);
	}

	public override int GetHashCode() => (Uri?.GetHashCode()) ?? 0;

	public static bool operator ==(BaseUri left, BaseUri right) => Equals(left, right);
	public static bool operator !=(BaseUri left, BaseUri right) => !Equals(left, right);
}