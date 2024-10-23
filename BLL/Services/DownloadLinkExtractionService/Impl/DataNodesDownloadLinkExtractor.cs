using System.Net;
using BLL.Services.DownloadLinkService;

namespace BLL.Services.DownloadLinkExtractionService.Impl
{
    public class DataNodesDownloadLinkExtractor : IDownloadLinkExtractor
    {
        private readonly HttpClient _client;

        public DataNodesDownloadLinkExtractor()
        {
            // Create a custom HttpClientHandler to disable automatic redirection
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false, // Disable auto-redirect to handle 302 manually
            };

            // Set up HttpClient with the custom handler
            _client = new HttpClient(handler);
        }

        public async Task<string?> ExtractDownloadFileAsync(string url)
        {
            string fileId = ExtractFileIdFromUrl(url);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://datanodes.to/download");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("op", "download2"),
                new KeyValuePair<string, string>("id", fileId)
            });

            requestMessage.Content = content;

            // Send the request
            var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            // Check if the response is a redirect (status code 302)
            if (response.StatusCode == HttpStatusCode.Found)
            {
                // Return the Location URL from headers (redirect URL)
                if (response.Headers.Location != null)
                {
                    return response.Headers.Location.AbsoluteUri;
                }
            }

            // Return null if there was no redirect
            return null;
        }

        private string ExtractFileIdFromUrl(string fileUrl)
        {
            try
            {
                Uri uri = new Uri(fileUrl);
                string fileId = uri.Segments[^1]; // ^1 gets the last segment
                return fileId;
            }
            catch (UriFormatException)
            {
                return null; // Invalid URL
            }
        }
    }
}
