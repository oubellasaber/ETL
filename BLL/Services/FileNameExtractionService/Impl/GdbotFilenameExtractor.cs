using BLL.Services.FileNameExtractionService;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    internal class GdbotFilenameExtractor : IFilenameExtractor
    {
        public async Task<string?> ExtractFilenameAsync(string downloadUrl)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc = web.Load(downloadUrl);

            string? filename = htmlDoc.DocumentNode.SelectSingleNode(".//h1")?.InnerText.Replace("Name : ", string.Empty);

            return filename;
        }
    }
}
