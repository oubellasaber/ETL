using BLL.DramaDay.Extract.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract
{
    public class RowDataExtractor
    {
        public static StandardEp ExtractSingle(HtmlNode tr)
        {
            var cells = GetCells(tr);

            return new StandardEp
            {
                Number = ExtractEpisodeNumber(cells[0].InnerText),
                EpVersions = ExtractEpVersions(cells.Skip(1).ToList()) // Updated to match the new format using EpVersions
            };
        }

        private static List<HtmlNode> GetCells(HtmlNode row)
        {
            // Get all cells in the row
            return row.SelectNodes(".//td")?.ToList() ?? new List<HtmlNode>();
        }

        private static int ExtractEpisodeNumber(string cellText)
        {
            // Extract the episode number from the cell text
            if (int.TryParse(cellText.Trim(), out int episodeNumber))
            {
                return episodeNumber;
            }
            return -1; // Handle error, possibly throw exception or handle differently
        }

        private static List<EpVersion> ExtractEpVersions(List<HtmlNode> cells)
        {
            // Extract link hosts and media formats from the given cells
            var mediaFormats = ExtractMediaFormats(cells[0].InnerHtml);
            var linkGroups = ExtractLinkGroups(cells[1].InnerHtml);

            return MatchFormatsWithLinks(mediaFormats, linkGroups);
        }

        private static List<string> ExtractMediaFormats(string cellHtml)
        {
            // Extract and split media formats from the cell HTML
            return cellHtml.Split("<br>", StringSplitOptions.RemoveEmptyEntries)
                           .ToList();
        }

        private static string[] ExtractLinkGroups(string cellHtml)
        {
            // Extract all links and their respective groups from the cell HTML
            return cellHtml.Split("<br>", StringSplitOptions.RemoveEmptyEntries);
        }

        private static List<EpVersion> MatchFormatsWithLinks(List<string> mediaFormats, string[] linkGroups)
        {
            var epVersions = new List<EpVersion>();

            // Match each format with its corresponding group of links
            for (int i = 0; i < mediaFormats.Count && i < linkGroups.Length; i++)
            {
                var mediaFormat = mediaFormats[i];
                var links = ExtractLinksFromGroup(linkGroups[i]);

                if (links != null)
                {
                    epVersions.Add(new EpVersion
                    {
                        RawQualityFormat = mediaFormat,
                        Hosts = CreateHostsFromLinks(links)
                    });
                }
            }

            return epVersions;
        }

        private static HtmlNodeCollection ExtractLinksFromGroup(string linkGroupHtml)
        {
            // Extract links from a given group of links (HTML content)
            return HtmlNode.CreateNode($"<div>{linkGroupHtml}</div>").SelectNodes(".//a");
        }

        private static List<Host> CreateHostsFromLinks(HtmlNodeCollection links)
        {
            // Create Host objects for each link
            var hosts = new List<Host>();
            foreach (var link in links)
            {
                hosts.Add(new Host
                {
                    UnresolvedUrl = link.GetAttributeValue("href", string.Empty),
                    HostName = link.InnerText.Trim()
                });
            }

            return hosts;
        }
    }
}
