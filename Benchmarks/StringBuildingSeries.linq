<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	BenchmarkRunner.Run<StringBuildingSeriesBenchmark>();
}

[MemoryDiagnoser]
public class StringBuildingSeriesBenchmark
{
	private const string Separator = "|";
	private const int StackallocCharLimit = 1024;

	[Params(8, 32, 128)]
	public int SegmentCount { get; set; }

	private string[] _segments = Array.Empty<string>();
	private string[] _concatParts = Array.Empty<string>();
	private int _totalLength;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_segments = Enumerable.Range(0, SegmentCount)
			.Select(i => $"token{i:000}")
			.ToArray();

		_concatParts = BuildConcatParts(_segments);
		_totalLength = CalculateTotalLength(_segments);
	}

	// Repeated string growth creates many intermediate strings as input scales
	// This is intentionally the baseline anti-pattern, shows heavy allocation churn
	[Benchmark(Baseline = true)]
	public string PlusEqualsLoop()
	{
		var result = string.Empty;

		for (var i = 0; i < _segments.Length; i++)
		{
			if (i > 0)
			{
				result += Separator;
			}

			result += _segments[i];
		}

		return result;
	}

	// Interpolation in a growth loop still rebuilds the accumulated result each iteration
	// Readable syntax, but usually similar allocation pressure to repeated concatenation
	[Benchmark]
	public string InterpolationLoop()
	{
		var result = string.Empty;

		for (var i = 0; i < _segments.Length; i++)
		{
			result = i == 0
				? $"{_segments[i]}"
				: $"{result}{Separator}{_segments[i]}";
		}

		return result;
	}

	// Concat over prebuilt parts avoids per-iteration growth of the final result variable
	// often reduces intermediate work compared to loop-based + and interpolation
	[Benchmark]
	public string StringConcat()
	{
		return string.Concat(_concatParts);
	}

	// Join is specialised for separator-based assembly and is usually efficient
	// Commonly produces one final result with predictable behavior as segment count increases
	[Benchmark]
	public string StringJoin()
	{
		return string.Join(Separator, _segments);
	}

	// StringBuilder avoids repeated immutable string replacement during appends
	// Default capacity can stilll trigger internal buffer growth and extra copying
	[Benchmark]
	public string StringBuilderDefault()
	{
		var sb = new StringBuilder();

		for (var i = 0; i < _segments.Length; i++)
		{
			if (i > 0)
			{
				sb.Append(Separator);
			}

			sb.Append(_segments[i]);
		}

		return sb.ToString();
	}

	// Pre-sizing StringBuilder reduces or removes internal resizing for known output length
	// This often improves throughput and lowers transient allocation versus default capacity
	[Benchmark]
	public string StringBuilderPresized()
	{
		var sb = new StringBuilder(_totalLength);

		for (var i = 0; i < _segments.Length; i++)
		{
			if (i > 0)
			{
				sb.Append(Separator);
			}

			sb.Append(_segments[i]);
		}

		return sb.ToString();
	}

	// Copying into one pre-sized char buffer keeps writes contiguous and predictable
	// Heap buffer allocation remains, but intermediate string creation is minimised
	[Benchmark]
	public string SpanWithArrayBuffer()
	{
		var buffer = new char[_totalLength];
		var cursor = 0;

		for (var i = 0; i < _segments.Length; i++)
		{
			if (i > 0)
			{
				Separator.AsSpan().CopyTo(buffer.AsSpan(cursor));
				cursor += Separator.Length;
			}

			var segment = _segments[i].AsSpan();
			segment.CopyTo(buffer.AsSpan(cursor));
			cursor += segment.Length;
		}

		return new string(buffer);
	}

	// stackalloc can remove the heap char-buffer allocation for bounded payload sizes
	// Best for small/known sizes large buffers should fall back to heap allocation
	[Benchmark]
	public string SpanWithStackalloc()
	{
		if (_totalLength > StackallocCharLimit)
		{
			return SpanWithArrayBuffer();
		}

		Span<char> buffer = stackalloc char[_totalLength];
		var cursor = 0;

		for (var i = 0; i < _segments.Length; i++)
		{
			if (i > 0)
			{
				Separator.AsSpan().CopyTo(buffer[cursor..]);
				cursor += Separator.Length;
			}

			var segment = _segments[i].AsSpan();
			segment.CopyTo(buffer[cursor..]);
			cursor += segment.Length;
		}

		return new string(buffer);
	}

	private static string[] BuildConcatParts(string[] segments)
	{
		var parts = new string[segments.Length == 0 ? 0 : segments.Length * 2 - 1];
		var idx = 0;

		for (var i = 0; i < segments.Length; i++)
		{
			if (i > 0)
			{
				parts[idx++] = Separator;
			}

			parts[idx++] = segments[i];
		}

		return parts;
	}

	private static int CalculateTotalLength(string[] segments)
	{
		if (segments.Length == 0)
		{
			return 0;
		}

		var charsFromSegments = segments.Sum(s => s.Length);
		var charsFromSeparators = (segments.Length - 1) * Separator.Length;
		return charsFromSegments + charsFromSeparators;
	}
}
