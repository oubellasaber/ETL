using BLL.Services.FileNameExtractionService;
using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    public class GofileFilenameExtractor : IFilenameExtractor
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
            await page.WaitForSelectorAsync(".contentName");

            var filename = await page.EvaluateExpressionAsync<string>("document.querySelector('.contentName').innerText");
            return filename;
        }
    }
}
