using System.Globalization;
using System.Text.RegularExpressions;

namespace BLL.DramaDay.Extract.Models
{
    public class Drama
    {
        public string Title { get; set; }
        public List<string> OtherTitles { get; set; }
        public int TotalEps { get; set; }
        public string BroadcastNetwork { get; set; }
        public BroadcastPeriod? BroadcastPeriod { get; set; }
        public AirTime? AirTime { get; set; }
        public Dictionary<string, List<StandardEp>> StandardEpsDictionary { get; set; }
        public Dictionary<string, List<RangedEps>> RangedEpsDictionary { get; set; }

        public Drama()
        {
            OtherTitles = new List<string>();
            StandardEpsDictionary = new Dictionary<string, List<StandardEp>>();
            RangedEpsDictionary = new Dictionary<string, List<RangedEps>>();
        }
    }

    public enum Days
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public struct AirTime
    {
        public List<Days> Days { get; set; }
        public TimeSpan Time { get; set; }
        public string TimeZone { get; set; }

        public AirTime(List<Days> days, TimeSpan time, string timeZone)
        {
            Days = days;
            Time = time;
            TimeZone = timeZone;
        }

        public static AirTime FromString(string input)
        {
            // Regex pattern for "Day1 & Day2 HH:mm TZ"
            var pattern = @"^(?<days>[\w\s&]+)\s+(?<time>\d{1,2}:\d{2})\s+(?<timezone>\w+)$";
            var match = Regex.Match(input, pattern);

            if (!match.Success)
            {
                throw new ArgumentException($"Invalid format: {input}");
            }

            // Extract days
            var daysPart = match.Groups["days"].Value.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            var timePart = match.Groups["time"].Value;
            var timeZone = match.Groups["timezone"].Value;

            var days = new List<Days>();
            foreach (var day in daysPart)
            {
                var trimmedDay = day.Trim();
                if (Enum.TryParse(trimmedDay, true, out Days parsedDay))
                {
                    days.Add(parsedDay);
                }
                else
                {
                    throw new ArgumentException($"Invalid day: {trimmedDay}");
                }
            }

            if (!TimeSpan.TryParse(timePart, out TimeSpan time))
            {
                throw new ArgumentException($"Invalid time: {timePart}");
            }

            return new AirTime(days, time, timeZone);
        }

        public override string ToString()
        {
            return $"{string.Join(", ", Days)} at {Time} {TimeZone}";
        }
    }

    public struct BroadcastPeriod
    {
        public DateOnly BroadcastStartDate { get; set; }
        public DateOnly BroadcastEndDate { get; set; }

        public BroadcastPeriod(DateOnly broadcastStartDate, DateOnly broadcastEndDate)
        {
            BroadcastStartDate = broadcastStartDate;
            BroadcastEndDate = broadcastEndDate;
        }

        public static BroadcastPeriod FromString(string input)
        {
            // Regex pattern for "YYYY-MMM-DD to YYYY-MMM-DD"
            var pattern = @"^(?<start>\d{4}-[A-Za-z]{3}-\d{2})\s+to\s+(?<end>\d{4}-[A-Za-z]{3}-\d{2})$";
            var match = Regex.Match(input, pattern);

            if (!match.Success)
            {
                throw new ArgumentException($"Invalid format: {input}");
            }

            var startDateStr = match.Groups["start"].Value;
            var endDateStr = match.Groups["end"].Value;

            if (!DateOnly.TryParseExact(startDateStr, "yyyy-MMM-dd", out DateOnly startDate) ||
                !DateOnly.TryParseExact(endDateStr, "yyyy-MMM-dd", out DateOnly endDate))
            {
                throw new ArgumentException("Invalid date format. Use 'YYYY-MMM-DD'.");
            }

            return new BroadcastPeriod(startDate, endDate);
        }

        public override string ToString()
        {
            return $"{BroadcastStartDate:yyyy-MM-dd} to {BroadcastEndDate:yyyy-MM-dd}";
        }
    }
}
