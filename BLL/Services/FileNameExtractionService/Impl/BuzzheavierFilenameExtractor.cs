using BLL.Services.FileNameExtractionService;
using HtmlAgilityPack;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    internal class BuzzheavierFilenameExtractor : IFilenameExtractor
    {
        public async Task<string?> ExtractFilenameAsync(string downloadUrl)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc = web.Load(downloadUrl);

            string? filename = htmlDoc.DocumentNode.SelectSingleNode(".//html/body/div[1]/div[2]/div/p/span")?.InnerText;

            return filename;
        }
    }
}
