<Query Kind="Program" />

void Main()
{
	// how many times a circle (a) will rotate around another circle (b), where a is 1/3 the size
	
	const double R = 90, r = 30, step = 0.02;
	double k = (R + r) / r;
	
	var rots = Rotations(R,r);
	rots.DumpTell();

	var dc = new DumpContainer().Dump("Epicycloid");
	var trail = new StringBuilder();

	for (double a = 0; a <= 2 * Math.PI; a += step)
	{
		double cx = (R + r) * Math.Cos(a), cy = (R + r) * Math.Sin(a);
		double px = cx - r * Math.Cos(k * a), py = cy - r * Math.Sin(k * a);
		trail.Append($"{px:F1},{py:F1} ");

		dc.Content = Util.RawHtml($"""
        <svg viewBox="-140 -140 280 280" width="420" height="420">
          <circle r="{R}" fill="none" stroke="#999" stroke-dasharray="3 3"/>
          <polyline points="{trail}" fill="none" stroke="#e07000" stroke-width="1.5"/>
          <circle cx="{cx:F1}" cy="{cy:F1}" r="{r}" fill="none" stroke="#0080c0"/>
          <line x1="{cx:F1}" y1="{cy:F1}" x2="{px:F1}" y2="{py:F1}" stroke="#0080c0"/>
          <circle cx="{px:F1}" cy="{py:F1}" r="4" fill="#e07000"/>
        </svg>
        """);

		Thread.Sleep(16);
	}
}

static double Rotations(double fixedRadius, double rollingRadius, bool inside = false)
	=> fixedRadius / rollingRadius + (inside ? -1 : 1);
