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
                Hosts = ExtractLinkHosts(cells.Skip(1).ToList()) // Skip episode number cell and pass remaining cells
            };
        }

        /* public static ((int start, int end) epsRange, List<LinkHost> linkHosts) ExtractRanged(HtmlNode tr)
        {
            var epsRange = ParseEpisodeRange(tr.SelectSingleNode(".//td").InnerText);
            var linkHosts = ExtractLinkHosts(tr.SelectNodes(".//td").Skip(1).ToList());

            return (epsRange, linkHosts);
        }*/

        /*public static SpecialEpisode ExtractSpecial(HtmlNode tr)
        {
            var cells = GetCells(tr);

            return new SpecialEpisode
            {
                EpRawText = cells[0].InnerText,
                LinkHosts = ExtractLinkHosts(cells.Skip(1).ToList())
            };
        }*/

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

        private static List<Host> ExtractLinkHosts(List<HtmlNode> cells)
        {
            // Extract link hosts from the given cells
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

        private static List<Host> MatchFormatsWithLinks(List<string> mediaFormats, string[] linkGroups)
        {
            var linkHosts = new List<Host>();

            // Match each format with its corresponding group of links
            for (int i = 0; i < mediaFormats.Count && i < linkGroups.Length; i++)
            {
                var mediaFormat = mediaFormats[i];
                var links = ExtractLinksFromGroup(linkGroups[i]);

                if (links != null)
                {
                    linkHosts.AddRange(CreateLinkHostsForFormat(mediaFormat, links));
                }
            }

            return linkHosts;
        }

        private static HtmlNodeCollection ExtractLinksFromGroup(string linkGroupHtml)
        {
            // Extract links from a given group of links (HTML content)
            return HtmlNode.CreateNode($"<div>{linkGroupHtml}</div>").SelectNodes(".//a");
        }

        private static IEnumerable<Host> CreateLinkHostsForFormat(string mediaFormat, HtmlNodeCollection links)
        {
            // Create LinkHost objects for each link under the current media format
            foreach (var link in links)
            {
                yield return new Host
                {
                    UnresolvedUrl = link.GetAttributeValue("href", string.Empty),
                    HostName = link.InnerText.Trim(),
                    RawQualityFormat = mediaFormat
                };
            }
        }

        private static (int start, int end) ParseEpisodeRange(string episodeText)
        {
            var parts = episodeText.Split('-').Select(int.Parse).ToArray();
            return (parts[0], parts[1]);
        }
    }
}
