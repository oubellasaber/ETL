using BLL.Services.FileNameExtractionService;
using HtmlAgilityPack;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    internal class PixeldrainFilenameExtractor : IFilenameExtractor
    {
        public async Task<string?> ExtractFilenameAsync(string downloadUrl)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc = web.Load(downloadUrl);

            string? filename = htmlDoc.DocumentNode.SelectSingleNode(".//*[@id=\"body\"]/div/div[1]/div[2]")?.InnerText;

            return filename;
        }
    }
}
