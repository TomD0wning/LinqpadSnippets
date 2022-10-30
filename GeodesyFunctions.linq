<Query Kind="Program" />

void Main()
{
	GeodesyFunctions geodsy = new GeodesyFunctions();
	
	GeoPoint Top = new GeoPoint {Latitude=51.0,Longitude=-0.0000 };
	//GeoPoint Bottom = new GeoPoint {Latitude=51.48483,Longitude=-0.02794};
	//GeoPoint endPos = new GeoPoint {Latitude=51.4709751, Longitude=-0.0490435};
	//// {51.4709751,-0.0490435}
	//var center = geodsy.calculateBBoxCenter(Top,Bottom);
	//geodsy.CalculateDistance(center, endPos).Dump();
	
	geodsy.PositionDisplacement(Top, 100,100).Dump();
}

public class GeodesyFunctions
{
	private readonly double meanEarthRadius = 6378137;
	private const double OffsetSeed = 0.006;
	
	public GeoPoint PositionDisplacement(GeoPoint location, int northingsOffset, int eastingsOffset)
	{
		var northingsOffsetRads = northingsOffset/meanEarthRadius;
		var eastingsOffsetRads = eastingsOffset/(meanEarthRadius * Math.Cos(Math.PI*location.Latitude/180));

		return new GeoPoint { Latitude = (location.Latitude + northingsOffsetRads* 180/Math.PI), Longitude = (location.Longitude + eastingsOffsetRads * 180/Math.PI)};
	}
	
	// https://en.wikipedia.org/wiki/Haversine_formula
	public double CalculateDistance(GeoPoint startPoint, GeoPoint endPoint)
	{
		 //convert inputs to radions
		var startLatAsRadions = ConvertToRadions(startPoint.Latitude);
		var endLatAsRadions = ConvertToRadions(endPoint.Latitude);
		var thetaAsRadions = ConvertToRadions(startPoint.Longitude - endPoint.Longitude);

		// Haversine: a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
		var distance = Math.Sin(startLatAsRadions) * Math.Sin(endLatAsRadions) + Math.Cos(startLatAsRadions) * Math.Cos(endLatAsRadions) * Math.Cos(thetaAsRadions);
		if (distance > 1) { distance = 1; }
		// back to degrees, then to miles
		distance = Math.Acos(distance);
		distance = ConvertToDegrees(distance);
		distance = distance * 60 * 1.1515;
		return distance;
	}

	public GeoPoint CalculateDestinationPoint(GeoPoint startPoint, double bearing, double distance)
	{
		// φ2 = asin( sin φ1 ⋅ cos δ + cos φ1 ⋅ sin δ ⋅ cos θ )
		var lat = Math.Asin(Math.Sin(startPoint.Latitude) * Math.Cos(distance / meanEarthRadius) + Math.Cos(startPoint.Latitude) * Math.Sin(distance / meanEarthRadius) * Math.Cos(bearing));

		//  λ2 = λ1 + atan2( sin θ ⋅ sin δ ⋅ cos φ1, cos δ − sin φ1 ⋅ sin φ2 )
		var lon = startPoint.Longitude + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance / meanEarthRadius) * Math.Cos(startPoint.Latitude), Math.Cos(distance / meanEarthRadius) - Math.Sin(startPoint.Latitude) * Math.Sin(lat));

		return new GeoPoint() { Latitude = ConvertToDegrees(lat), Longitude = ConvertToDegrees(lon) };
	}

	public double CalculateForwardAzimuth(GeoPoint startPoints, GeoPoint endPoints)
	{
		double y = Math.Sin(endPoints.Longitude - startPoints.Longitude) * Math.Cos(endPoints.Latitude);
		double x = Math.Cos(startPoints.Latitude) * Math.Sin(endPoints.Latitude) - Math.Sin(startPoints.Latitude) * Math.Cos(endPoints.Latitude) * Math.Cos(endPoints.Longitude - startPoints.Longitude);
		return Wrap2PI(Math.PI + Math.Atan2(y, x));
	}

	public double CalculateInverseAzimuth(GeoPoint startPoints, GeoPoint endPoints)
	{
		return Wrap2PI(CalculateForwardAzimuth(startPoints, endPoints) + Math.PI);
	}

	public double ConvertToRadions(double angleInDegrees)
	{
		return (angleInDegrees * Math.PI) / 180;
	}

	public double ConvertToDegrees(double angleInRadions)
	{
		return (angleInRadions * 180) / Math.PI;
	}

	private double Wrap(double val, double lim)
	{
		double vl = Math.Abs(val);
		double sign = Math.Sign(val);

		while (vl > lim)
			vl -= lim;

		return vl * sign;
	}

	private double Wrap2PI(double angle_rad)
	{
		return Wrap(angle_rad, (Math.PI * 2));
	}

	public GeoPoint calculateBBoxCenter(GeoPoint top, GeoPoint bottom)
	{
		//Lat = North/South, Lon = East/West
		//Lat = Y Long = X
		
		var x1_topRight_lon = top.Longitude;
		var x2_btmLeft_lon = bottom.Longitude;
		var y1_topRight_lat = top.Latitude;
		var y2_btmLeft_lat = bottom.Latitude;

		var bbox = $"{x1_topRight_lon},{y1_topRight_lat},{x2_btmLeft_lon},{y2_btmLeft_lat}";
		bbox.Dump();
		
		var Xres = (x1_topRight_lon + x2_btmLeft_lon) / 2;
		var yRes = (y1_topRight_lat + y2_btmLeft_lat) / 2;

		return new GeoPoint {Latitude = yRes, Longitude = Xres};
	}

	public static GeoPoint OffsetGeoLocation(GeoPoint location)
	{
		if (location == null)
		{
			return new GeoPoint(0, 0);
		}
		var rand = new Random();
		var offset = (rand.NextDouble() * OffsetSeed);

		return new GeoPoint(Math.Round(location.Latitude + offset, 7), Math.Round(location.Longitude + offset, 7));
	}
}

public class GeoPoint
{
	public GeoPoint(double lat, double lon)
	{
		Latitude = lat;
		Longitude = lon;
	}
	
	public GeoPoint(){}
	
	public double Latitude { get; set; }

	public double Longitude { get; set; }
}

public class GeoLocation
{
	public GeoLocation(decimal lat, decimal lon)
	{
		Latitude = lat;
		Longitude = lon;
	}
	
	public GeoLocation(){}
	
	public decimal Latitude { get; set; }
	
	public decimal Longitude {get; set; }
}

