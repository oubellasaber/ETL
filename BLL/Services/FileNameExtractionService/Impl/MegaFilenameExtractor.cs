using BLL.Services.FileNameExtractionService;
using PuppeteerSharp;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    public class MegaFilenameExtractor : IFilenameExtractor
    {
        public async Task<string?> ExtractFilenameAsync(string downloadUrl)
        {
            // Ensure Puppeteer is set up (downloads Chromium if not already available)
            await new BrowserFetcher().DownloadAsync(); // No need to specify revision.

            // Launch a headless browser
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true // Set to false if you want to see the browser UI for debugging
            });

            // Open a new page
            using var page = await browser.NewPageAsync();

            // Go to the provided download URL
            await page.GoToAsync(downloadUrl, WaitUntilNavigation.Networkidle2);

            // Wait for the filename and extension elements to appear (you might need to tweak the selector)
            await page.WaitForSelectorAsync(".filename");
            await page.WaitForSelectorAsync(".extension");

            // Extract the text from the relevant elements
            var filename = await page.EvaluateExpressionAsync<string>("document.querySelector('.filename').innerText");
            var extension = await page.EvaluateExpressionAsync<string>("document.querySelector('.extension').innerText");

            // Combine filename and extension
            return filename + extension;
        }
    }
}
