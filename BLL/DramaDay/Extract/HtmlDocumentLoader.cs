using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract
{
    public class HtmlDocumentLoader
    {
        private readonly HttpClient _httpClient;
        private readonly HtmlDocument _document;

        private HtmlDocumentLoader(HttpClient httpClient, HtmlDocument document)
        {
            _httpClient = httpClient;
            _document = document;
        }

        public static async Task<HtmlDocumentLoader> HtmlDocumentLoaderAsync(HttpClient httpClient, string url)
        {
            return new HtmlDocumentLoader(httpClient, await LoadDocument(httpClient, url));
        }

        private static async Task<HtmlDocument> LoadDocument(HttpClient httpClient, string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error fetching document: {response.StatusCode}");
                }

                var htmlContent = await response.Content.ReadAsStringAsync();

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                return htmlDocument;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load document from {url}", ex);
            }
        }

        public HtmlNode GetNode(string xpath) => _document.DocumentNode.SelectSingleNode(xpath);
    }
}
