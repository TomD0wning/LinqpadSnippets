<Query Kind="Program">
  <NuGetReference>NodaTime</NuGetReference>
  <Namespace>NodaTime</Namespace>
</Query>

void Main()
{
	var validTime = ValidatePublishTime(6, 22);

	validTime.Dump();
	CalculateValidPublishTime(CurrentUkTime).Dump();

	UkTimeZone.Dump();

}

private static readonly DateTimeZone UkTimeZone = DateTimeZoneProviders.Tzdb["Europe/London"];
public DateTime CurrentUkTime => SystemClock.Instance.GetCurrentInstant().InZone(UkTimeZone).ToDateTimeUnspecified();


public bool ValidatePublishTime(int fromTime, int toTime)
{
	return CurrentUkTime.Hour > fromTime && CurrentUkTime.Hour < toTime;
}

public DateTime UkDateTimeToUtc(DateTime currentDateTime)
{
	return LocalDateTime
		.FromDateTime(currentDateTime)
		.InZoneLeniently(UkTimeZone)
		.ToInstant()
		.ToDateTimeUtc();
}

public DateTime CalculateValidPublishTime(DateTime currentDateTime)
{
	var publishDateTime = CurrentUkTime;
	if (CurrentUkTime.Hour > 22)
	{
		publishDateTime = new DateTime(CurrentUkTime.Year, CurrentUkTime.Month, CurrentUkTime.Day, 6, 1, 0).AddDays(1);
	}

	if (CurrentUkTime.Hour < 6)
	{
		publishDateTime = new DateTime(CurrentUkTime.Year, CurrentUkTime.Month, CurrentUkTime.Day, 6, 1, 0);
	}

	return LocalDateTime
		.FromDateTime(publishDateTime)
		.InZoneLeniently(UkTimeZone)
		.ToInstant()
		.ToDateTimeUtc();
}