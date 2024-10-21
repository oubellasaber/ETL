using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract.Utility
{
    public static class TableRowExtensions
    {
        private static bool IsTableRow(this HtmlNode node)
        {
            return node.NodeType == HtmlNodeType.Element && node.Name.Equals("tr", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsHeader(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            if (cells == null || cells.Count != 3)
                return false;

            return cells[0].InnerText.Contains("ep", StringComparison.OrdinalIgnoreCase) ||
                   cells[1].InnerText.Contains("quality", StringComparison.OrdinalIgnoreCase) ||
                   cells[2].InnerText.Contains("download", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsSingleEp(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var epCell = tr.SelectSingleNode(".//td");

            if (epCell == null || string.IsNullOrWhiteSpace(epCell.InnerText))
                return false;

            return int.TryParse(epCell.InnerText.Trim(), out _);
        }

        public static bool IsRangedEp(this HtmlNode tr, int lastEpisode)
        {
            if (!tr.IsTableRow())
                return false;

            var epCell = tr.SelectSingleNode(".//td");

            if (epCell == null || string.IsNullOrWhiteSpace(epCell.InnerText))
                return false;

            string epCellInnerText = epCell.InnerText.Trim();
            string expectedRange = $"01-{lastEpisode:00}";

            return epCellInnerText.Contains(expectedRange, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsSpecialSingleEp(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var epCell = tr.SelectSingleNode(".//td");

            if (epCell == null || string.IsNullOrWhiteSpace(epCell.InnerText))
                return false;

            return epCell.InnerText.Contains("special", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEmptyRow(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            return cells != null && cells.All(cell => string.IsNullOrEmpty(cell.InnerText));
        }

        public static bool IsPasswordRow(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            if (cells == null)
                return false;

            string passwordText = "Password: dramaday.net";

            foreach (var cell in cells)
            {
                if (cell.InnerText.Contains(passwordText, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static (bool isAdditionalInfoRow, string additionalInfo) IsAdditionalInfoRow(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return (false, string.Empty);

            var cells = tr.SelectNodes(".//td");

            if (cells == null)
                return (false, string.Empty);

            foreach (var cell in cells)
            {
                if (!string.IsNullOrEmpty(cell.GetAttributeValue("data-colspan", string.Empty)))
                {
                    return (true, cell.InnerText.Trim());
                }
            }

            return (false, string.Empty);
        }
    }
}
