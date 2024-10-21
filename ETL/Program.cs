using BLL.DramaDay.Extract.Models;

string airTimeInput = "Friday & Saturday 20:30 KST";
AirTime airSchedule = AirTime.FromString(airTimeInput);
Console.WriteLine($"Air Time: {airSchedule.ToString()}");

// Create a BroadcastPeriod object from a string
string broadcastPeriodInput = "2016-Dec-16 to 2017-Jan-28";
BroadcastPeriod broadcastPeriod = BroadcastPeriod.FromString(broadcastPeriodInput);
Console.WriteLine($"Broadcast Period: {broadcastPeriod.ToString()}");