using BLL.Services.FileNameExtractionService;
using PuppeteerSharp;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    public class DatenodesFilenameExtractor : IFilenameExtractor
    {
        public async Task<string?> ExtractFilenameAsync(string downloadUrl)
        {
            await new BrowserFetcher().DownloadAsync(); // No need to specify revision.

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            using var page = await browser.NewPageAsync();

            await page.GoToAsync(downloadUrl, WaitUntilNavigation.Networkidle2);
            await page.WaitForSelectorAsync("h1");

            var filename = await page.EvaluateExpressionAsync<string>("document.querySelector('h1').innerText");
            return filename;
        }
    }
}
