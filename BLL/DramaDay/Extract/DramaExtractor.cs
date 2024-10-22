using BLL.DramaDay.Extract.Models;
using BLL.DramaDay.Extract.Utility;
using HtmlAgilityPack;

namespace BLL.DramaDay.Extract
{
    public class DramaExtractor
    {
        public readonly HtmlDocumentLoader _htmlDocLoader;
        private List<HtmlNode> _rows;
        public Drama drama { get; set; }
        private string key = "Ohhhh";

        public static async Task<DramaExtractor> BuildDramaDataExtractorAsync(string dramaUrl)
        {
            HtmlDocumentLoader htmlDocLoader = await HtmlDocumentLoader.HtmlDocumentLoaderAsync(new HttpClient(), dramaUrl);

            return new DramaExtractor(htmlDocLoader, dramaUrl);
        }

        private DramaExtractor(HtmlDocumentLoader htmlDocLoader, string dramaUrl)
        {
            _htmlDocLoader = htmlDocLoader;
            _rows = RemoveUncesseryRowsFromTable(_htmlDocLoader.GetNode("//table").SelectNodes("//tbody/tr").ToList());
            Extract(dramaUrl);
        }

        /*
         Episode episode = RowDataExtractor.ExtractSingle(row);
                    if(!_stdDramasDic.ContainsKey(key))
                    {
                        _stdDramasDic.Add(key, new StandardDrama());
                    }
                    _stdDramasDic[key].Episodes.Add(episode);
         */

        public void Extract(string url)
        {
            DramaGeneralDataExtractor ext = new DramaGeneralDataExtractor(_htmlDocLoader.GetNode("//div[@class='wpb_wrapper']/p"), url);
            drama = ext.GetDramaWithGeneralData();

            foreach (var row in _rows)
            {
                CheckIfKeyExist();

                if (row.IsSingleEp())
                {
                    StandardEp standardEp = RowDataExtractor.ExtractSingle(row);
                    drama.StandardEpsDictionary[key].Add(standardEp);
                }

                // handle the rest here
            }
        }

        private void CheckIfKeyExist()
        {
            if (!drama.StandardEpsDictionary.ContainsKey(key))
            {
                drama.StandardEpsDictionary.Add(key, new List<StandardEp>());
            }
        }

        private List<HtmlNode> RemoveUncesseryRowsFromTable(List<HtmlNode> rows)
        {
            if (rows[0].IsHeader())
            {
                rows.RemoveAt(0);
            }

            rows.RemoveAll(r => r.IsEmptyRow() || r.IsPasswordRow());

            return rows;
        }
    }
}
