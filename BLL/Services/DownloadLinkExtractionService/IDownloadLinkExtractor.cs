namespace BLL.Services.DownloadLinkService
{
    internal interface IDownloadLinkExtractor
    {
        public Task<string?> ExtractDownloadFileAsync(string url);
    }
}