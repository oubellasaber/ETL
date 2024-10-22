using BLL.DramaDay.Extract.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract
{
    public class DramaGeneralDataExtractor
    {
        private readonly HtmlNode _dataNode;
        private readonly string _url;
        private Dictionary<string, string> _dictionary;

        public DramaGeneralDataExtractor(HtmlNode dataNode, string url)
        {
            _dataNode = dataNode;
            _url = url;
            _dictionary = ExtactKeyValuePairs();
        }

        public Drama GetDramaWithGeneralData()
        {
            Drama drama = new Drama();

            drama.Title = GetDramaTitleFromUrl();
            drama.OtherTitles = GetOtherTitles();
            drama.TotalEps = GetTotalEpisodes();
            drama.BroadcastNetwork = GetValueFromDictionary("broadcast network", string.Empty);
            drama.AirTime = GetAirTime();
            drama.BroadcastPeriod = drama.AirTime != null ? GetBroadcastPeriod() : null;

            return drama;
        }

        private List<string> GetOtherTitles()
        {
            if (_dictionary.TryGetValue("title", out string title))
            {
                return title.Split("/", StringSplitOptions.TrimEntries).ToList();
            }
            return new List<string>();
        }

        private int GetTotalEpisodes()
        {
            if (_dictionary.TryGetValue("episodes", out string totalEps) &&
                int.TryParse(totalEps, out int result))
            {
                return result;
            }
            return default;
        }

        private string GetValueFromDictionary(string key, string defaultValue)
        {
            return _dictionary.TryGetValue(key, out string value) ? value : defaultValue;
        }

        private AirTime? GetAirTime()
        {
            if (_dictionary.TryGetValue("air time", out string airTime))
            {
                return AirTime.FromString(airTime);
            }
            return null;
        }

        private BroadcastPeriod? GetBroadcastPeriod()
        {
            if (_dictionary.TryGetValue("broadcast period", out string broadcastPeriod))
            {
                return broadcastPeriod.Contains("to") ? BroadcastPeriod.FromString(broadcastPeriod) : null;
            }
            return null;
        }

        private string GetDramaTitleFromUrl()
        {
            var uri = new Uri(_url);
            string path = uri.AbsolutePath;

            string titlePart = path.Trim('/');

            string title = titlePart.Replace('-', ' ');

            return title;
        }

        private Dictionary<string, string> ExtactKeyValuePairs()
        {
            List<string> lines = _dataNode.InnerText.Split("\n").ToList();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                string cleanLine = line.Trim().Replace("&amp;", "&");

                int colonIndex = cleanLine.IndexOf(':');

                if (colonIndex != -1)
                {
                    string key = cleanLine.Substring(0, colonIndex).Trim().ToLower();
                    string value = cleanLine.Substring(colonIndex + 1).Trim().ToLower();
                    keyValuePairs[key] = value;
                }
            }

            return keyValuePairs;
        }
    }
}
