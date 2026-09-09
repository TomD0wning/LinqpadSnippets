<Query Kind="Program" />

void Main()
{
	var configuration = new WineTestConfiguration(
		bottleCount: 1_000,
		testerCount: 10);

	var poisonedBottle = BottleNumber.From(231, configuration);

	var simulation = WineTestingSimulation.Create(configuration);

	var testPlan = simulation.GenerateTestPlan();
	var outcome = simulation.Run(poisonedBottle);
	var decodedBottle = simulation.Decode(outcome);

	new
	{
		Configuration = configuration,
		MaximumSupportedBottles = configuration.MaximumSupportedBottles,
		SecretPoisonedBottle = poisonedBottle.Value,
		SickTesters = outcome.SickTesters.Select(t => t.Value).ToArray(),
		DecodedBottle = decodedBottle.Value,
		Success = decodedBottle == poisonedBottle
	}.Dump("Simulation Result");

	testPlan
		.Take(20)
		.Select(plan => new
		{
			Bottle = plan.Bottle.Value,
			Binary = plan.Bottle.ToBinary(configuration.TesterCount),
			Testers = plan.Testers.Select(t => $"Tester {t.Value}").ToArray()
		})
		.Dump("Testing Plan: First 20 Bottles");

	outcome
		.SickTesters
		.Select(t => new
		{
			Tester = t.Value,
			BinaryContribution = t.BinaryValue
		})
		.Dump("Sick Tester Contributions");

	simulation
		.ValidateAllBottles()
		.Dump("Exhaustive Self-Check");
}

public sealed record WineTestConfiguration
{
	public WineTestConfiguration(int bottleCount, int testerCount)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bottleCount);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(testerCount);

		if (testerCount > 30)
		{
			throw new ArgumentOutOfRangeException(
				nameof(testerCount),
				testerCount,
				"Tester count is too large for this Int32-backed demo.");
		}

		var maximumSupportedBottles = (1 << testerCount) - 1;

		if (bottleCount > maximumSupportedBottles)
		{
			throw new ArgumentException(
				$"Cannot uniquely identify {bottleCount:N0} one-based bottles with {testerCount:N0} testers. " +
				$"Maximum supported bottles is {maximumSupportedBottles:N0}.");
		}

		BottleCount = bottleCount;
		TesterCount = testerCount;
	}

	public int BottleCount { get; }

	public int TesterCount { get; }

	public int MaximumSupportedBottles => (1 << TesterCount) - 1;
}

public sealed class WineTestingSimulation
{
	private readonly WineTestConfiguration _configuration;

	private WineTestingSimulation(WineTestConfiguration configuration)
	{
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	public static WineTestingSimulation Create(WineTestConfiguration configuration) =>
		new(configuration);

	public IReadOnlyList<BottleTestPlan> GenerateTestPlan() =>
		Enumerable
			.Range(1, _configuration.BottleCount)
			.Select(bottleNumber => BottleNumber.From(bottleNumber, _configuration))
			.Select(CreateBottleTestPlan)
			.ToArray();

	public TestOutcome Run(BottleNumber poisonedBottle)
	{
		EnsureBottleBelongsToConfiguration(poisonedBottle);

		var sickTesters = TesterNumbers()
			.Where(tester => poisonedBottle.HasBitSet(tester.ZeroBasedIndex))
			.ToArray();

		return new TestOutcome(sickTesters);
	}

	public BottleNumber Decode(TestOutcome outcome)
	{
		ArgumentNullException.ThrowIfNull(outcome);

		var decodedValue = outcome
			.SickTesters
			.Sum(tester => tester.BinaryValue);

		return BottleNumber.From(decodedValue, _configuration);
	}

	public object ValidateAllBottles()
	{
		var failures = Enumerable
			.Range(1, _configuration.BottleCount)
			.Select(value => BottleNumber.From(value, _configuration))
			.Select(bottle =>
			{
				var outcome = Run(bottle);
				var decoded = Decode(outcome);

				return new
				{
					Bottle = bottle.Value,
					Decoded = decoded.Value,
					Success = bottle == decoded
				};
			})
			.Where(result => !result.Success)
			.ToArray();

		return new
		{
			CheckedBottleCount = _configuration.BottleCount,
			FailureCount = failures.Length,
			Success = failures.Length == 0,
			Failures = failures
		};
	}

	private BottleTestPlan CreateBottleTestPlan(BottleNumber bottle)
	{
		var testers = TesterNumbers()
			.Where(tester => bottle.HasBitSet(tester.ZeroBasedIndex))
			.ToArray();

		return new BottleTestPlan(bottle, testers);
	}

	private IEnumerable<TesterNumber> TesterNumbers() =>
		Enumerable
			.Range(1, _configuration.TesterCount)
			.Select(TesterNumber.FromOneBased);

	private void EnsureBottleBelongsToConfiguration(BottleNumber bottle)
	{
		if (bottle.Value > _configuration.BottleCount)
		{
			throw new ArgumentOutOfRangeException(
				nameof(bottle),
				bottle.Value,
				$"Bottle {bottle.Value:N0} is outside the configured range.");
		}
	}
}

public readonly record struct BottleNumber
{
	private BottleNumber(int value)
	{
		Value = value;
	}

	public int Value { get; }

	public static BottleNumber From(int value, WineTestConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration);

		if (value < 1)
		{
			throw new ArgumentOutOfRangeException(
				nameof(value),
				value,
				"Bottle numbers are one-based and must be greater than zero.");
		}

		if (value > configuration.BottleCount)
		{
			throw new ArgumentOutOfRangeException(
				nameof(value),
				value,
				$"Bottle number cannot exceed configured bottle count of {configuration.BottleCount:N0}.");
		}

		return new BottleNumber(value);
	}

	public bool HasBitSet(int zeroBasedBitIndex) =>
		(Value & (1 << zeroBasedBitIndex)) != 0;

	public string ToBinary(int width) =>
		Convert.ToString(Value, 2).PadLeft(width, '0');

	public override string ToString() => Value.ToString();
}

public readonly record struct TesterNumber
{
	private TesterNumber(int value)
	{
		Value = value;
	}

	public int Value { get; }

	public int ZeroBasedIndex => Value - 1;

	public int BinaryValue => 1 << ZeroBasedIndex;

	public static TesterNumber FromOneBased(int value)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

		return new TesterNumber(value);
	}

	public override string ToString() => Value.ToString();
}

public sealed record BottleTestPlan(
	BottleNumber Bottle,
	IReadOnlyList<TesterNumber> Testers);

public sealed record TestOutcome(
	IReadOnlyList<TesterNumber> SickTesters);