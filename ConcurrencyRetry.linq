<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public static class ConcurrencyRetry
{
	public static async Task<T> Process<T>(Func<Task<T>> action)
	{
		var attempt = 0;
		var maxRetries = 5;
		var pow = 1;
		var maxDelayMilliseconds = 3000;
		var delayMilliseconds = 100;

		while (attempt < maxRetries)
		{
			try
			{
				return await action();
			}
			catch (ConcurrencyException)
			{
				attempt++;
				if (attempt >= maxRetries)
				{
					throw;
				}

				pow <<= 1;

				var millisecondsDelay = Math.Min(delayMilliseconds * (pow - 1), maxDelayMilliseconds);
				var random = new Random();
				var jitter = random.Next(
					millisecondsDelay - delayMilliseconds,
					millisecondsDelay + delayMilliseconds);

				Task.Delay(jitter).Wait();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		return default(T);
	}

	public static Task Process(Func<Task> action)
	{
		return Process(
			async () =>
			{
				await action();
				return true;
			});
	}

	public class ConcurrencyException : Exception
	{
		public ConcurrencyException()
		{
		}

		public ConcurrencyException(string message) : base(message)
		{
		}

		public ConcurrencyException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
