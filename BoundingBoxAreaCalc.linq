<Query Kind="Program" />

void Main()
{
	//Lat = North/South, Lon = East/West
	//Lat = Y Long = X
	var x1_topRight_lon = -0.00181;
	var x2_btmLeft_lon = -0.02794;
	var y1_topRight_lat = 51.50963;
	var y2_btmLeft_lat = 51.48483;
	
	var x = (x2_btmLeft_lon - x1_topRight_lon)  *  (y2_btmLeft_lat - y1_topRight_lat);
	x.Dump();
}

