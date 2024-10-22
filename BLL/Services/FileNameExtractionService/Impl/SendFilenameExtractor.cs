using BLL.Services.FileNameExtractionService;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNameExtractors.FilenameExtrctors.Impl
{
    internal class SendFilenameExtractor : IFilenameExtractor
    {
        public async Task<string?> ExtractFilenameAsync(string downloadUrl)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc = web.Load(downloadUrl);

            string? filename = htmlDoc.DocumentNode.SelectSingleNode(".//html/body/div[1]/div/div/div[2]/h6")?.InnerText;

            return filename;
        }
    }
}
