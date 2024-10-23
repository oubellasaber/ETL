using BLL.Services.DownloadLinkService;
using BLL.Services.FileNameExtractionService;
using FilenameExtractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.DownloadLinkExtractionService
{
    internal class DownloadLinkExtractor
    {
        private static readonly string[] _supportedDownloadServices =
        {
            "datanodes.to",
        };

        public string DownloadUrl { get; private set; }
        public string? Domain { get; private set; }
        private IDownloadLinkExtractor _extr;

        private DownloadLinkExtractor(string downloadUrl, IDownloadLinkExtractor dlExtractor)
        {
            DownloadUrl = downloadUrl;
            _extr = dlExtractor;
        }

        public static DownloadLinkExtractor? Instance(string downloadUrl)
        {
            if (!IsDownloadUrlSupported(ref downloadUrl, out string? domain))
                return null;

            IDownloadLinkExtractor dlExtractor = (new DownloadLinkExtractorFactory()).GetDownloadLinkExtractor(domain!, downloadUrl);

            return new DownloadLinkExtractor(downloadUrl, dlExtractor);
        }

        public async Task<string?> ExtractDownloadLink() => await _extr.ExtractDownloadFileAsync(DownloadUrl);

        private static bool IsDownloadUrlSupported(ref string downloadUrl, out string? domain)
        {
            domain = ExtractDomain(downloadUrl);

            if (string.IsNullOrEmpty(domain) || !_supportedDownloadServices.Contains(domain))
                return false;

            return true;
        }

        private static string? ExtractDomain(string downloadUrl)
        {
            // Add "https://" if the scheme is missing
            if (!downloadUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                downloadUrl = "https://" + downloadUrl;
            }

            // Validate and create URI
            if (Uri.TryCreate(downloadUrl, UriKind.Absolute, out Uri? uri))
            {
                string host = uri.Host;

                if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                {
                    host = host.Substring(4); // Remove the first 4 characters ('www.')
                }

                return host; // Return normalized domain
            }

            // Return null if the URL is invalid
            return null;
        }
    }
}
