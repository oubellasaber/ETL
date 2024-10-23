using BLL.Services.DownloadLinkExtractionService.Impl;
using BLL.Services.DownloadLinkService;

namespace BLL.Services.DownloadLinkExtractionService
{
    internal class DownloadLinkExtractorFactory
    {
        public IDownloadLinkExtractor GetDownloadLinkExtractor(string domain, string downloadUrl)
        {
            switch (domain)
            {
                case "datanodes.to":
                    return new DataNodesDownloadLinkExtractor();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
